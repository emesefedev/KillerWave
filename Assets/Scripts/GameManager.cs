using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Light directionalLight;
    private Vector3 initialCameraPosition = new Vector3(0, 0, -300);

    private Vector3 directionalLightRotation = new Vector3(50, -30, 0);
    private Color directionalLightColor = new Color(0.596f, 0.8f, 1, 1);

    private void Awake()
    {
        CheckGameManagerIsInScene();
    }

    private void Start()
    {
        CameraSetup();
        LightSetup();
    }

    private void CheckGameManagerIsInScene()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else 
        {
            Destroy(this);
        }
    }


    private void CameraSetup() 
    {
        mainCamera.transform.position = initialCameraPosition;
        mainCamera.transform.rotation = Quaternion.identity;

        mainCamera.clearFlags = CameraClearFlags.SolidColor;
        mainCamera.backgroundColor = Color.black;
    }

    private void LightSetup()
    {
        directionalLight.transform.eulerAngles = directionalLightRotation;
        directionalLight.color = directionalLightColor;
    }
}
