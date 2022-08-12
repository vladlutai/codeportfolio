using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Nickname
{
    public class NicknameProvider : MonoBehaviour
    {
#pragma warning disable 649
    
#pragma warning restore 649

        #region Constnts
        private const string NicknameDatabasePath = "Databases/Nickname/NicknamesMixed_";
        private readonly (int, int) _nicknameDatabaseIndexRange = (1, 100);
        private const string DefaultNickname = "Player";
        private readonly (int, int) _defaultNicknameIdRange = (0, Int16.MaxValue);
        #endregion

        private TextAsset _databaseTextAsset;
        private string _database;
        private Coroutine _loadDataBaseAsyncCoroutine;

        private void Awake()
        {
            LoadRandomDatabase();
        }

        private void LoadRandomDatabase()
        {
            if (_loadDataBaseAsyncCoroutine == null)
            {
                UnloadCurrentDatabase();
                int databaseIndex = Random.Range(_nicknameDatabaseIndexRange.Item1, _nicknameDatabaseIndexRange.Item2 + 1);
                _loadDataBaseAsyncCoroutine = StartCoroutine(LoadDatabaseAsync(NicknameDatabasePath + databaseIndex));
            }
        }

        private IEnumerator LoadDatabaseAsync(string path)
        {
            ResourceRequest resourceRequest = Resources.LoadAsync(path);
            while (!resourceRequest.isDone)
            {
                yield return null;
            }
            _databaseTextAsset = resourceRequest.asset as TextAsset;
            _database = !ReferenceEquals(_databaseTextAsset, null) ? _databaseTextAsset.text : null;
            if (_database == null)
            {
                Debug.LogError(MethodBase.GetCurrentMethod().Name);
            }
            _loadDataBaseAsyncCoroutine = null;
        }

        private void UnloadCurrentDatabase()
        {
            if (!ReferenceEquals(_databaseTextAsset, null))
            {
                Resources.UnloadAsset(_databaseTextAsset);
            }
        }

        public string GetNickname()
        {
            if (_database == null)
                return DefaultNickname + Random.Range(_defaultNicknameIdRange.Item1, _defaultNicknameIdRange.Item2);
            int randomPoint = GetRandomPoint();
            while (_database[randomPoint] == '\n')
            {
                randomPoint = GetRandomPoint();
            }
            string nickname = String.Empty;
            (int, int) nickNameRange = (_database.LastIndexOf('\n', randomPoint) + 1, _database.IndexOf('\n', randomPoint));
            for (int i = nickNameRange.Item1; i < nickNameRange.Item2; i++)
            {
                nickname += _database[i];
            }
            return nickname;
        }

        private int GetRandomPoint()
        {
            return Random.Range(0, _database.Length);
        }
    }
}