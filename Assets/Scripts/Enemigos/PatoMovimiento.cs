using UnityEngine;
using System.Collections;

public class PatoMovimiento : Entidad
{
    [SerializeField] private GameObject objetoCanvas;

    public float fuerzaSalto = 5f;
    public float velocidadHorizontal = 2f;
    public float esperaEntreSaltos = 1.0f; 
    public int saltosPorDireccion = 3;
    
    public Transform comprobadorSuelo; 
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;

    private Rigidbody2D rb;
    private Animator animator;
    private int direccion = -1;
    private int vidaDB;
    private int danoDB;

    [SerializeField] private Material flashMaterial;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;
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
        ConsultarEnemigo(1, 0);
        vida = VidaMaxima;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;

        StartCoroutine(SaltoCiclo());
    }

    void Update()
    {
        ActualizarAnimaciones();
    }

    IEnumerator SaltoCiclo()
    {
        while (true)
        {
            for (int i = 0; i < saltosPorDireccion; i++)
            {
                while (!EstaEnElSuelo())
                {
                    yield return null;
                }

                yield return new WaitForSeconds(esperaEntreSaltos);

                rb.velocity = new Vector2(direccion * velocidadHorizontal, fuerzaSalto);

                yield return new WaitForSeconds(0.2f);
            }

            direccion *= -1;

            float escalaX = direccion > 0 ? -1f : 1f;
            transform.localScale = new Vector3(escalaX, 1f, 1f);
        }
    }

    void ActualizarAnimaciones()
    {
        if (animator != null)
        {
            animator.SetBool("jumping", !EstaEnElSuelo());
        }
    }

    bool EstaEnElSuelo()
    {
        if (comprobadorSuelo != null)
        {
            return Physics2D.OverlapCircle(
                comprobadorSuelo.position,
                radioSuelo,
                capaSuelo
            );
        }

        return Mathf.Abs(rb.velocity.y) < 0.1f;
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

            if (vidaDB <= 0) vidaDB = 200;
            if (danoDB <= 0) danoDB = 35;
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
        velocidadHorizontal = vel;
    }

    public void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

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