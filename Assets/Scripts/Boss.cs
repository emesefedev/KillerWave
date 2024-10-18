using UnityEngine;

public class Boss : MonoBehaviour 
{
	private void OnTriggerEnter(Collider other)
    { 
        if (other.gameObject.CompareTag("Player"))
        {
			Die(other.gameObject);
        }
    }

	public void Die(GameObject other)
    {
        Destroy(other);
    }
}
