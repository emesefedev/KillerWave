using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SOActorModel actorModel;
    [SerializeField] private float spawnRate;
    [SerializeField][Range(0,10)] private int quantity;

    [SerializeField] private GameObject enemies;

    private void Awake()
    {
        StartCoroutine(SpawnEnemy(quantity, spawnRate));
    }

    private IEnumerator SpawnEnemy(int quantity, float spawnRate)
    {
        for (int i = 0; i < quantity; i++)
        {
            GameObject enemyUnit = CreateEnemy();
            enemyUnit.transform.SetParent(transform);
            enemyUnit.transform.position = transform.position;
            yield return new WaitForSeconds(spawnRate);
        }

        yield return null;
    }

    private GameObject CreateEnemy()
    {
        GameObject enemy = Instantiate(actorModel.actor);
        enemy.GetComponent<IActorTemplate>().ActorStats(actorModel);
        enemy.name = actorModel.actorName;
        return enemy;
    }
}
