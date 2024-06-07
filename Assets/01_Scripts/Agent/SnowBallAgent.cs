using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents;
using UnityEngine;

// MLAgent 학습을 위한 스크립트입니다.
public class SnowBallAgent : Agent
{
    private AgentContoller agentController;
    private SnowArea snowArea;

    private void OnEnable()
    {
        InitPositionAndRotation();
    }

    private void InitPositionAndRotation()
    {
        transform.localPosition = new Vector3(Random.Range(-6f, 11f), 0.5f, Random.Range(-3f, 13f));
        transform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
    }

    public override void Initialize()
    {
        MaxStep = 1000000;
        agentController = GetComponent<AgentContoller>();
        snowArea = transform.root.GetComponentInChildren<SnowArea>();
    }

    public override void OnEpisodeBegin()
    {
        snowArea.InitArea();
        InitPositionAndRotation();
        agentController.SetVelocity(Vector3.zero);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var continuousActions = actions.ContinuousActions;
        Vector3 moveVelocity = new Vector3(continuousActions[0], 0, continuousActions[1]);

        // float moveForward = continuousActions[0]; // -1 to 1
        // float turn = continuousActions[1]; // -1 to 1

        // Vector3 move = transform.forward * moveForward;
        // Vector3 rotation = Vector3.up * turn * _turnSpeed * Time.fixedDeltaTime;

        // _rigidbody.MovePosition(_rigidbody.position + move);
        // _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(rotation));

        agentController.SetVelocity(moveVelocity);

        AddReward(-1 / (float)MaxStep);
    }

    public void GiveReward(float rewardAmount)
    {
        AddReward(rewardAmount);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Water"))
        {
            AddReward(-1f);
            EndEpisode();
        }

        if (collision.transform.CompareTag("SnowBall"))
        {
            AddReward(-1f);
        }
    }

}
