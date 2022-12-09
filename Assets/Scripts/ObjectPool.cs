using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public ObjectPool Instance { get; private set; }
    public ObjectPool() 
    {
        Instance= this;
   
    }

    public void CreateObject(List<Object> objects,int size)
    {
        foreach (Object obj in objects)
        {
            Transform objectParent = new GameObject(obj.Name + "s").transform;

            for (int i = 0; i < size; i++)
            {
                Instantiate(obj.Prefab,new Vector3(-2000,0),Quaternion.identity,objectParent);
            }
        }
    }

    public class Object
    {
        public string Name;
        public GameObject Prefab;
    }
    
}
