using Unity.MLAgents;
using UnityEngine;

public class SnowBall : PoolableMono
{
    private Rigidbody _rigidbody;

    private SnowBallAgent owner = null;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    private Vector3 worldRightDirection = Vector3.right;

    private Vector3 initPosition;
    private Vector3 initScale;
    private Quaternion initRotation;

    [Header("ImpactForce")]
    [SerializeField] private float maxImpactForceY;

    private int initLayer;
    private int newLayer;

    private bool isRolling => (transform.parent != null);

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        initLayer = LayerMask.NameToLayer("SnowBall");
        newLayer = LayerMask.NameToLayer("OthersSnowBall");

        initPosition = transform.localPosition;
        initScale = transform.localScale;
        initRotation = transform.localRotation;
    }

    public override void OnPop()
    {
        if (transform.parent.parent.TryGetComponent(out SnowBallAgent agent))
        {
            owner = agent;
        }

        _rigidbody.isKinematic = true;
        gameObject.layer = initLayer;
        transform.localPosition = initPosition;
        transform.localScale = initScale;
        transform.localRotation = initRotation;
    }

    public override void OnPush()
    {
        owner = null;
    }

    private void Update()
    {
        transform.Rotate(worldRightDirection * rotateSpeed * Time.deltaTime, Space.Self);

        if (isRolling)
        {
            Growing();
        }
    }

    private void Growing()
    {
        transform.localScale += Vector3.one * 0.0015f;
        transform.parent.transform.localPosition += new Vector3(0, 0.00043f, 0.00066f);
        owner.GiveReward(0.01f);
    }

    public void Throw(Vector3 dir)
    {
        // 플레이어가 바라보고 있는 방향으로 던져짐
        transform.parent = null;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(dir * moveSpeed);
        gameObject.layer = newLayer;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out IImpactable impactObject))
        {
            Vector3 reverseImpactDir = -collision.contacts[0].normal;
            reverseImpactDir.y = Mathf.Clamp(transform.localScale.y, transform.localScale.y, maxImpactForceY);
            impactObject.OnImpact(reverseImpactDir, transform.localScale.x);

            if (isRolling)
            {
                InputHandler.Instance.InitInput();
            }

            owner.GiveReward(1f);
            PoolManager.Instance.Push(this);
        }
        
        if (collision.transform.CompareTag("Water"))
        {
            owner.GiveReward(-1f);
            PoolManager.Instance.Push(this);
        }
    }
}
