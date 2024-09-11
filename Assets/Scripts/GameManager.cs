using UnityEngine;
using UnityEngine.SceneManagement;
using Scenes = ScenesManager.Scenes;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public static Scenes currentScene;

    public static int playerLives = 3;

    [SerializeField] private ScenesManager scenesManager;

    private bool died = false;
    public bool Died {
        get { return died; }
        set { died = value; }
    }

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Light directionalLight;
    private Vector3 initialCameraPosition = new Vector3(0, 0, -300);

    private Vector3 directionalLightRotation = new Vector3(50, -30, 0);
    private Color directionalLightColor = new Color(0.596f, 0.8f, 1, 1);

    private void Awake()
    {
        CheckGameManagerIsInScene();
        currentScene = (Scenes) SceneManager.GetActiveScene().buildIndex;
        CameraAndLightSetup(currentScene);
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

    private void CameraAndLightSetup(Scenes currentScene)
    {
        switch (currentScene)
        {
            case Scenes.TestLevel : 
            case Scenes.Level1 : 
            case Scenes.Level2 : 
            case Scenes.Level3 : 
            {
                CameraSetup();
                LightSetup();
                break;
            }
        }
    }

    public void LoseLife()
    {
        if (playerLives >= 1)
        {
            playerLives--;
            Debug.Log($"Lives left: {playerLives}");
            scenesManager.ResetCurrentScene();
        }
        else
        {
            playerLives = 3;
            scenesManager.GameOver();
        }
    }
}
