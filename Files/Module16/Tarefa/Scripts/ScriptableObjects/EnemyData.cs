using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Mentorama/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public int health = 50;
    public int attack = 10;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public float speed = 2f;
 
}
