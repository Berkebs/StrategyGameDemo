using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackableObject 
{
   Vector3 GetPosition();
    void TakeDamage(int Damage);
}
