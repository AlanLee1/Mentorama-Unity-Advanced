using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Mentorama/PlayerData", order = 2)]
public class PlayerData : ScriptableObject
{
    public int health = 100;
    public int rangedCharges = 2;
    public int attack = 10;

}
