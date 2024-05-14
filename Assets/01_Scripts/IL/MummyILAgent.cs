using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents;
using UnityEngine;
using System.Collections;

public class MummyILAgent : Agent
{
    public float moveSpeed = 10f;
    public float turnSpeed = 50f;

    private ColorTarget colorTargeter;
    private Rigidbody rigidBody;
    private Vector3 originPos;

    public Material goodMat;
    public Material badMat;
    private Material originMat;
    private Renderer floorRenderer;

    public override void Initialize()
    {
        colorTargeter = transform.parent.GetComponent<ColorTarget>();
        rigidBody = GetComponent<Rigidbody>();
        originPos = transform.localPosition;
        floorRenderer = transform.parent.Find("Floor").GetComponent<Renderer>();
        originMat = floorRenderer.material;
    }

    public override void OnEpisodeBegin()
    {
        colorTargeter.TargetingColor();

        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        transform.localPosition = originPos;
        transform.localRotation = Quaternion.identity;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var DiscreteActions = actions.DiscreteActions;

        Vector3 direction = Vector3.zero;
        Vector3 rotationAxis = Vector3.zero;

        switch (DiscreteActions[0])
        {
            case 1:
                direction = transform.forward;
                break;
            case 2:
                direction = -transform.forward;
                break;
        }

        switch (DiscreteActions[1])
        {
            case 1:
                rotationAxis = Vector3.down;
                break;
            case 2:
                rotationAxis = Vector3.up;
                break;
        }

        rigidBody.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(rotationAxis, turnSpeed * Time.fixedDeltaTime);

        AddReward(-0.01f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var DiscreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.W))
        {
            DiscreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            DiscreteActionsOut[0] = 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            DiscreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            DiscreteActionsOut[1] = 2;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(colorTargeter.targetColor.ToString()))
        {
            StartCoroutine(ChangeFloorColor(goodMat));
            AddReward(1f);
            EndEpisode();
        }
        else if (collision.collider.CompareTag("HINT"))
        {

        }
        else
        {
            StartCoroutine(ChangeFloorColor(badMat));
            AddReward(-1f);
            EndEpisode();
        }
    }

    private IEnumerator ChangeFloorColor(Material changeMat)
    {
        floorRenderer.material = changeMat;
        yield return new WaitForSeconds(0.2f);
        floorRenderer.material = originMat;
    }
}
