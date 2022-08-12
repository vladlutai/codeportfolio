using System;
using System.Collections;
using System.Collections.Generic;
using MatchMaking;
using Photon.Pun;
using Player;
using UnityEngine;

namespace GameModes.CaptureZone
{
    public class CaptureZone : MonoBehaviour, IPunObservable
    {
        public enum CaptureZoneType
        {
            Default,
            Unoccupied,
            Occupying,
            Occupied
        }

        [Serializable]
        private class CaptureZoneState
        {
#pragma warning disable 649
            [SerializeField] private CaptureZoneType captureZoneType;
            [SerializeField] private GameObject particleGameObject;
#pragma warning restore 649
            
            public CaptureZoneType CaptureZoneType => captureZoneType;
            public GameObject ParticleGameObject => particleGameObject;

            public void SetActive(bool value)
            {
                particleGameObject.SetActive(value);
            }
        }
#pragma warning disable 649
        [SerializeField] private MatchProvider.TeamEnum teamEnum;
        [SerializeField] private List<CaptureZoneState> captureZoneStatesList;
#pragma warning restore 649

        #region Constants
        private const float OccupyingTime = 7f;
        #endregion

        private CaptureZoneType _currentCaptureZoneType = CaptureZoneType.Default;
        private Coroutine _occupyingCoroutine;
        
        public static event Action<int> OnPlayerCaptureEvent;

        public void SetType(CaptureZoneType newType)
        {
            if(_currentCaptureZoneType == newType)
                return;
            _currentCaptureZoneType = newType;
            foreach (var captureZoneState in captureZoneStatesList)
            {
                captureZoneState.SetActive(captureZoneState.CaptureZoneType == newType);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Player") || _currentCaptureZoneType == CaptureZoneType.Occupied)
                return;
            
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController.TeamEnum != teamEnum)
                StartOccupyingTimer(OccupyingTime,
                    () => OnPlayerCaptureEvent?.Invoke(playerController.PhotonView.ViewID));
        }

        private void OnTriggerExit(Collider other)
        {
            if(!other.CompareTag("Player") || _currentCaptureZoneType == CaptureZoneType.Occupied)
                return;

            if (_occupyingCoroutine != null)
            {
                StopCoroutine(_occupyingCoroutine);
                _occupyingCoroutine = null;
            }
            SetType(CaptureZoneType.Unoccupied);
        }

        private void StartOccupyingTimer(float time, Action onTimerDone)
        {
            _occupyingCoroutine ??= StartCoroutine(OccupyingTimer(time, onTimerDone));
        }

        private IEnumerator OccupyingTimer(float time, Action onTimerDone)
        {
            SetType(CaptureZoneType.Occupying);
            float timer = time;
            while ((timer -= Time.fixedDeltaTime) > 0f)
            {
                yield return new WaitForFixedUpdate();
            }
            onTimerDone?.Invoke();
            SetType(CaptureZoneType.Occupied);
            _occupyingCoroutine = null;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_currentCaptureZoneType);
            }
            else if (stream.IsReading)
            {
                SetType((CaptureZoneType) stream.ReceiveNext());
            }
        }
    }
}