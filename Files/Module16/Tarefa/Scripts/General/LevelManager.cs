using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Transform playerStartPosition;

    [Header("Enemies")]
    [SerializeField] bool isBossLevel = false;
    [SerializeField] LevelEnemiesData levelEnemies;
    [SerializeField] List<Transform> levelWaypoints;
    List<EnemyController> currentEnemies = new List<EnemyController>();
    int enemiesDead = 0;

    [Header("Portals")]
    [SerializeField] List<LevelPortal> levelPortals;
    [SerializeField] Animator animatorStatue;
    readonly int anim_open = Animator.StringToHash("Open");
    [SerializeField] GameObject statue;
    [SerializeField] CinemachineVirtualCamera camStatue;

    ////TAREFA: Pode substituir o gateObject por um script para controlar o gate, GateController por exemplo.
    //[Header("Gate")]
    //[SerializeField] GameObject gateObject;

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
        gm.currentLevelManager = this;

        gm.player.transform.position = playerStartPosition.position;
        gm.player.gameObject.SetActive(true);

        if (isBossLevel)
        {
            SpawnBoss();
        } else
        {
            SpawnEnemies();
        }
    }

    private void SpawnBoss()
    {
        EnemyController newEnemy = Instantiate(levelEnemies.levelEnemyList[0].enemyPrefab, levelWaypoints[0].position, Quaternion.identity, transform).GetComponent<EnemyController>();

        newEnemy.EnemySetup(levelWaypoints);

        currentEnemies.Add(newEnemy);
    }

    private void SpawnEnemies()
    {
        int enemiesLength = levelEnemies.levelEnemyList.Count;
        int waypointsLength = levelWaypoints.Count;
        int amount;
        EnemyController newEnemy = null;

        if (waypointsLength > 0)
        {
            for (int i = 0; i < enemiesLength; i++)
            {
                amount = levelEnemies.levelEnemyList[i].amount;

                if (amount > 1)
                {
                    for (int j = 0; j < amount; j++)
                    {
                        InstantiateEnemy(newEnemy, levelEnemies.levelEnemyList[i].enemyPrefab, waypointsLength);
                    }
                } else
                {
                    InstantiateEnemy(newEnemy, levelEnemies.levelEnemyList[i].enemyPrefab, waypointsLength);
                }

            }

        }
    }

    private void InstantiateEnemy(EnemyController newEnemy, GameObject enemy, int randomLength)
    {
        int spawnPoint = Random.Range(0, randomLength);

        newEnemy = Instantiate(enemy, levelWaypoints[spawnPoint].position, Quaternion.identity, transform).GetComponent<EnemyController>();

        newEnemy.EnemySetup(levelWaypoints);

        currentEnemies.Add(newEnemy);
    }

    public void EnemyDead()
    {
        enemiesDead++;

        int enemyCount = currentEnemies.Count;

        if (enemiesDead >= enemyCount)
        {
            Debug.Log("Level Completed!");

            if (isBossLevel)
            {
                StartCoroutine(Open());

            } else
            {
                ActivatePortals();
            }
        }
    }

    public GameObject GetBossGameObject()
    {
        if (isBossLevel)
        {
            if (currentEnemies.Count > 0)
            {
                return currentEnemies[0].gameObject;
            }
        }
        return null;
    }

    public void ActivatePortals()
    {
        int count = levelPortals.Count;

        for (int i = 0; i < count; i++)
        {
            levelPortals[i].ShowPortal();
        }
    }

    IEnumerator Open()
    {
        camStatue.Priority = 11;
        yield return new WaitForSecondsRealtime(2f);
        animatorStatue.SetTrigger(anim_open);
        yield return new WaitForSecondsRealtime(2.5f);
        camStatue.Priority = 0;
        statue.gameObject.SetActive(false);
    }

}
