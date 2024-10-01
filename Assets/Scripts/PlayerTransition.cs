using System.Collections;
using UnityEngine;

public class PlayerTransition : MonoBehaviour
{
    [SerializeField] private Player playerComponent;
    [SerializeField] private SphereCollider sphereCollider;

    private Vector3 startPosition = Vector3.zero;
    private Vector3 transitionToEnd = new Vector3(-100,0,0); 
    private Vector3 transitionToCompleteGame = new Vector3(7000,0,0); 
    private Vector3 readyPosition = new Vector3(900,0,0); 

    private float distanceCovered;
    private float journeyLength;
    
    private bool levelStarted = true; 
    private bool speedOff = false; 
    private bool levelEnds = false; 
    private bool gameCompleted = false; 
    
    public bool LevelEnds 
    { 
        get { return levelEnds; } 
        set {levelEnds = value; } 
    } 
    public bool GameCompleted 
    { 
        get { return gameCompleted; } 
        set { gameCompleted = value; }
    }
   

    private void Start()
    {
        transform.localPosition = Vector3.zero;
        startPosition = transform.localPosition;
        Distance();
    }

    private void Update()
    {
        if (levelStarted)
        {
            PlayerMovement(transitionToEnd, 10);
        }

        if (levelEnds)
        {
            playerComponent.enabled = false;
            sphereCollider.enabled = false;
            Distance();
            PlayerMovement(transitionToEnd, 200);
        }

        if (gameCompleted)
        {
            playerComponent.enabled = false;
            sphereCollider.enabled = false;
            PlayerMovement(transitionToCompleteGame, 200);
        }

        if (speedOff)
        {
            Invoke("SpeedOff", 1f);
        }
    }

    private void PlayerMovement(Vector3 point, float transitionSpeed)
    {
        Vector3 localPosition = transform.localPosition;

        if (Mathf.Round(localPosition.x) >= point.x - 5 && 
            Mathf.Round(localPosition.x) <= point.x + 5 &&
            Mathf.Round(localPosition.y) >= -5f && 
            Mathf.Round(localPosition.y) <= 5f)
        {
            if (levelEnds)
            {
                levelEnds = false;
                speedOff = true;
            }

            if (levelStarted)
            {
                levelStarted = false;
                distanceCovered = 0;
                playerComponent.enabled = true;
            }
        }
        else 
        {
            distanceCovered += Time.deltaTime * transitionSpeed;

            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, point, fractionOfJourney);
        }
    }

    private void Distance()
    {
        journeyLength = Vector3.Distance(startPosition, readyPosition);
    }

    private void SpeedOff()
    {
        transform.Translate(Vector3.left * Time.deltaTime * 800);
    }
}
