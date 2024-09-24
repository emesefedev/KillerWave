using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    private float timer = 0;
    private float timeToLoad = 3;

    [SerializeField] private ScenesManager.Scenes sceneToLoad;

    private void Start()
    {
        GameManager.Instance.GetScoreManager().ResetScore();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeToLoad)
        {
            SceneManager.LoadScene((int)sceneToLoad);
        }
    }
}
