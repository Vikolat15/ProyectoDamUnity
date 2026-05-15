using UnityEngine;
using System.Collections;

public class MurcielagoMovimiento : Entidad
{
    public float Velocidad = 2f;
    public float tiempoPorDireccion = 2f;
    
    [Header("Referencias Directas")]
    [SerializeField] private GameObject objetoCanvas; 
    [SerializeField] private Material flashMaterial;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;

    private float temporizador;
    private int direccion = 1; 
    private int vidaDB;
    private int danoDB;

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
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null) originalMaterial = spriteRenderer.material;
        if (rb != null) rb.gravityScale = 0;

        ConsultarEnemigo(0, 0); 
        
        vida = VidaMaxima; 

        if (objetoCanvas == null)
        {
            Debug.LogWarning("Murcielago: No se ha asignado objetoCanvas en el Inspector.");
        }
    }

    void Update()
    {
        temporizador += Time.deltaTime;

        if (temporizador >= tiempoPorDireccion)
        {
            direccion *= -1;
            temporizador = 0f;
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.velocity = new Vector2(0, direccion * Velocidad);
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
            vidaDB = 300;
            danoDB = 2;
        }
    }

    protected override void Morir()
    {
        updatePuntuacion(100);
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
        Velocidad = vel;
    }

    public void Flash()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        if (spriteRenderer != null && flashMaterial != null)
        {
            spriteRenderer.material = flashMaterial;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.material = originalMaterial;
        }
        flashRoutine = null;
    }
}