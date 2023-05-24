using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static bool _isQuitting = false;

    public static T Instance
    {
        get
        {
            if (_instance == null && !_isQuitting)
            {
                _instance = (T)FindObjectOfType(typeof(T));

                // If an instance is found, make it persistent
                if (_instance != null)
                {
                    DontDestroyOnLoad(_instance.gameObject);
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            // If another instance exists and it's not this instance, destroy this GameObject
            Destroy(this.gameObject);
        }
        else if (_instance == null)
        {
            // If no instance exists, make this the instance
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Your code here. This function is called whenever a scene is loaded.
    }


    public virtual void OnApplicationQuit()
    {
        _isQuitting = true;
    }
}
