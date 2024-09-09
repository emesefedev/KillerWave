using UnityEngine;

public class PlayerBullet : MonoBehaviour, IActorTemplate
{
    private int speed;
    private int health;
    private int hitPower;
    private GameObject actor;
    [SerializeField] private SOActorModel bulletModel;

    private void Awake()
    {
        ActorStats(bulletModel);
    }

    private void Update()
    {
        transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            IActorTemplate actorTemplate = other.GetComponent<IActorTemplate>();
            if (actorTemplate != null)
            {
                if (health <= 0)
                {
                    Die();
                }
                else if (health >= 1)
                {
                    health -= actorTemplate.SendDamage();
                }
            }
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void ActorStats(SOActorModel actorModel)
    {
        speed = actorModel.speed;
        health = actorModel.health;
        hitPower = actorModel.hitPower;
        actor = actorModel.actor;
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
