using UnityEngine;

public class EnemigoInteligente : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidadPatrulla = 2f;
    public float velocidadPersecucion = 3.5f;
    public float tiempoPorDireccion = 2f;
    public float zonaMuertaHorizontal = 0.2f; // <-- Nueva: Margen para evitar temblores

    [Header("Detección")]
    public float rangoVision = 10f;

    private Rigidbody2D rb;
    private Animator animator;
    private GameObject player;

    private float temporizador;
    private int direccion = 1; 
    private bool tieneLineaDeVision = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (animator != null) animator.SetBool("running", true);

        ActualizarDeteccion();

        if (!tieneLineaDeVision)
        {
            // Lógica de Patrulla
            temporizador += Time.deltaTime;
            if (temporizador >= tiempoPorDireccion)
            {
                direccion *= -1;
                temporizador = 0f;
            }
        }
        else
        {
            // Lógica de Persecución con Zona Muerta
            float distanciaX = player.transform.position.x - transform.position.x;

            // Solo cambia la dirección si está fuera del margen de la zona muerta
            if (Mathf.Abs(distanciaX) > zonaMuertaHorizontal)
            {
                direccion = (distanciaX > 0) ? 1 : -1;
            }
            else
            {
                // Si está justo debajo/encima, se queda quieto horizontalmente
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

        Vector2 haciaJugador = (player.transform.position - transform.position);
        RaycastHit2D ray = Physics2D.Raycast(transform.position, haciaJugador.normalized, rangoVision);

        if (ray.collider != null)
        {
            tieneLineaDeVision = ray.collider.CompareTag("Player");
        }
        else
        {
            tieneLineaDeVision = false;
        }
    }

    void Flip()
    {
        // Solo giramos si la dirección es distinta de 0
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
}