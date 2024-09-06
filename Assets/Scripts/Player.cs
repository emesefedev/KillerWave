using UnityEngine;

public class Player : MonoBehaviour, IActorTemplate
{
    private int speed;
    private int health;
    private int hitPower;
    private GameObject actor;
    private GameObject bullet;
    private GameObject _Player;

    // World space measurements
    private float width;
    private float height;

    public int Health {
        get { return health; }
        set { health = value; }
    }
    public GameObject Bullet {
        get { return bullet; }
        set { bullet = value; }
    }
    
    private void Start()
    {
        // Independent of screen resolution
        width = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)). x - 0.5f);
        height = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).y - 0.5f);

        _Player = GameObject.Find("_Player");
    }

    private void Update()
    {
        Movement();
        Attack();
    }

    private void Movement()
    {

    }

    private void Attack()
    {

    }

    public void ActorStats(SOActorModel actorModel)
    {
        health = actorModel.health;
        speed = actorModel.speed;
        hitPower = actorModel.hitPower;
        bullet = actorModel.actorBullets;
    }

    public int SendDamage()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }
}
