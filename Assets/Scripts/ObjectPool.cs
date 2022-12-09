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
}
