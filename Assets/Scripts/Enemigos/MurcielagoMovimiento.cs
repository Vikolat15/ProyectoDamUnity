using UnityEngine;
using System.Collections;

public class MurcielagoMovimiento : Entidad
{
    public float Velocidad = 2f;
    public float tiempoPorDireccion = 2f;
    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float temporizador;
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
        ConsultarEnemigo(3, 0);
        vida = VidaMaxima; 
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Rigidbody2D.gravityScale = 0;

        spriteRenderer = GetComponent<SpriteRenderer>();
            
        originalMaterial = spriteRenderer.material;
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
        Rigidbody2D.velocity = new Vector2(0, direccion * Velocidad);
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
            if (objetoEncontrado.TryGetComponent<Puntuacion>(out Puntuacion script)) {
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
        Velocidad = vel;
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