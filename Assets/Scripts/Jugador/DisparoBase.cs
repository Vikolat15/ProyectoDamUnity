using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaBase : MonoBehaviour
{
    public float velocidadBala;
    private Rigidbody2D Rigidbody2D;
    private Vector2 Direccion;
    private Animator Animator;
    private Collider2D Collider;

    private int dano;

    void Start()
    {
        ConsultarBala(0,0);
        Rigidbody2D = GetComponent<Rigidbody2D>();   
        Animator = GetComponent<Animator>();
        Collider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
            Rigidbody2D.velocity = Direccion * velocidadBala;
        
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
        if (collision.gameObject.TryGetComponent<PatoMovimiento>(out PatoMovimiento enemyComponent1))
        {
            enemyComponent1.recibirDano(dano);
        }
        if (collision.gameObject.TryGetComponent<setaMovimiento>(out setaMovimiento enemyComponent2))
        {
            enemyComponent2.recibirDano(dano);
        }
        if (collision.gameObject.TryGetComponent<GallinaMovimiento>(out GallinaMovimiento enemyComponent3))
        {
            enemyComponent3.recibirDano(dano);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PatoMovimiento>(out PatoMovimiento enemyComponent4))
        {
            enemyComponent4.recibirDano(dano);
        }
        if (collision.gameObject.TryGetComponent<setaMovimiento>(out setaMovimiento enemyComponent5))
        {
            enemyComponent5.recibirDano(dano);
        }
        if (collision.gameObject.TryGetComponent<GallinaMovimiento>(out GallinaMovimiento enemyComponent6))
        {
            enemyComponent6.recibirDano(dano);
        }
    }

    public void ConsultarBala(int id, int idNivel)
    {
        DatabaseManager script = DatabaseManager.Instance;
        if (script == null)
        {
            GameObject obj = GameObject.FindWithTag("Admin");
            if (obj != null) obj.TryGetComponent(out script);
        }

        if (script != null)
        {
            dano = script.GetDanoBala(id, idNivel);
            if (dano <= 0) dano = 50;
        }
        else
        {
            Debug.LogWarning("BalaBase: Admin no encontrado. Usando daño por defecto: 50.");
            dano = 50;
        }
    }
}