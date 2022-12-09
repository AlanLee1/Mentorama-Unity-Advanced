using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelEnemy
{
    public GameObject enemyPrefab;
    public int amount;
}

[CreateAssetMenu(fileName = "LevelEnemiesData", menuName = "Mentorama/LevelEnemies", order = 1)]
public class LevelEnemiesData : ScriptableObject
{
    public List<LevelEnemy> levelEnemyList;
}
