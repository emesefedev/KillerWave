using UnityEngine;

public class EnemyWave : MonoBehaviour, IActorTemplate
{
    private int speed;
    private int health;
    private int hitPower;

    private int bulletSpeed;
    
    // Enemy Wave
    [SerializeField] private float verticalSpeed = 2;
    [SerializeField] private float verticalAmplitude = 1;
    private float verticalSine;
    private float time;

    private void Update()
    {
        Attack();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (health >= 1)
            {
                health -= other.GetComponent<IActorTemplate>().SendDamage();
            }
            
            if (health <= 0)
            {
                Die();
            }
            
        }
    }

    private void Attack()
    {
        time += Time.deltaTime;
        verticalSine = Mathf.Sin(time * verticalSpeed) * verticalAmplitude;
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x + speed * Time.deltaTime, pos.y + verticalSine, pos.z); 
    }

    public void ActorStats(SOActorModel actorModel)
    {
        speed = actorModel.speed;
        health = actorModel.health;
        hitPower = actorModel.hitPower;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public int SendDamage()
    {
        return hitPower;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}