#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [InitializeOnLoad]
    public class AssemblyRecompileTimeReporter : EditorWindow
    {
        private static bool _isTrackingTime;
        private static double _startTime;

        #region Constants
        private const string CompileStartTimeSaveKey = "CompileStartTime";
        #endregion

        static AssemblyRecompileTimeReporter()
        {
            EditorApplication.update += EditorUpdate;
            _startTime = PlayerPrefs.GetFloat(CompileStartTimeSaveKey);
            if (_startTime > 0)
            {
                _isTrackingTime = true;
            }
        }

        private static void EditorUpdate()
        {
            if (EditorApplication.isCompiling && !_isTrackingTime)
            {
                _startTime = EditorApplication.timeSinceStartup;
                PlayerPrefs.SetFloat(CompileStartTimeSaveKey, (float)_startTime);
                _isTrackingTime = true;
            }
            else if (!EditorApplication.isCompiling && _isTrackingTime)
            {
                var finishTime = EditorApplication.timeSinceStartup;
                _isTrackingTime = false;
                var compileTime = finishTime - _startTime;
                PlayerPrefs.DeleteKey(CompileStartTimeSaveKey);
                ClearLog();
                Debug.Log($"Compile Finished: <color=yellow><b>{compileTime:F2}s</b></color> {DateTime.Now:HH:mm:ss}");
            }
        }

        private static void ClearLog()
        {
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method?.Invoke(new object(), null);
        }
    }
}
#endif