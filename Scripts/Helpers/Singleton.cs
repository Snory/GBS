using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: Singleton<T>
{
    private static T _instance;

    public static T Instatance { get { return _instance; } }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = (T) this;

        }
        else
        {
            Debug.LogError($"[Singleton]: Trying to instantiate {this.gameObject.name} again.");
            Destroy(this.gameObject);
        }


    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
