using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private Transform playerTransform;
    public Transform PlayerTransform => playerTransform;

    private void Awake()
    {
        SetFrameRate();

        Init();   
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void Init()
    {
        // �Ŵ������� �ʱ�ȭ �մϴ�.
        PoolManager.Instance.Init();
        InputHandler.Instance.Init();
        CameraController.Instance.Init();
    }

    public void FinishGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }

    private void SetFrameRate()
    {
#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif
    }
}
