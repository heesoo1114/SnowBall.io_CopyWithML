using UnityEngine;

public class AgentContoller : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Collider _collider;
    private Animator _animator;

    #region MovementProperty

    [Header("Movement")]
    [SerializeField] private float threshold;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    private Vector3 previousVelocity;

    private Vector3 moveVelocity;
    private bool isMoveInputIn => (moveVelocity.sqrMagnitude >= threshold);

    #endregion

    private Transform snowBallHolderTransform;
    private SnowBall mySnowBall;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _animator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();

        snowBallHolderTransform = transform.Find("SnowBallHolder");
    }

    private void FixedUpdate()
    {
        if (isMoveInputIn)
        {
            Move();
            Rotate();
        }

        MoveCheck();
    }

    #region Movement

    public void SetVelocity(Vector3 velocity)
    {
        // ���̽�ƽ�� �������� �ʴ´ٸ� �ٷ� ����ϴ�.
        if (velocity == Vector3.zero)
        {
            _rigidbody.velocity = velocity;
        }
        moveVelocity = velocity;
    }

    private void Move()
    {
        moveVelocity = moveVelocity.normalized * moveSpeed;
        _rigidbody.velocity = moveVelocity;
    }

    private void Rotate()
    {
        Quaternion dirQuat = Quaternion.LookRotation(moveVelocity);
        Quaternion moveQuat = Quaternion.Slerp(_rigidbody.rotation, dirQuat, rotateSpeed);
        _rigidbody.MoveRotation(moveQuat);
    }

    private void MoveCheck()
    {
        Vector3 currentVelocity = _rigidbody.velocity;

        // �Է��� ���� �ʰ� �ִ� ���¿��� �Է��� �޴� ���·� ���ϴ��� Ȯ���մϴ�.
        if (previousVelocity.sqrMagnitude < threshold && currentVelocity.sqrMagnitude >= threshold)
        {
            BeginMove();
        }
        // �Է��� ���� �ִ� ���¿��� �Է��� ���� �ʴ� ���·� ���ϴ��� Ȯ���մϴ�.
        else if (previousVelocity.sqrMagnitude >= threshold && currentVelocity.sqrMagnitude < threshold)
        {
            EndMove();
        }

        previousVelocity = _rigidbody.velocity;
    }

    #endregion

    public void BeginMove()
    {
        _animator.SetBool("isMove", true);

        // �����̸� �����մϴ�.
        CreateSnowBall();
    }

    public void EndMove()
    {
        _animator.SetBool("isMove", false);

        // ������ �ִ� �����̸� �߻��մϴ�.
        ThrowSnowBall();
    }

    #region SnowBall

    private void CreateSnowBall()
    {
        // �����̸� �����մϴ�.
        mySnowBall = PoolManager.Instance.Pop("SnowBall") as SnowBall;
        mySnowBall.transform.parent = snowBallHolderTransform;
        mySnowBall.transform.localPosition = Vector3.zero;
    }

    private void ThrowSnowBall()
    {
        // ������ �ִ� �����̸� �߻��մϴ�.
        if (mySnowBall != null)
        {
            mySnowBall.Throw();
        }
    }

    #endregion

}
