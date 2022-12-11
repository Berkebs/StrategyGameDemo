using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Soldier")]
public class SoldierSO : ScriptableObject
{
    public int MovementSpeed;
    public int SoldierDamage;
    public int SoldierHP;
    public float SoldierAttackRate;
}
