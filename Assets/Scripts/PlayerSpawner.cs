using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] SOActorModel actorModel;
    GameObject playerShip;

    private const string PLAYER_NAME = "Player";
    private Vector3 initialPosition = Vector3.zero;
    private Quaternion initialRotation = Quaternion.Euler(0, 180, 0);
    private Vector3 playerScale = new Vector3(60, 60, 60);

    private void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        playerShip = Instantiate(actorModel.actor);
        //playerShip.GetComponent<Player>().ActorStats(actorModel);

        playerShip.name = PLAYER_NAME;
        playerShip.transform.position = initialPosition;
        playerShip.transform.rotation = initialRotation;
        playerShip.transform.localScale = playerScale;
        playerShip.transform.SetParent(transform);

        

    }
}
