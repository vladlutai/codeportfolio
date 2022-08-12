using System.IO;
using UnityEngine;

namespace Saves
{
    public abstract class AbstractSaveSystem : MonoBehaviour
    {
        #region Constants
        public const string SaveFileExtension = ".dat";
        #endregion

        private void OnApplicationFocus(bool focus)
        {
            if (!focus)
            {
                SaveData();
            }

        }

        protected abstract void SaveData();

        protected abstract void LoadAndInitData();

        protected void Save<T>(T saves, string savesPath)
        {
            SerializeUtility.SerializeUtility.Serialize(saves, GetSaveFilePath(savesPath));
        }

        protected T Load<T>(string savesPath)
        {
            return SerializeUtility.SerializeUtility.Deserialize<T>(GetSaveFilePath(savesPath));
        }

        private string GetSaveFilePath(string fileName)
        {
            string saveFilePath;
#if UNITY_EDITOR
            saveFilePath = Path.Combine(Application.dataPath, "Data", "Saves", fileName + SaveFileExtension);
#else
        saveFilePath = Path.Combine(Application.persistentDataPath, fileName + SaveFileExtension);
#endif
            return saveFilePath;
        }
    }
}