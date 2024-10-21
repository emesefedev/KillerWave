using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnPhysics : MonoBehaviour 
{
    [SerializeField] private float timeToSwitchPhysics = 38;
	private float timer = 0;
	private bool switchPhysicsOn = false;
	
    private void Update() 
    {
		if (Time.timeScale == 1)
		{
			timer += Time.deltaTime;	
		}

		if (timer > timeToSwitchPhysics && !switchPhysicsOn)
		{
			switchPhysicsOn = true;

			for (int i = 0; i < this.transform.childCount; i++)
			{
				this.transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
			}
		}
	}
}
