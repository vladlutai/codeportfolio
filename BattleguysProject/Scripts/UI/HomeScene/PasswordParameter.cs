using System;
using TMPro;
using UnityEngine;

namespace UI.HomeScene
{
    public class PasswordParameter : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private TMP_InputField passwordTextMeshPro;
        [SerializeField] private CanvasGroup passwordFieldCanvasGroup;
#pragma warning restore 649

        public void AccessParameterChanged(string access)
        {
            passwordTextMeshPro.text = "";
            switch (access)
            {
                case "Public":
                    passwordFieldCanvasGroup.interactable = false;
                    break;
                
                case "Private":
                    passwordFieldCanvasGroup.interactable = true;
                    break;
            }
        }
    }
}