using UnityEngine;

namespace Player
{
    public class SpawnPoint : MonoBehaviour
    {
        public enum StateEnum
        {
            UnOccupied,
            Occupied
        }
#pragma warning disable 649
    
#pragma warning restore 649

        private StateEnum _state = StateEnum.UnOccupied;

        public StateEnum State => _state;

        public void UpdateState(StateEnum newStateEnum)
        {
            _state = newStateEnum;
        }
    }
}