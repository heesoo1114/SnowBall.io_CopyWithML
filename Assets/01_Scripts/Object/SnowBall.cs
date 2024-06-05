using UnityEngine;

public class SnowBall : PoolableMono
{
    private Rigidbody _rigidbody;

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
        _rigidbody.isKinematic = true;
        gameObject.layer = initLayer;
        transform.localPosition = initPosition;
        transform.localScale = initScale;
        transform.localRotation = initRotation;
    }

    public override void OnPush()
    {
        
    }

    private void Update()
    {
        transform.Rotate(worldRightDirection * rotateSpeed * Time.deltaTime, Space.Self);

        if (transform.parent != null)
        {
            Growing();
        }
    }

    private void Growing()
    {
        transform.localScale += Vector3.one * 0.0014f;
        transform.parent.transform.localPosition += new Vector3(0, 0.00043f, 0.0006f);
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

            PoolManager.Instance.Push(this);
        }
    }
}
