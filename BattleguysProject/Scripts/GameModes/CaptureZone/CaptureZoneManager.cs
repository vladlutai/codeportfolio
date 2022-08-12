using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace GameModes.CaptureZone
{
    public class CaptureZoneManager : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private List<CaptureZone> captureZonesList;
#pragma warning restore 649

        private void Start()
        {
            SetActiveCaptureZones(false, CaptureZone.CaptureZoneType.Unoccupied);
        }

        private void OnEnable()
        {
            EventManager.StartListening(Constants.MatchStarted, OnMatchStarted);
            EventManager.StartListening(Constants.MatchEnded, OnMatchEnded);
        }

        private void OnDisable()
        {
            EventManager.StopListening(Constants.MatchStarted, OnMatchStarted);
            EventManager.StopListening(Constants.MatchEnded, OnMatchEnded);
        }

        private void OnMatchStarted()
        {
            SetActiveCaptureZones(true, CaptureZone.CaptureZoneType.Unoccupied);
        }

        private void OnMatchEnded()
        {
            SetActiveCaptureZones(false, CaptureZone.CaptureZoneType.Unoccupied);
        }

        private void SetActiveCaptureZones(bool value, CaptureZone.CaptureZoneType type)
        {
            foreach (var captureZone in captureZonesList)
            {
                captureZone.gameObject.SetActive(value);
                captureZone.SetType(type);
            }
        }
    }
}