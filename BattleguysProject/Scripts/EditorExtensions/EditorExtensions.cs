#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class EditorExtensions
{
    [MenuItem("Custom/Clear save data")]
    private static void ClearSaveData()
    {
        DirectoryInfo di = new DirectoryInfo(Path.Combine(Application.dataPath, "Data", "Saves"));

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }        
        PlayerPrefs.DeleteAll();
        Debug.Log("Save data cleared");
    }
}
#endif