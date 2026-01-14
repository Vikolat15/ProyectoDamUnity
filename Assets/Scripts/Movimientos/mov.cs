using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimientojugador : MonoBehaviour
{
    public float Velocidad;
    public float Salto;
    private Rigidbody2D Rigidbody2D;
    private float Horizontal;
    private Animator Animator;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");

        Animator.SetBool("running", Horizontal != 0.0f);
    
        if (Input.GetKeyDown(KeyCode.Space)){
            Jump();
        }

        if (Horizontal > 0.0f){
            transform.localScale = new Vector3(-1.0f, 1.0f , 1.0f);
        } else if (Horizontal < 0.0f){
            transform.localScale = new Vector3(1.0f, 1.0f , 1.0f);
        }
    }

    void FixedUpdate(){
        Rigidbody2D.velocity = new Vector2((Velocidad * Horizontal), Rigidbody2D.velocity.y);
    }

    void Jump(){
        Rigidbody2D.AddForce(Vector2.up * Salto, ForceMode2D.Impulse);
    }

}
