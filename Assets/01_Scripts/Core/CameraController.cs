using UnityEngine;

public class CameraController : MonoSingleton<CameraController>
{
    private Camera mainCam;

    private Transform playerTransform;

    [SerializeField] private float offsetY;
    [SerializeField] private float offsetZ;

    public override void Init()
    {
        mainCam = Camera.main;
        playerTransform = GameManager.Instance.PlayerTransform;
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            Vector3 camPos = new Vector3(playerTransform.position.x, offsetY, playerTransform.position.z + offsetZ);
            mainCam.transform.position = camPos;
        }
    }
}
