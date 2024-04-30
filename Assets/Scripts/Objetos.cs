using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objetos : MonoBehaviour
{
    public int id;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.FillWithObjects(id, this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
