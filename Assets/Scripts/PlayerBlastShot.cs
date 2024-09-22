using UnityEngine;

public class PlayerBlastShot : MonoBehaviour
{
    [SerializeField] private GameObject blastShotBullet;

	void Start()
	{
		if (GetComponentInParent<Player>())
		{
			GetComponentInParent<Player>().Bullet = blastShotBullet;	
		}
	}
}
