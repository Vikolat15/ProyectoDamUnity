using UnityEngine;
using System.Collections;

public class GallinaMovimiento : Entidad
{
    [SerializeField] private GameObject player; 
    [SerializeField] private GameObject objetoCanvas;

    [Header("Configuración de Movimiento")]
    public float velocidadPatrulla = 2f;
    public float velocidadPersecucion = 3.5f;
    public float tiempoPorDireccion = 2f;
    public float zonaMuertaHorizontal = 0.2f; 

    public float rangoVision = 10f;
    public Animator animador; 

    private Rigidbody2D Rigidbody2D;
    private Animator animator;
    private float temporizador;
    private int direccion = 1;
    private bool tieneLineaDeVision = false;
    private int vidaDB;
    private int danoDB;

    [SerializeField] private Material flashMaterial;
    private SpriteRenderer spriteRenderer;
    private Material Material;
    [SerializeField] private Coroutine flashRoutine;
    [SerializeField] public AudioSource audioSource;

    [SerializeField] private int nivel;
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

    new void Start()
    {
        ConsultarEnemigo(2, 0);
        vida = VidaMaxima; 
        Rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Material = spriteRenderer.material;
    }

    void Update()
    {
        if (animator != null)
            animator.SetBool("running", direccion != 0);

        ActualizarDeteccion();

        if (!tieneLineaDeVision)
        {
            temporizador += Time.deltaTime;
            if (temporizador >= tiempoPorDireccion)
            {
                direccion *= -1;
                temporizador = 0f;
            }
        }
        else if (player != null)
        {
            float distanciaX = player.transform.position.x - transform.position.x;

            if (Mathf.Abs(distanciaX) > zonaMuertaHorizontal)
            {
                direccion = (distanciaX > 0) ? 1 : -1;
            }
            else
            {
                direccion = 0;
            }
        }

        Flip();
    }

    void FixedUpdate()
    {
        float velocidadActual = tieneLineaDeVision ? velocidadPersecucion : velocidadPatrulla;
        Rigidbody2D.velocity = new Vector2(direccion * velocidadActual, Rigidbody2D.velocity.y);
    }

    void ActualizarDeteccion()
    {
        Vector3 origen = transform.position;
        Vector3 destino = player.GetComponent<Collider2D>().bounds.center;
        destino.z = 0; 

        Vector2 direccionRayo = (destino - origen).normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(origen, direccionRayo, rangoVision);
        
        bool viendoJugador = false;

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                viendoJugador = true;
                break; 
            }

            if (!hit.collider.isTrigger) 
            {
                viendoJugador = false;
                break; 
            }
        }

        tieneLineaDeVision = viendoJugador;
    }

    void Flip()
    {
        if (direccion > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (direccion < 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = tieneLineaDeVision ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Movimientojugador>(out Movimientojugador player))
        {
            Rigidbody2D Rigidbody2DPlayer = collision.gameObject.GetComponent<Rigidbody2D>();

            bool empujado = Rigidbody2DPlayer.velocity.y < -0.1f && Rigidbody2DPlayer.transform.position.y > transform.position.y + 0.5f;

                player.recibirDano(danoDB);
                StartCoroutine(Empuje(player, Rigidbody2DPlayer));
            
        }
    }

    private IEnumerator Empuje(Movimientojugador player, Rigidbody2D Rigidbody2D)
    {
        player.recibiendoEmpuje = true;

        float Dirrecion = (Rigidbody2D.transform.position.x > transform.position.x) ? 1f : -1f;

        float fuerzaHorizontal = 8f;
        float fuerzaVertical = 12f;    

        Rigidbody2D.velocity = new Vector2(Dirrecion * fuerzaHorizontal, fuerzaVertical);

        yield return new WaitForSeconds(0.2f);

        Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x * 0.5f, Rigidbody2D.velocity.y);

        player.recibiendoEmpuje = false;
    }

    public void ConsultarEnemigo(int id, int idNivel)
    {
        if (DatabaseManager.Instance != null)
        {
            vidaDB = DatabaseManager.Instance.GetSaludEnemigo(id, idNivel);
            danoDB = DatabaseManager.Instance.GetDanoEnemigo(id, idNivel);
        }
        else
        {
            vidaDB = 200;
            danoDB = 50;
        }
    }

    protected override void Morir()
    {
        updatePuntuacion(50);
        Destroy(gameObject, 0.2f);
    }

    public void updatePuntuacion(int pt)
    {
            if (objetoCanvas.TryGetComponent<Puntuacion>(out Puntuacion script)) {
                script.changePuntuacion(pt);
            }
    }

    public void reducirVelocidad(float vel)
    {
        velocidadPersecucion = vel * 2;
        velocidadPatrulla = vel;
    }

    public void Flash()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material = Material;
        flashRoutine = null;
    }
}