using UnityEngine;
using UnityEngine.EventSystems;

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
    private GameObject[] screenPoints = new GameObject[2];
    
    private float cameraTravelSpeed;
    private float movingScreen;

    private Vector3 direction;
    [SerializeField] private Rigidbody playerRigidbody;
    
    public static bool mobileMode = false;
    private float autoAttackRepetitionTime = 0.3f;

    [SerializeField] private GameObject explosion;

    public int Health {
        get { return health; }
        set { health = value; }
    }
    public GameObject Bullet {
        get { return bullet; }
        set { bullet = value; }
    }

    public float CameraTravelSpeed {
        get { return cameraTravelSpeed; }
        set { cameraTravelSpeed = value; }
    }
    
    private void Start()
    {
        _Player = GameObject.Find("_Player");

        CalculateBoundaries();

        mobileMode = false;
        #if UNITY_ANDROID && !UNITY_EDITOR
            mobileMode = true;
            InvokeRepeating("Attack", 0, autoAttackRepetitionTime);
        #endif
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            PlayerSpeedWithCamera();
            if (mobileMode)
            {
                MovementMobileControls();
            }
            else
            {
                Movement();
                Attack();
            }
        }
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

    private void CalculateBoundaries()
    {
        screenPoints[0] = new GameObject("p1");
        screenPoints[1] = new GameObject("p2");

        Vector3 v1 = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 300));
        Vector3 v2 = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 300));

        screenPoints[0].transform.position = v1;
        screenPoints[1].transform.position = v2;

        screenPoints[0].transform.SetParent(transform.parent);
        screenPoints[1].transform.SetParent(transform.parent);

        movingScreen = screenPoints[1].transform.position.x;
    }

    private void PlayerSpeedWithCamera()
    {
        if (cameraTravelSpeed > 1)
        {
            movingScreen += cameraTravelSpeed * Time.deltaTime;
            transform.position += Vector3.right * cameraTravelSpeed * Time.deltaTime;
        }
        else
        {
            movingScreen = 0;
        }
    }

    private void MovementMobileControls()
    {
        // TODO: Bloquear movimiento horizontal cuando el modo automático de la cámara está activado
        // TODO: Hacer movimiento por Transform y no por Rigidbody, porque si no tendría que usar el FixedUpdate
        if (Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null)
        {
            Touch touch = Input.GetTouch(0);
            
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(touch.position.x, touch.position.y, 300));
            touchPosition.z = 0;

            direction = (touchPosition - transform.position).normalized;
            playerRigidbody.velocity = new Vector3(direction.x, direction.y, 0) * speed;

            direction.x += movingScreen;

            if (touch.phase == TouchPhase.Ended)
            {
                playerRigidbody.velocity = Vector3.zero;
            }
        }
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        float left = screenPoints[0].transform.localPosition.x;
        float right = screenPoints[1].transform.localPosition.x;
        
        float top = screenPoints[0].transform.localPosition.y;
        float down = screenPoints[1].transform.localPosition.y;

        
        if (horizontalInput > 0)
        {
            if (transform.localPosition.x < (right - right / 5f) + movingScreen)
            {
                transform.localPosition += new Vector3(horizontalInput * speed * Time.deltaTime, 0, 0);
            }
        } 
        else if (horizontalInput < 0)
        {
            if (transform.localPosition.x > (left - left / 5f) + movingScreen)
            {
                transform.localPosition += new Vector3(horizontalInput * speed * Time.deltaTime, 0, 0);
            }
        }


        if (verticalInput > 0)
        {
            if (transform.localPosition.y < (top - top / 3f))
            {
                transform.localPosition += new Vector3(0, verticalInput * speed * Time.deltaTime, 0);
            }
        }
        else if (verticalInput < 0)
        {
            if (transform.localPosition.y > (down - down / 1.5f))
            {
                transform.localPosition += new Vector3(0, verticalInput * speed * Time.deltaTime, 0);
            }
        }
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Fire1") || mobileMode)
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
        GameObject explosionInstance = Instantiate(explosion);
        explosionInstance.transform.position = transform.position; 

        StartCoroutine(GameManager.Instance.DelayedLoseLife());
    }
}
