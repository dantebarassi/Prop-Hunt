using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonChoose : MonoBehaviour
{
    public static SingletonChoose Instance { get; private set; }
    [Networked, OnChangedRender(nameof(OnRoleChanged))]
    public int selectedMode { get; set; }

    public void OnRoleChanged(int index)
    {
        selectedMode = index;
        Debug.Log(selectedMode);
    }
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
