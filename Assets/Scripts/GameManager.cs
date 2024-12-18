using UnityEngine;
using UnityEngine.SceneManagement;
using Scenes = ScenesManager.Scenes;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public static Scenes currentScene; // TODO: La currentScene la detecta ScenesManager

    public static int playerLives = 3;

    [SerializeField] private ScenesManager scenesManager;
    [SerializeField] private ScoreManager scoreManager;
    
    [SerializeField] private GameObject lifePrefab;

    private bool died = false;
    public bool Died {
        get { return died; }
        set { died = value; }
    }

    private Camera mainCamera; 
    private Light directionalLight; 
    private Vector3 initialCameraPosition = new Vector3(0, 0, -300);

    private Vector3 directionalLightRotation = new Vector3(50, -30, 0);
    private Color directionalLightColor = new Color(0.596f, 0.8f, 1, 1);

    private void Awake()
    {
        #if UNITY_ANDROID
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        #endif

        CheckGameManagerIsInScene();
        currentScene = (Scenes) SceneManager.GetActiveScene().buildIndex;
        CameraAndLightSetup(currentScene);
    }

    private void Start()
    {
        SetLivesDisplay(playerLives);
    }

    public void SetLivesDisplay(int lives)
    {
        GameObject livesGameObject = GameObject.FindGameObjectWithTag("LivesUI");
        if (livesGameObject != null)
        {
            if (livesGameObject.transform.childCount < 1)
            {
                // Al entrar en este if, asumimos que es el principio de un nivel
                for (int i = 0; i < 5; i++)
                {
                    GameObject life = Instantiate(lifePrefab);
                    life.transform.SetParent(livesGameObject.transform);
                }
            }

            int totalChildren = livesGameObject.transform.childCount;
            for (int i = 0; i < totalChildren; i++)
            {
                livesGameObject.transform.GetChild(i).gameObject.SetActive(true);
            }

            for (int i = 0; i < (totalChildren - lives); i++)
            {
                livesGameObject.transform.GetChild(totalChildren - i - 1).gameObject.SetActive(false);
            }
        }
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


    public void CameraSetup(float cameraSpeed) 
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        mainCamera.transform.position = initialCameraPosition;
        mainCamera.transform.rotation = Quaternion.identity;

        mainCamera.clearFlags = CameraClearFlags.SolidColor;
        mainCamera.backgroundColor = Color.black;

        CameraMovement cameraMovement = mainCamera.GetComponent<CameraMovement>();
        if (cameraMovement) cameraMovement.CameraSpeed = cameraSpeed; // Only available in Level 3
    }

    private void LightSetup()
    {
        directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();

        directionalLight.transform.eulerAngles = directionalLightRotation;
        directionalLight.color = directionalLightColor;
    }

    private void CameraAndLightSetup(Scenes currentScene)
    {
        switch (currentScene)
        {
            case Scenes.Level1 : 
            case Scenes.Level2 : 
            {
                CameraSetup(0);
                LightSetup();
                break;
            }
            case Scenes.Level3 : 
                CameraSetup(150); // TODO: Eliminar Magic numbers
                break;
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

    public IEnumerator DelayedLoseLife() 
    {
        yield return new WaitForSeconds(1);
        LoseLife();
    }

    public ScoreManager GetScoreManager()
    {
        return scoreManager;
    }

    public ScenesManager GetScenesManager()
    {
        return scenesManager;
    }
}
