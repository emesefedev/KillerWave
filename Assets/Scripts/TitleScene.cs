using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    private void Start()
    {
        GameManager.playerLives = 3;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene((int)ScenesManager.Scenes.Shop);
        }
    }
}
