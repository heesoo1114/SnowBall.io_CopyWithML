using UnityEngine;

public class InputHandler : MonoSingleton<InputHandler>
{
    private AgentContoller controller;

    [Header("Input")]
    [SerializeField] private FixedJoystick joystick;

    public override void Init()
    {
        if (GameManager.Instance.PlayerTransform != null)
        {
            controller = GameManager.Instance.PlayerTransform.GetComponent<AgentContoller>();
        }
    }

    private void Update()
    {
        // TouchInput();

        if (controller != null)
        {
            MoveInput();
        }
    }

    private void MoveInput()
    {
        float xInput = joystick.Horizontal;
        float zInput = joystick.Vertical;
        Vector3 moveInput = new Vector3(xInput, 0, zInput);

        controller.SetVelocity(moveInput);
    }

    public void InitInput()
    {
        joystick.InitInputAndHandle();
    }

}
