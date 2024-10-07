using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScenesManager : MonoBehaviour
{
    // Scene names must be equal to the values of Scenes enum
    // The values of Scenes enum must be in the same order as the Scenes in Build Settings
    public enum Scenes
    {
        BootUp,
        Title,
        Shop,
        Level1,
        Level2,
        Level3,
        GameOver
    } 

    public enum MusicMode
    {
        FadeDown,
        MusicOn,
        NoSound
    } 

    private Scenes currentScene;
    
    private float gameTimer = 0;
    private float[] endLevelTimer = {30, 30, 45};
    
    private bool gameEnding = false;

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioClip levelMusic;

    private void Start()
    {
        StartCoroutine(MusicVolume(MusicMode.MusicOn));
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        // TODO: Mirar si realmente hace falta en el update o lo puedo programar cada vez que se cambia de escena
        int currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        if ((int)currentScene != currentSceneBuildIndex)
        {
            currentScene = (Scenes)currentSceneBuildIndex;
        }
        GameTimer();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(MusicVolume(MusicMode.MusicOn));
        
        // TODO: Mejorar esto, porque creo que tengo un spaghetti
        GameManager.Instance.SetLivesDisplay(GameManager.playerLives);
        
        ScoreManager scoreManager = GameManager.Instance.GetScoreManager();
        scoreManager.UpdateScoreUI(ScoreManager.PlayerScore);
    }

    public void ResetCurrentScene() 
    {
        gameTimer = 0;
        StartCoroutine(MusicVolume(MusicMode.NoSound));
        SceneManager.LoadScene((int)currentScene);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(Scenes.GameOver.ToString());
    }

    public void BeginGame(int gameLevel)
    {
        SceneManager.LoadScene(gameLevel);
    }

    private void NextLevel()
    {
        gameEnding = false;
        gameTimer = 0;
        StartCoroutine(MusicVolume(MusicMode.MusicOn));
        SceneManager.LoadScene((int)currentScene + 1);
    }

    private void GameTimer()
    {
        switch (currentScene)
        {
            case Scenes.Level1:
            case Scenes.Level2:   
            case Scenes.Level3:    
                if (gameTimer < endLevelTimer[(int)currentScene - 3])
                {
                    // Level has not been completed
                    gameTimer += Time.deltaTime;
                }
                else
                {
                    StartCoroutine(MusicVolume(MusicMode.FadeDown));

                    if (!gameEnding)
                    {
                        gameEnding = true;
                        PlayerTransition playerTransition = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTransition>();
                        if (!currentScene.ToString().Equals("Level3"))
                        {
                            playerTransition.LevelEnds = true;
                        }
                        else
                        {
                            playerTransition.GameCompleted = true;
                        }
                        Invoke("NextLevel", 4);
                    }
                }

                if (musicAudioSource.clip == null)
                {
                    Debug.Log("Must add clip");
                    musicAudioSource.clip = levelMusic;
                    musicAudioSource.Play();
                }

                break;
        }
    }

    // TODO: no acabo de entender por qué es una corrutina
    private IEnumerator MusicVolume(MusicMode mode) 
    {
        switch (mode)
        {
            case MusicMode.NoSound:
                musicAudioSource.Stop();
                break;
            
            case MusicMode.FadeDown:
                musicAudioSource.volume -= Time.deltaTime / 3;
                break;

            case MusicMode.MusicOn:
                if (musicAudioSource.clip)
                {
                    musicAudioSource.Play();
                    musicAudioSource.volume = 1;    
                }
                break;

            default:
                musicAudioSource.clip = null;
                break;
        }

        yield return new WaitForSeconds(0.1f);
    }

    private void OnDestroy()
    {
        Debug.Log("Help");
    }
}