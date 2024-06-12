using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents;
using UnityEngine;

/*
mlagents-learn "D:\GitHub\Snowball.io_CopyWithML\ml-agents-release_20\config\ppo\SnowBall.yaml" --run-id=test20 --results-dir="D:\GitHub\Snowball.io_CopyWithML\Results"
 */

// MLAgent 학습을 위한 스크립트입니다.
public class SnowBallAgent : Agent
{
    private AgentContoller agentController;
    private SnowArea snowArea;

    [SerializeField] private bool isLearning;

    private void InitPositionAndRotation()
    {
        transform.localPosition = new Vector3(Random.Range(-6f, 11f), 0.5f, Random.Range(-3f, 13f));
        transform.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
    }

    public override void Initialize()
    {
        agentController = GetComponent<AgentContoller>();
        snowArea = transform.root.GetComponentInChildren<SnowArea>();
    }

    public override void OnEpisodeBegin()
    {
        if (isLearning)
        {
            snowArea.InitArea();
            InitPositionAndRotation();
            agentController.SetVelocity(Vector3.zero);
        }

        Debug.Log("Episode Begin");
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var discreteActions = actions.DiscreteActions;
        Vector3 moveVelocity = Vector3.zero;
        
        switch (discreteActions[0])
        {
            case 0:
                moveVelocity += Vector3.zero; // 정지
                break;
            case 1:
                moveVelocity += transform.forward; // 앞
                break;
            case 2:
                moveVelocity += -transform.forward; // 뒤
                break;
            case 3:
                moveVelocity += -transform.right; // 좌
                break;
            case 4:
                moveVelocity += transform.right; // 우
                break;
        }

        Debug.Log(moveVelocity);

        if (moveVelocity != Vector3.zero)
        {
            agentController.SetVelocity(moveVelocity);

        }

        GiveReward(-0.00001f);
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

        if (collision.transform.CompareTag("Wall"))
        {
            AddReward(-1f);
            EndEpisode();
        }
    }

}
