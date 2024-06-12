using System.Collections;
using UnityEngine;

public class SnowArea : MonoBehaviour
{
    private readonly Vector3 minusScaleSize = new Vector3(0.00025f, 0, 0.00025f);

    [SerializeField] private float limitScaleSize;
    private Vector3 initScaleSize;

    [SerializeField] private GameObject agentObject;
    private Transform agentParentTransform;

    private void Awake()
    {
        agentParentTransform = transform.parent.Find("Agents");
    }

    private void Start()
    {
        initScaleSize = transform.localScale;

        // InitArea();
        // StartGroundSmaller();
    }

    public void InitArea()
    {
        transform.localScale = initScaleSize;
        // Instantiate(agentObject, agentParentTransform);
    }

    public void StartGroundSmaller()
    {
        StartCoroutine(GroundSmallerLoop());
    }

    private IEnumerator GroundSmallerLoop()
    {
        while (true)
        {
            transform.localScale -= minusScaleSize;
            
            if (transform.localScale.x <= limitScaleSize)
            {
                yield break;
            }

            yield return null;
        }
    }
}
