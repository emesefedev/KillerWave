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
        TestLevel,
        Level1,
        Level2,
        Level3,
        GameOver
    }    

    public void ResetCurrentScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(Scenes.GameOver.ToString());
    }

    public void BeginGame()
    {
        SceneManager.LoadScene(Scenes.TestLevel.ToString());
    }
}
