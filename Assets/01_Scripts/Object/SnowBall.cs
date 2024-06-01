using UnityEngine;

public class SnowBall : PoolableMono
{
    private Rigidbody _rigidbody;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    private Vector3 initPosition;
    private Vector3 worldRightDirection = Vector3.right;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        initPosition = transform.position;
    }

    public override void OnPop()
    {
        _rigidbody.isKinematic = true;
        transform.position = initPosition;
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
        transform.localScale += Vector3.one * 0.0005f;
        transform.parent.transform.localPosition += new Vector3(0, 0.0001f, 0.0001f);
    }

    public void Throw(Vector3 dir)
    {
        // �÷��̾ �ٶ󺸰� �ִ� �������� ������
        transform.parent = null;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(dir * moveSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out IImpactable impactObject))
        {
            Vector3 reverseImpactDir = collision.contacts[0].normal * -1;
            impactObject.OnImpact(reverseImpactDir, transform.localScale.x);

            PoolManager.Instance.Push(this);
        }
    }
}
