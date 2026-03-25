using UnityEngine;
using System.Collections;

public class PatoMovimiento : Entidad
{
    public float fuerzaSalto = 5f;
    public float velocidadHorizontal = 2f;
    public float esperaEntreSaltos = 1.0f; 
    public int saltosPorDireccion = 3;
    public Animator animador;
    
    [Header("Detección de Suelo")]
    public Transform comprobadorSuelo; 
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;
    private Rigidbody2D rb;
    private Animator animator;
    private int direccion = -1;
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
        ConsultarEnemigo(1, 0);
        vida = VidaMaxima; 
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
                yield return new WaitForFixedUpdate();

                while (!EstaEnElSuelo())
                {
                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForSeconds(esperaEntreSaltos);

                rb.velocity = new Vector2(direccion * velocidadHorizontal, fuerzaSalto);

                yield return new WaitForSeconds(0.2f);
            }

            direccion *= -1;
            transform.localScale = new Vector3(direccion > 0 ? -1 : 1, 1, 1);
        }
    }

    void ActualizarAnimaciones()
    {
        if (animator == null || rb == null) return;

        bool enSuelo = EstaEnElSuelo();

        animator.SetBool("jumping", !enSuelo);
    }


    bool EstaEnElSuelo()
    {
        if (comprobadorSuelo != null)
        {
            return Physics2D.OverlapCircle(comprobadorSuelo.position, radioSuelo, capaSuelo);
        }
        return Mathf.Abs(rb.velocity.y) < 0.1f;
    }

    private void OnDrawGizmos()
    {
        if (comprobadorSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(comprobadorSuelo.position, radioSuelo);
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
        velocidadHorizontal = vel;
    }
}