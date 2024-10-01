using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static int playerScore;
    public static int PlayerScore {get {return playerScore;}}

    public void SetScore(int pointsToAdd)
    {
        playerScore += pointsToAdd;
        UpdateScoreUI(playerScore);
    }

    public void ResetScore()
    {
        playerScore = 0;
        UpdateScoreUI(playerScore);
    }

    public void UpdateScoreUI(int score)
    {
        GameObject scoreTextGameObject = GameObject.FindGameObjectWithTag("ScoreUI");

        if (scoreTextGameObject != null)
        {
            TextMeshProUGUI scoreText = scoreTextGameObject.GetComponent<TextMeshProUGUI>();
            scoreText.text = score.ToString();
        }
    }
}
