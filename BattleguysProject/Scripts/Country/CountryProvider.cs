using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace Country
{
    public class CountryProvider : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private CountryData countryData;
#pragma warning restore 649

        #region Constants
        private const string NativeCountryRequestUrl = "https://delivery.x-mediate.com/mediation/api/v2/mediate/gdpr";
        private const string NativeCountryCodeSaveKey = "NativeCountry";
        #endregion

        private Coroutine _nativeCountryRequestCoroutine;
        private CountryCode _nativeCountryCode = CountryCode.Un;

        public CountryCode NativeCountryCode
        {
            get => _nativeCountryCode;
            private set
            {
                _nativeCountryCode = value;
                PlayerPrefs.SetInt(NativeCountryCodeSaveKey, (int) _nativeCountryCode);
            }
        }

        private void Awake()
        {
            Init();
            GetNativeCountry();
        }

        private void Init()
        {
            _nativeCountryCode = (CountryCode) PlayerPrefs.GetInt(NativeCountryCodeSaveKey, (int) CountryCode.Un);
        }
        
        private void GetNativeCountry()
        {
            if (!PlayerPrefs.HasKey(NativeCountryCodeSaveKey))
                _nativeCountryRequestCoroutine ??= StartCoroutine(NativeCountryRequest());
        }

        private IEnumerator NativeCountryRequest()
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(NativeCountryRequestUrl);
            yield return unityWebRequest.SendWebRequest();
            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                NativeCountryResponse deserializedNativeCountryResponse =
                    DeserializeNativeCountryResponse(unityWebRequest.downloadHandler.text);
                if (deserializedNativeCountryResponse != null)
                {
                    ParseNativeCountryCode(deserializedNativeCountryResponse.CountryCodeNormalized);
                }
            }
            _nativeCountryRequestCoroutine = null;
        }

        private NativeCountryResponse DeserializeNativeCountryResponse(string response)
        {
            return JsonUtility.FromJson<NativeCountryResponse>(response);
        }

        private void ParseNativeCountryCode(string countryCode)
        {
            if (Enum.TryParse(countryCode, out CountryCode countryCodeParsed))
            {
                NativeCountryCode = countryCodeParsed;
            }
        }

        public Sprite GetCountrySprite(CountryCode countryCode)
        {
            return countryData.CountrySpritesList[(int) countryCode];
        }

        public Sprite GetRandomCountrySprite()
        {
            return countryData.CountrySpritesList[Random.Range(0, countryData.CountrySpritesList.Count)];
        }
    }

    [Serializable]
    public class NativeCountryResponse
    {
        [SerializeField] private string ip_address;
        [SerializeField] private string country_name;
        [SerializeField] private string country_code;
        [SerializeField] private int is_gdpr_country;
        public string CountryCodeNormalized => GetNormalizedCountryCode();

        public override string ToString()
        {
            return
                $"IpAddress:{ip_address}\nCountryName:{country_name}\nCountryCode:{country_code}\nIsGdprCountry:{is_gdpr_country}";
        }

        private string GetNormalizedCountryCode()
        {
            string countryCodeNormalized = country_code[0].ToString().ToUpper() + country_code[1].ToString().ToLower();
            return countryCodeNormalized;
        }
    }
}