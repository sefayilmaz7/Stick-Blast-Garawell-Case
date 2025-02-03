using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// To access the heir by a static field "Instance".
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    [SerializeField] private bool dontDestroyOnLoad;

    public static T Instance
    {
        get { return master; }
    }

    public static T master { get; private set; }

    void Awake()
    {
        if (master == null)
        {
            master = this as T;
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            AwakeSingleton();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void AwakeSingleton() { }
}