using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    Rigidbody _rb;
    Movement _movement;
    //Inputs _inputs;

    public float speed, speedOnCast;

    // Start is called before the first frame update
    protected override void Start()
    {
        //Falta agregar ek camera transform a los dos para hacerlo como el swapm
        _rb = GetComponent<Rigidbody>();
        _movement = new Movement(transform, _rb, speed, speedOnCast);
        // _inputs = new Inputs(_movement, this);

    }

    public void ChangeForm()
    {
         _movement.ChangeForm();
    }
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        //_inputs.Attack = false;
        //UIManager.instance.UpdateBar(UIManager.Bar.PlayerHp, _hp);
    }
    public override void Die()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
