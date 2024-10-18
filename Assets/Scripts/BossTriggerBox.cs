using UnityEngine;
using UnityEngine.Playables;

public class BossTriggerBox : MonoBehaviour 
{
	private GameObject timeLine;
	
    private void Start()
	{
		timeLine = GameObject.Find("Timeline");
	}

	private void OnTriggerEnter(Collider other)
	{
        if (timeLine != null)
        {
            PlayableDirector playableDirector = timeLine.GetComponent<PlayableDirector>();
            playableDirector.Play();
        }
	}
}
