using UnityEngine;
using System.Collections;

public class EnemigoFinal : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadPersecucion = 3f;
    public float velocidadCarga = 12f;
    public float distanciaCarga = 5f;

    [Header("Tiempos")]
    public float tiempoPreparacion = 0.6f;
    public float tiempoCarga = 1f;
    public float cooldownCarga = 2f;

    private Rigidbody2D rb;
    private Animator anim;
    private GameObject player;

    private int direccion = 1; 
    private bool estaAtacando = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Si está atacando (preparando, cargando o en cooldown), 
        // solo permitimos el giro al final de la corrutina, no aquí.
        if (estaAtacando || player == null) return;

        float distanciaX = player.transform.position.x - transform.position.x;
        
        if (Mathf.Abs(distanciaX) > 0.5f)
        {
            direccion = (distanciaX > 0) ? 1 : -1;
        }
        else
        {
            direccion = 0;
        }

        ActualizarGiroVisual(direccion);

        float distanciaY = Mathf.Abs(player.transform.position.y - transform.position.y);
        if (Mathf.Abs(distanciaX) < distanciaCarga && distanciaY < 1.5f)
        {
            StartCoroutine(RutinaDeCarga());
        }
    }

    void FixedUpdate()
    {
        if (estaAtacando) return;
        rb.velocity = new Vector2(direccion * velocidadPersecucion, rb.velocity.y);
        
        if (anim != null) 
            anim.SetBool("running", direccion != 0);
    }

    // He movido el giro a una función para poder usarlo en varios sitios
    void ActualizarGiroVisual(int dir)
    {
        if (dir > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (dir < 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    IEnumerator RutinaDeCarga()
    {
        estaAtacando = true;
        
        int dirCarga = (player.transform.position.x > transform.position.x) ? 1 : -1;
        ActualizarGiroVisual(dirCarga);

        // --- FASE 1: PREPARACIÓN ---
        rb.velocity = new Vector2(0, rb.velocity.y);
        if (anim != null) anim.SetTrigger("alert");
        yield return new WaitForSeconds(tiempoPreparacion);

        // --- FASE 2: EMBESTIDA ---
        if (anim != null) anim.SetBool("charging", true);
        float t = 0;
        while (t < tiempoCarga)
        {
            rb.velocity = new Vector2(dirCarga * velocidadCarga, rb.velocity.y);
            t += Time.deltaTime;
            yield return null;
        }

        // --- FASE 3: DESCANSO Y GIRO INMEDIATO ---
        if (anim != null) anim.SetBool("charging", false);
        rb.velocity = new Vector2(0, rb.velocity.y);

        // AQUÍ EL CAMBIO: Miramos al jugador justo al frenar
        if (player != null)
        {
            int nuevaDireccion = (player.transform.position.x > transform.position.x) ? 1 : -1;
            ActualizarGiroVisual(nuevaDireccion);
        }

        yield return new WaitForSeconds(cooldownCarga);

        estaAtacando = false;
    }
}