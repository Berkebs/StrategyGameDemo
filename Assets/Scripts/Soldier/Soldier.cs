using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Soldier : MonoBehaviour,IAttackableObject
{
    private float MovementSpeed;
    private float NextAttackTime;
    private float AttackRate;
    private int MaxHealth;
    private int CurrentHealth;
    private IAttackableObject TargetAttackableObject;

    private int currentPathIndex;
    private List<Vector3> pathVectorList;

    public SoldierSO soldierSO;
    public ObjectTypes ObjectType;
    public Transform HealthBarSprite;

    private void Update()
    {
        HandleMovement();
        if (TargetAttackableObject != null)
        {
            Collider2D[] FindEnemies = Physics2D.OverlapCircleAll(transform.position, 5f);

            foreach (Collider2D FindEnemy in FindEnemies)
            {

                if (FindEnemy.TryGetComponent(out IAttackableObject Enemy))
                {
                    if (Enemy == TargetAttackableObject && Time.time >= NextAttackTime)
                    {
                        Attack();
                        if (Enemy==null)
                        {
                            TargetAttackableObject = null;
                        }
                        else
                        {
                            NextAttackTime = Time.time + AttackRate;

                        }
                    }
                }
            }
           
        }
    }
    private void HandleMovement()
    {
        if (pathVectorList != null)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 1f)
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                transform.position = transform.position + moveDir * MovementSpeed * Time.deltaTime;
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count)
                {
                    StopMoving();
                    
                }
            }
        }
    }
    private void OnEnable()
    {
        Debug.Log("Enable");
        MovementSpeed = soldierSO.MovementSpeed;
        AttackRate = soldierSO.SoldierAttackRate;
        MaxHealth = CurrentHealth = soldierSO.SoldierHP;
        NextAttackTime = 0;
        SetBar((float)CurrentHealth / MaxHealth);
    }

    private void DestroySelf() 
    {
        this.transform.position = new Vector3(-2000,0);
        this.gameObject.SetActive(false);
    }

    private void StopMoving()
    {
        pathVectorList = null;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;

        pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
        TargetAttackableObject = null;

    }
    public void SetTarget( IAttackableObject AttackObject)
    {
        SetTargetPosition(AttackObject.GetPosition());
        TargetAttackableObject = AttackObject;
    }

    void Attack() 
    {
        if (TargetAttackableObject.GetCurrentHP() > 0)
        {
            if (TargetAttackableObject.TakeDamage(soldierSO.SoldierDamage))
            {
                TargetAttackableObject = null;
            }
        }
    }
    public void SetBar(float SetValue)
    {
        HealthBarSprite.localScale = new Vector3(SetValue, HealthBarSprite.localScale.y, HealthBarSprite.localScale.z);
    }

    public bool TakeDamage(int Damage)
    {
        CurrentHealth -= Damage;
        SetBar((float)CurrentHealth / MaxHealth);
        if (CurrentHealth <= 0)
        {
            DestroySelf();
            return true;
        }
        return false;
    }

    public int GetCurrentHP()
    {
        return CurrentHealth;
    }


}
