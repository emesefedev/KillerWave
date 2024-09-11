using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    // Scene names must be equal to the values of Scenes enum
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
        SceneManager.LoadScene("TestLevel");
    }
}
