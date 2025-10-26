using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonChoose : MonoBehaviour
{
    public static SingletonChoose Instance { get; private set; }
    public int selectedMode;
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
