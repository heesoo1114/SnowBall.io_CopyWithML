using UnityEngine;

public class SnowBall : PoolableMono
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    private Vector3 initPosition;
    private Vector3 rotateDirection;

    private void Awake()
    {
        initPosition = transform.position;
    }

    public override void OnPop()
    {
        transform.position = initPosition;
        rotateDirection = Vector3.right;
    }

    public override void OnPush()
    {
        
    }

    private void Update()
    {
        transform.Rotate(rotateDirection * rotateSpeed * Time.deltaTime, Space.Self);

        Growing();
    }

    private void Growing()
    {

    }

    public void Throw()
    {
        // 플레이어가 바라보고 있는 방향으로 던져짐
        Debug.Log("Throw this");
        transform.parent = null;
    }
}
