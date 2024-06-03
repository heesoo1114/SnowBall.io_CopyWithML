using UnityEngine;

public class AgentContoller : MonoBehaviour, IImpactable
{
    private Rigidbody _rigidbody;
    private Collider _collider;
    private Animator _animator;

    private readonly string snowBallPoolId = "SnowBall";
    private readonly int isMoveHash = Animator.StringToHash("isMove");
    private readonly int isFlyingHash = Animator.StringToHash("isFlying");

    #region MovementProperty

    [Header("Movement")]
    [SerializeField] private float threshold;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    private Vector3 previousVelocity;

    private Vector3 moveVelocity;
    private bool isMoveInputIn => (moveVelocity.sqrMagnitude >= threshold);
    private bool isGrounded;

    #endregion

    private Transform snowBallHolderTransform;
    private Vector3 initHolderPosition;

    private SnowBall mySnowBall;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _animator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();

        snowBallHolderTransform = transform.Find("SnowBallHolder");
        initHolderPosition = snowBallHolderTransform.localPosition;
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
        // 조이스틱을 움직이지 않는다면 바로 멈춥니다.
        if (velocity == Vector3.zero)
        {
            _rigidbody.velocity = Vector3.zero;
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

    // 땅에서 움직임을 시작했는지 멈췄는지 검사하고 그에 맞는 행동을 취합니다.
    private void MoveCheck()
    {
        if (false == isGrounded) return;

        Vector3 currentVelocity = _rigidbody.velocity;

        // 입력을 받지 않고 있던 상태에서 입력을 받는 상태로 변하는지 확인합니다.
        if (previousVelocity.sqrMagnitude < threshold && currentVelocity.sqrMagnitude >= threshold)
        {
            BeginMove();
        }
        // 입력을 받지 있던 상태에서 입력을 받지 않는 상태로 변하는지 확인합니다.
        else if (previousVelocity.sqrMagnitude >= threshold && currentVelocity.sqrMagnitude < threshold)
        {
            EndMove();
        }

        previousVelocity = _rigidbody.velocity;
    }

    #endregion

    public void BeginMove()
    {
        _animator.SetBool(isMoveHash, true);

        // 눈덩이를 생성합니다.
        CreateSnowBall();
    }

    public void EndMove()
    {
        _animator.SetBool(isMoveHash, false);

        // 굴리고 있는 눈덩이를 발사합니다.
        ThrowSnowBall();
        
    }

    #region SnowBall

    private void CreateSnowBall()
    {
        // 눈덩이를 생성합니다.
        mySnowBall = PoolManager.Instance.Pop(snowBallPoolId) as SnowBall;
        mySnowBall.transform.parent = snowBallHolderTransform;
        mySnowBall.transform.localPosition = Vector3.zero;
    }

    private void ThrowSnowBall()
    {
        // 굴리고 있는 눈덩이를 발사합니다.
        if (mySnowBall != null)
        {
            mySnowBall.Throw(transform.forward);
        }
        snowBallHolderTransform.localPosition = initHolderPosition;
    }

    #endregion

    public void OnImpact(Vector3 dir, float forceValue)
    {
        _rigidbody.AddForce(dir * forceValue, ForceMode.Impulse);
        isGrounded = false;
        _animator.SetBool(isFlyingHash, true);

        // Effects
    }

    private void Land()
    {
        isGrounded = true;
        _rigidbody.velocity = Vector3.zero;
        _animator.SetBool(isFlyingHash, false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isGrounded) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            Land();
        }
    }

}
