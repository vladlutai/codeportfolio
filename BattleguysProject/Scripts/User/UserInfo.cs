using UnityEngine;

namespace User
{
    public static class UserInfo
    {
        public static string Nickname
        {
            get
            {
                if (string.IsNullOrEmpty(_nickname))
                {
                    GenerateDefaultNickname();
                }

                return _nickname;
            }
        }

        private static string _nickname;

        public static void ChangeNickname(string newNickname)
        {
            _nickname = newNickname;
        }

        private static void GenerateDefaultNickname()
        {
            _nickname = $"Player{Random.Range(0, byte.MaxValue)}";
        }
    }
}