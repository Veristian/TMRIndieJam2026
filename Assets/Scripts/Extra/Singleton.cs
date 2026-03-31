using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>();
                if (instance == null)
                {
                    Debug.Log($"No instances of {typeof(T)} found.");
                    return null;
                }
                return instance;
            }
            else
            {
                return instance;
            }

        }
    }

    protected virtual void Awake()
    {
        instance = this as T;
    }
}
