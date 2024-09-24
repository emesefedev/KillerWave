using UnityEngine;
using UnityEngine.SceneManagement;

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

    private float gameTimer = 0;
    private float[] endLevelTimer = {30, 30, 45};
    private Scenes currentScene;
    private bool gameEnding = false;

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

    public void ResetCurrentScene() 
    {
        gameTimer = 0;
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
        SceneManager.LoadScene((int)currentScene + 1);
    }

    private void GameTimer()
    {
        switch (currentScene)
        {
            case Scenes.Level1 | Scenes.Level2 | Scenes.Level3 :
                if (gameTimer < endLevelTimer[(int)currentScene - 3])
                {
                    // Level has not been completed
                    gameTimer += Time.deltaTime;
                }
                else
                {
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

                break;
        }
    }
}
