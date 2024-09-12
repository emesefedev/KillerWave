using UnityEngine;

[CreateAssetMenu(fileName = "New Actor Model", menuName = "Actor Model")]
public class SOActorModel : ScriptableObject
{
    public enum AttackType {
        Wave,
        Player,
        Flee,
        Bullet
    }

    public string actorName;
    public AttackType attackType;
    [TextArea] public string description;
    public int health;
    public int speed;
    public int hitPower;
    public int score;
    public GameObject actor;
    public GameObject actorBullets;
}
