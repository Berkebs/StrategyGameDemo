using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : MonoBehaviour,IAttackableObject
{
    public BuildingSO BuildObjectSO;
    private Vector2Int origin;
    private Vector2 InstantiateObjectPos;
    int MaxHealth;
    int CurrentHealth;
    public Transform HealthBarSprite;

    //Create Build Object
    public static BuildObject Create(Vector3 worldPosition, Vector2Int origin, BuildingSO BuildSO)
    {
        GameObject BuildObjectTransform = ObjectPool.Instance.SpawnObject(BuildSO.ObjectType,worldPosition);
        BuildObject buildObject = BuildObjectTransform.GetComponent<BuildObject>();
        float halfCellSize = Pathfinding.Instance.grid.GetCellSize() / 2;
        buildObject.InstantiateObjectPos = new Vector2(worldPosition.x+(halfCellSize*BuildSO.width),worldPosition.y-halfCellSize);
        buildObject.BuildObjectSO = BuildSO;
        buildObject.origin = origin;
        buildObject.MaxHealth= buildObject.CurrentHealth = BuildSO.BuildHP;
        return buildObject;
    }
    public List<Vector2Int> GetGridPositionList()
    {
        return BuildObjectSO.GetGridPositionList(origin);

    }
    public void DestroySelf()
    {
        this.gameObject.transform.position = new Vector2(-2000,0);
        this.gameObject.SetActive(false);
    }

    //Get Information Panel
    public void OnClick()
    {
        UIManager.Instance.SelectBarrack(this);
    }
    public void CreateObject(ObjectTypes ObjectType) 
    {
        ObjectPool.Instance.SpawnObject(ObjectType, InstantiateObjectPos);
    }
    public Vector3 GetPosition()
    {
        return InstantiateObjectPos;
    }


    //Health System
    public void SetBar(float SetValue)
    {
        HealthBarSprite.localScale = new Vector3(SetValue, HealthBarSprite.localScale.y,HealthBarSprite.localScale.z);
    }

    public bool TakeDamage(int Damage) 
    {
        CurrentHealth -= Damage;
        SetBar((float)CurrentHealth/MaxHealth);
        if (CurrentHealth<=0)
        {

            BuildingSystem.Instance.DestroyBuild(origin);
            return true;
        }
        return false;
    }
    public int GetCurrentHP() 
    {
        return CurrentHealth;
    }

}
