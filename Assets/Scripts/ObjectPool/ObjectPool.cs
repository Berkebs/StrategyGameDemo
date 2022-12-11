using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }
    public List<poolObject> PoolObjects;
    public Dictionary<ObjectTypes, Queue<GameObject>> Pools;

    private void Awake()
    {
        Instance= this;
        Pools=new Dictionary<ObjectTypes, Queue<GameObject>>();
        CreateObjects(PoolObjects);
    }

    public void CreateObjects(List<poolObject> objects)
    {
        foreach (poolObject obj in objects)
        {
            Transform objectParent = new GameObject(obj.ObjectType.ToString() + "Parent").transform;
            Queue<GameObject> objectPool=new Queue<GameObject>();
            for (int i = 0; i < obj.PoolSize; i++)
            {
                GameObject instobj=  Instantiate(obj.Prefab,new Vector3(-2000,0),Quaternion.identity,objectParent);
                instobj.SetActive(false);
                objectPool.Enqueue(instobj);
            }
            Pools.Add(obj.ObjectType,objectPool);
        }
    }

    public GameObject SpawnObject(ObjectTypes Type,Vector3 spawnPos) 
    {
          if (!Pools.ContainsKey(Type))
              return null;

          GameObject spawnObject = Pools[Type].Dequeue();
          spawnObject.SetActive(true);
          spawnObject.transform.position = spawnPos;
          spawnObject.transform.rotation = Quaternion.identity;


          Pools[Type].Enqueue(spawnObject);

          return spawnObject;
    }

}

[System.Serializable]
public class poolObject
{
    public ObjectTypes ObjectType;
    public GameObject Prefab;
    public int PoolSize;
}

public enum ObjectTypes
{
    Barrack,
    PowerPlant,
    SoldierA,
    SoldierB,
    SoldierC
}