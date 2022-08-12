using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player.AI.Gym
{
    public class MoveToGoalAgent : Agent
    {
#pragma warning disable 649
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material winMaterial;
        [SerializeField] private Material loseMaterial;
        [SerializeField] private MeshRenderer floorMeshRenderer;
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private Transform targetTransformPosition;
#pragma warning restore 649

        public override void OnEpisodeBegin()
        {
            transform.localPosition = new Vector3(Random.Range(-3.2f, 3.2f), 0, Random.Range(-3f, 3f));
            targetTransformPosition.localPosition = new Vector3(Random.Range(-3.2f, 3.2f), 0, Random.Range(-3f, 3f));
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            float moveX = actions.ContinuousActions[0];
            float moveZ = actions.ContinuousActions[1];
            transform.localPosition += new Vector3(moveX, 0f, moveZ) * Time.deltaTime * moveSpeed;
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
            continuousActions[0] = Input.GetAxisRaw("Horizontal");
            continuousActions[1] = Input.GetAxisRaw("Vertical");
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(transform.localPosition);
            sensor.AddObservation(targetTransformPosition.localPosition);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("MlAgentTarget"))
            {
                floorMeshRenderer.material = winMaterial;
                SetReward(1f);
                EndEpisode();
            }
            if (other.CompareTag("MlAgentWall"))
            {
                floorMeshRenderer.material = loseMaterial;
                SetReward(-1f);
                EndEpisode();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("MlAgentTarget"))
            {
                floorMeshRenderer.material = winMaterial;
                SetReward(1f);
                EndEpisode();
            }
        }
    }
}