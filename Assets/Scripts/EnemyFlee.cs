using UnityEngine;
using UnityEngine.AI;

public class EnemyFlee : MonoBehaviour, IActorTemplate {

    [SerializeField] SOActorModel actorModel;
    private int health;
    private int hitPower;
    private int score;

    private Transform player;
    private bool gameStarts = false;

    [SerializeField] private float distanceToRunAway = 200;
    [SerializeField] private NavMeshAgent agent;

    private void Start()
    {
        ActorStats(actorModel);
        Invoke("DelayedStart", 0.5f);
    }

    private void Update()
    {
        if (gameStarts)
        {
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.position);

                if (distance < distanceToRunAway)
                {
                    Vector3 awayFromPlayer = transform.position - player.position;
                    Vector3 newPosition = transform.position + awayFromPlayer;
                    agent.SetDestination(newPosition);
                }

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if the player or their bullet hits you....
        if (other.tag == "Player")
        {
            if (health >= 1)
            {
                health -= other.GetComponent<IActorTemplate>().SendDamage();    
            }
            if (health <= 0)
            {
                //died by player, apply score to 
                GameManager.Instance.GetComponent<ScoreManager>().SetScore(score);
                Die();
            }
        }
    }

	public void ActorStats(SOActorModel actorModel)
    {
        agent.speed = actorModel.speed;
        health = actorModel.health;
        hitPower = actorModel.hitPower;
        score = actorModel.score;
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    public int SendDamage()
    {
        return hitPower;
    }

	public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void DelayedStart()
    {
        gameStarts = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
