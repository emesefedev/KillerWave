using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static int playerScore;
    public static int PlayerScore {get {return playerScore;}}

    public void SetScore(int pointsToAdd)
    {
        playerScore += pointsToAdd;
    }

    public void ResetScore()
    {
        playerScore = 0;
    }
}
