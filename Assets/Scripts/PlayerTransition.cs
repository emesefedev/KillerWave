using System.Collections;
using UnityEngine;

public class PlayerTransition : MonoBehaviour
{
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
   
}
