using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

namespace Player.AI
{
    public class PlayerAi : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private new Camera camera;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private PlayerInputController playerInputController;
#pragma warning restore 649

        private void Start()
        {
            navMeshAgent.updateRotation = false;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    navMeshAgent.SetDestination(hit.point);
                }
            }
            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                var desiredVelocity = navMeshAgent.desiredVelocity;
                playerInputController.move = new Vector2(desiredVelocity.x, desiredVelocity.z);
            }
            else
            {
                playerInputController.move = Vector2.zero;
            }
        }
    }
}