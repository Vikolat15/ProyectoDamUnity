using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaHielo : MonoBehaviour
{
    public float velocidadBala;
    private int dano;
    private Rigidbody2D Rigidbody2D;
    private Vector2 Direccion;
    private Collider2D Collider;
    private SpriteRenderer Sprite;

    void Start()
    {
        ConsultarBala(2, 0);
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
        MurcielagoMovimiento murcielago = collision.gameObject.GetComponent<MurcielagoMovimiento>();
        RinoceronteMovimiento rinoceronte = collision.gameObject.GetComponent<RinoceronteMovimiento>();

        if (pato != null || seta != null || gallina != null || murcielago != null || rinoceronte != null)
        {
            Collider.enabled = false;
            Sprite.enabled = false;

            if (pato != null) pato.recibirDano(dano);
            if (seta != null) seta.recibirDano(dano);
            if (gallina != null) gallina.recibirDano(dano);
            if (murcielago != null) murcielago.recibirDano(dano);
            if (rinoceronte != null) rinoceronte.recibirDano(dano);
            if (pato != null) pato.Flash();
            if (seta != null) seta.Flash();
            if (gallina != null) gallina.Flash();
            if (murcielago != null) murcielago.Flash();
            if (rinoceronte != null) rinoceronte.Flash();

            if (pato != null) pato.reducirVelocidad(1.5f);
            if (seta != null) seta.reducirVelocidad(1.5f);
            if (murcielago != null) murcielago.reducirVelocidad(1.5f);
            if (rinoceronte != null) rinoceronte.reducirVelocidad(12f);

            yield return new WaitForSeconds(5f); 

            if (pato != null) pato.reducirVelocidad(2f);
            if (seta != null) seta.reducirVelocidad(4f);
            if (gallina != null) gallina.reducirVelocidad(2f);
            if (murcielago != null) murcielago.reducirVelocidad(2f);
            if (rinoceronte != null) rinoceronte.reducirVelocidad(18f);

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
            if (dano <= 0) dano = 45;
        }
        else
        {
            Debug.LogWarning("BalaHielo: Admin no encontrado. Usando daño por defecto: 45.");
            dano = 45;
        }
    }
}