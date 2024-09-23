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
    }

    private void CreatePlayer()
    {
        // TODO: Definitivamente, mejorar esta función, porque vaya desastre más desastroso
        upgradedShip = CheckUpgradedPlayerShip();
        
        if (!upgradedShip || GameManager.Instance.Died)
        {
            GameManager.Instance.Died = false;

            playerShip = Instantiate(actorModel.actor);
            playerShip.GetComponent<Player>().ActorStats(actorModel);
        }  

        playerShip.transform.position = initialPosition;
        playerShip.transform.rotation = initialRotation;
        playerShip.transform.localScale = playerScale;

        playerShip.name = PLAYER_NAME;
        playerShip.GetComponent<PlayerTransition>().enabled = true;
        
        playerShip.GetComponentInChildren<ParticleSystem>().transform.localScale = thrusterScale;
        playerShip.transform.SetParent(transform);

        GetComponentInChildren<Player>().enabled = true;
        GameManager.Instance.CameraSetup();
    }

    private bool CheckUpgradedPlayerShip()
    {
        GameObject upgradedPlayerShip = GameObject.Find("Upgraded Ship"); // TODO: Cambiar esto para que no dependa del nombre
        if (!upgradedPlayerShip)
        {
           return false;
        }

        playerShip = upgradedPlayerShip;
        return true;
    }
}