using UnityEngine;
using System.Collections;

public class RinoceronteMovimiento : Entidad
{
    public float velocidadCarga = 8f;
    public float tiempoPreparacion = 2.0f;
    public float duracionCarga = 1.5f;
    
    private Rigidbody2D rb;
    private Animator animator;
    private Transform jugador;
    private int direccion = 1;
    private int vidaDB;
    private int danoDB;

    [SerializeField] private Material flashMaterial;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;

    public override int VidaMaxima
    {
        get { return vidaDB; }
        protected set { base.VidaMaxima = value; }
    }

    public override int Dano
    {
        get { return danoDB; }
        protected set { base.Dano = value; }
    }

    void Start()
    {
        ConsultarEnemigo(4, 0);
        vida = VidaMaxima;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jugador = GameObject.FindWithTag("Player").transform;
        StartCoroutine(CicloCarga());

        spriteRenderer = GetComponent<SpriteRenderer>();
            
        originalMaterial = spriteRenderer.material;
    }

    IEnumerator CicloCarga()
    {
        while (true)
        {
            rb.velocity = Vector2.zero;
            
            if (animator != null) animator.SetBool("running", false);

            if (jugador != null)
            {
                direccion = (jugador.position.x > transform.position.x) ? 1 : -1;
                transform.localScale = new Vector3(direccion > 0 ? -1 : 1, 1, 1);
            }


            yield return new WaitForSeconds(tiempoPreparacion);

            if (animator != null)
            {
                animator.SetBool("running", true);
            }

            float tiempoCargando = 0;
            while (tiempoCargando < duracionCarga)
            {
                rb.velocity = new Vector2(direccion * velocidadCarga, rb.velocity.y);
                tiempoCargando += Time.deltaTime;
                yield return null;
            }

            rb.velocity = new Vector2(0, rb.velocity.y);
            if (animator != null) animator.SetBool("running", false);
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Movimientojugador>(out Movimientojugador playerComponent))
        {
            playerComponent.recibirDano(danoDB);
        }
    }

    public void ConsultarEnemigo(int id, int idNivel)
    {
        DatabaseManager script = DatabaseManager.Instance;
        if (script == null)
        {
            GameObject obj = GameObject.FindWithTag("Admin");
            if (obj != null) obj.TryGetComponent(out script);
        }

        if (script != null)
        {
            vidaDB = script.GetSaludEnemigo(id, idNivel);
            danoDB = script.GetDanoEnemigo(id, idNivel);
            if (vidaDB <= 0) vidaDB = 10;
            if (danoDB <= 0) danoDB = 300;
        }
        else
        {
            Debug.LogWarning("PatoMovimiento: Admin no encontrado. Usando valores por defecto.");
            vidaDB = 300;
            danoDB = 50;
        }
    }

    protected override void Morir()
    {
        updatePuntuacion(100);
        Destroy(gameObject, 0.2f);
    }

    public void updatePuntuacion(int pt)
    {
        GameObject objetoEncontrado = GameObject.FindWithTag("Canvas");

        if (objetoEncontrado != null)
        {
            if (objetoEncontrado.TryGetComponent<Puntuacion>(out Puntuacion script))
            {
                script.changePuntuacion(pt);
            }
        }
        else
        {
            Debug.LogError("Canvas no encontrado");
        }
    }

    public void reducirVelocidad(float vel)
    {
        velocidadCarga = vel;
    }

    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial;

        yield return new WaitForSeconds(0.2f);

        spriteRenderer.material = originalMaterial;

        flashRoutine = null;
    }
}