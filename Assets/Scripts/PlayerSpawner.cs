using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] SOActorModel actorModel;
    GameObject playerShip;
    private bool upgradedShip = false;

    private const string PLAYER_NAME = "Player";
    private Vector3 initialPosition = Vector3.zero;
    private Quaternion initialRotation = Quaternion.Euler(0, 180, 0);
    private Vector3 playerScale = new Vector3(60, 60, 60);
    private Vector3 thrusterScale = new Vector3(25, 25, 25);

    private void Start()
    {
        CreatePlayer();
        GetComponentInChildren<Player>().enabled = true;
        GameManager.Instance.CameraSetup();
    }

    private void CreatePlayer()
    {
        // TODO: Definitivamente, mejorar esta función, porque vaya desastre más desastroso
        if (GameObject.Find("Upgraded Ship")) // TODO: Cambiar esto para que no dependa del nombre
        {
            upgradedShip = true;
        }

        if (!upgradedShip || GameManager.Instance.Died)
        {
            GameManager.Instance.Died = false;

            playerShip = Instantiate(actorModel.actor);
            playerShip.transform.position = transform.position;
            playerShip.transform.rotation = Quaternion.Euler(180, 270, 0);
            playerShip.GetComponent<Player>().ActorStats(actorModel);
        }
        else
        {
            playerShip = GameObject.Find("Upgraded Ship"); // TODO: Cambiar esto, repetición de código
        }   

        playerShip.transform.position = initialPosition;
        playerShip.transform.rotation = initialRotation;
        playerShip.transform.localScale = playerScale;

        playerShip.name = PLAYER_NAME;
        
        playerShip.GetComponentInChildren<ParticleSystem>().transform.localScale = thrusterScale;
        playerShip.transform.SetParent(transform);
        GameManager.Instance.CameraSetup();
    }
}
