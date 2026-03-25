using UnityEngine;

public class GallinaMovimiento : Entidad
{
    [Header("Configuración de Movimiento")]
    public float velocidadPatrulla = 2f;
    public float velocidadPersecucion = 3.5f;
    public float tiempoPorDireccion = 2f;
    public float zonaMuertaHorizontal = 0.2f; 

    [Header("Detección")]
    public float rangoVision = 10f;
    public Animator animador;
    private Rigidbody2D rb;
    private Animator animator;
    private GameObject player;
    private float temporizador;
    private int direccion = 1;
    private bool tieneLineaDeVision = false;
    private int vidaDB;
    private int danoDB;
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
        ConsultarEnemigo(2, 0);
        vida = VidaMaxima; 
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
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
        else
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
        rb.velocity = new Vector2(direccion * velocidadActual, rb.velocity.y);
    }

void ActualizarDeteccion()
{
    if (player == null) return;

    Vector3 origen = new Vector3(transform.position.x, transform.position.y, 0);
    
    Vector3 destino = player.GetComponent<Collider2D>().bounds.center;
    destino.z = 0; 

    Vector2 direccionRayo = (destino - origen).normalized;

    RaycastHit2D[] hits = Physics2D.RaycastAll(origen, direccionRayo, rangoVision);
    
    bool pudoVerAlJugador = false;

    foreach (var hit in hits)
    {
        if (hit.collider.gameObject == gameObject) continue;

        if (hit.collider.CompareTag("Player"))
        {
            pudoVerAlJugador = true;
            break; 
        }

        if (!hit.collider.isTrigger) 
        {
            pudoVerAlJugador = false;
            break; 
        }
    }

    tieneLineaDeVision = pudoVerAlJugador;
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
        if (collision.gameObject.TryGetComponent<Movimientojugador>(out Movimientojugador playerComponent))
        {
            playerComponent.recibirDano(50);
        }
    }

    public void ConsultarEnemigo(int id, int idNivel)
    {
        GameObject objetoEncontrado = GameObject.FindWithTag("Admin");

        if (objetoEncontrado != null) 
        {
            if (objetoEncontrado.TryGetComponent<DatabaseManager>(out DatabaseManager script)) 
            {
                vidaDB = script.GetSaludEnemigo(id, idNivel);
                danoDB = script.GetDanoEnemigo(id, idNivel);
                
            }
        } 
        else 
        {
            Debug.LogError("Admin no encontrado");
        }
    }

    protected override void Morir()
    {
        updatePuntuacion(50);
        Destroy(gameObject);
    }

    public void updatePuntuacion(int pt)
    {
        GameObject objetoEncontrado = GameObject.FindWithTag("Canvas");
    
        if (objetoEncontrado != null) 
        {
            if (objetoEncontrado.TryGetComponent<Puntuacion>(out Puntuacion script)) {
                script.changePuntuacion(pt);
            }
        } else
        {
            Debug.LogError("Canvas no encontrado");
            return;
        }
    }


        public void reducirVelocidad(float vel)
    {
        velocidadPersecucion = vel * 2;
        velocidadPatrulla = vel;
    }
}