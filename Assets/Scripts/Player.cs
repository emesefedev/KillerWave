using UnityEngine;

public class Player : MonoBehaviour, IActorTemplate
{
    private int speed;
    private int health;
    private int hitPower;
    private GameObject actor;
    private GameObject bullet;
    private Vector3 bulletScale = new Vector3(7, 7, 7);
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
        width = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).x - 0.5f);
        height = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).y - 0.5f);

        _Player = GameObject.Find("_Player");
    }

    private void Update()
    {
        Movement();
        Attack();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (health >= 1)
            {
                // TODO: What? no tusta. When shield is created, modify this
                Transform enemy = transform.Find("energy +1(Clone)");
                if (enemy != null)
                {
                    Destroy(enemy.gameObject);
                    health -= other.GetComponent<IActorTemplate>().SendDamage();
                }
                else
                {
                    // TODO: Try to avoid magic number. What does this 1 represent?
                    health -= 1;
                }
            }
            
            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Movement()
    {
        
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput > 0)
        {
            // TODO: Arreglar porque falla horizontalmente. El margen derecho está mal
            if (transform.localPosition.x < width + width / 0.9f)
            {
                transform.localPosition += new Vector3(horizontalInput * speed * Time.deltaTime, 0, 0);
            }
        } 
        else if (horizontalInput < 0)
        {
            if (transform.localPosition.x > width + width / 6f)
            {
                transform.localPosition += new Vector3(horizontalInput * speed * Time.deltaTime, 0, 0);
            }
        }

        if (verticalInput > 0)
        {
            if (transform.localPosition.y < height / 2.5f)
            {
                transform.localPosition += new Vector3(0, verticalInput * speed * Time.deltaTime, 0);
            }
        }
        else if (verticalInput < 0)
        {
            if (transform.localPosition.y > -height / 3f)
            {
                transform.localPosition += new Vector3(0, verticalInput * speed * Time.deltaTime, 0);
            }
        }
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
            bulletInstance.transform.SetParent(_Player.transform);
            bulletInstance.transform.localScale = bulletScale;
        }
    }

    public void ActorStats(SOActorModel actorModel)
    {
        speed = actorModel.speed;
        health = actorModel.health;
        hitPower = actorModel.hitPower;
        bullet = actorModel.actorBullets;
    }

    public int SendDamage()
    {
        return hitPower;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void Die()
    {
        GameManager.Instance.LoseLife();
    }
}
