using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class GameManager : NetworkBehaviour
{
    public Dictionary<int,GameObject> Objetos;
    public static GameManager instance;
    public int playerCount;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else if (instance != this)
        {
            Destroy(gameObject); 
        }
        Objetos = new Dictionary<int, GameObject>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FillWithObjects(int idTryEntry, GameObject objectsTryEntry)
    {
        if (!Objetos.ContainsKey(idTryEntry))
        {
            Objetos.Add(idTryEntry, objectsTryEntry);
            Debug.Log("Se agrego" + idTryEntry + "   " + objectsTryEntry);
        }
            
    }
}
