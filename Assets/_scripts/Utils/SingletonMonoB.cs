using System;
using UnityEngine;

public class SingletonMonoB<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T I
    {
        get
        {
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (!instance)
        {
            if (typeof(T) != GetType())
            {
                Destroy(this);
                throw new Exception("Singleton instance type mismatch!");
            }
            instance = this as T;
        }
        else
        {
            Destroy(this);
        }
    }

}
