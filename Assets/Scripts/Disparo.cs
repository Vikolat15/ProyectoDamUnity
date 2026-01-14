using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public float velocidadBala;
    private Rigidbody2D Rigidbody2D;
    private Vector2 Direccion;
    private Animator Animator;
    private Collider2D Collider;

    //private bool haImpactado = false;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();   
        Animator = GetComponent<Animator>();
        Collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       // if (!haImpactado){
            Rigidbody2D.velocity = Direccion * velocidadBala;
        //} else {
        //    Destroy(gameObject);
        //}
        
    }

    public void SetDireccion(Vector2 direccion){
        Direccion = direccion;
        
        if (direccion.x < 0){
            transform.localScale = new Vector3(1, 1, 1);
        } else {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        //haImpactado = true;
    }
}

// El codigo que esta comentado puede que sea util al programar 
// el impacto con enemigos
