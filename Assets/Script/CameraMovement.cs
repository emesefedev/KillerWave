using UnityEngine;

public class CameraMovement : MonoBehaviour 
{	
    private Player player;
	private bool beginMoving = false;
	private float cameraSpeed;
    
    public float CameraSpeed
    {
        get {return cameraSpeed;}
        set {cameraSpeed = value;}
    }

	private void Start() 
	{
        player = FindObjectOfType<Player>();
        Debug.Log(player);

		Invoke("DelayStart", 4f);
	}

	private void Update()
	{
		if (beginMoving)
		{
			if (transform.position.x < 5350) // TODO: Quitar los magic numbers
			{
				transform.Translate(Vector3.right * Time.deltaTime * cameraSpeed);	
			}
			else
			{
				beginMoving = false;
                player.CameraTravelSpeed = 0;
			}	
		}
	}

    private void DelayStart()
	{
		beginMoving = true;
		if (GameObject.Find("Player"))
		{
		    player.CameraTravelSpeed = cameraSpeed;
		}
	}
}
