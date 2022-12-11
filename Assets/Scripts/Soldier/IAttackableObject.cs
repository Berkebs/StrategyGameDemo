using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackableObject 
{
    Vector3 GetPosition();
    bool TakeDamage(int Damage);
    int GetCurrentHP();
}
