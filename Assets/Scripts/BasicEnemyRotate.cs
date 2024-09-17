using UnityEngine;

public class BasicEnemyRotate : MonoBehaviour
{
    [SerializeField] float speed = 0;

    private void Update()
    {
        transform.Rotate(Vector3.left * speed * Time.deltaTime);    
    }
}
