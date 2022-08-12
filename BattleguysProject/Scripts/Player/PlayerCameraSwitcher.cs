using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Player
{
    public class PlayerCameraSwitcher : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private CinemachineVirtualCamera playerVirtualCamera;
        [SerializeField] private List<CinemachineVirtualCamera> mapViewVirtualCamerasList;
#pragma warning restore 649

        public void SetVirtualCameraTarget(Transform target)
        {
            playerVirtualCamera.Follow = target;
            playerVirtualCamera.LookAt = target;
        }
    }
}