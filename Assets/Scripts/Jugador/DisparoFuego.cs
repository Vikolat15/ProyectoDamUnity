using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaFuego : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidadBala;
    private int dano;

    [Header("Componentes")]
    private Rigidbody2D Rigidbody2D;
    private Vector2 Direccion;
    private Collider2D Collider;
    private SpriteRenderer Sprite;

    void Start()
    {
        ConsultarBala(1, 0);
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();
        Sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (Collider.enabled)
        {
            Rigidbody2D.velocity = Direccion * velocidadBala;
        }
        else
        {
            Rigidbody2D.velocity = Vector2.zero;
        }
    }

    public void SetDireccion(Vector2 direccion)
    {
        Direccion = direccion;
        if (direccion.x < 0) {
            transform.localScale = new Vector3(1, 1, 1);
        } else {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        PatoMovimiento pato = collision.gameObject.GetComponent<PatoMovimiento>();
        setaMovimiento seta = collision.gameObject.GetComponent<setaMovimiento>();
        GallinaMovimiento gallina = collision.gameObject.GetComponent<GallinaMovimiento>();

        if (pato != null || seta != null || gallina != null)
        {
            Collider.enabled = false;
            Sprite.enabled = false;

            if (pato != null) pato.recibirDano(dano);
            if (seta != null) seta.recibirDano(dano);
            if (gallina != null) gallina.recibirDano(dano);

            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(1f); 

                if (pato != null) pato.recibirDano(5);
                if (seta != null) seta.recibirDano(5);
                if (gallina != null) gallina.recibirDano(5);
                
                Debug.Log("Quemado: " + (i + 1));
            }
            Destroy(gameObject);
        }
            Destroy(gameObject);

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
            if (dano <= 0) dano = 40;
        }
        else
        {
            Debug.LogWarning("BalaFuego: Admin no encontrado. Usando daño por defecto: 40.");
            dano = 40;
        }
    }
}