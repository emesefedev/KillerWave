using UnityEngine;

public class Boss : MonoBehaviour 
{
    [SerializeField] private GameObject explosion;

	private void OnTriggerEnter(Collider other)
    { 
        if (other.gameObject.CompareTag("Player"))
        {
			Die(other.gameObject);
        }
    }

	public void Die(GameObject other)
    {
        GameObject explosionInstance = Instantiate(explosion);
        explosionInstance.transform.position = transform.position; 

        Destroy(other);
    }
}
