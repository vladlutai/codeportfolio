using System;
using UnityEngine;

namespace Analytics
{
    public class AnalyticsController : MonoBehaviour
    {
        [Serializable]
        public class CredentialKeys
        {
            public bool isFirebaseEnabled = true;
            public bool isAppsflyerEnabled = true;
        }
        
#pragma warning disable 649
    
#pragma warning restore 649
    }
}