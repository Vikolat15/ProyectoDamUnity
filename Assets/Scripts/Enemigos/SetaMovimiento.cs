using UnityEngine;

public class setaMovimiento : Entidad
{
    public float Velocidad = 2f;
    public float tiempoPorDireccion = 2f;
    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float temporizador;
    private int direccion = 1; 
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
        ConsultarEnemigo(0, 0);
        vida = VidaMaxima; 
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {

        temporizador += Time.deltaTime;

        if (temporizador >= tiempoPorDireccion)
        {
            direccion *= -1;
            temporizador = 0f;
        }

        if (direccion > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2(direccion * Velocidad, Rigidbody2D.velocity.y);
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
            if (vidaDB <= 0) vidaDB = 300;
            if (danoDB <= 0) danoDB = 30;
        }
        else
        {
            Debug.LogWarning("setaMovimiento: Admin no encontrado. Usando valores por defecto.");
            vidaDB = 300;
            danoDB = 30;
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
        Velocidad = vel;
    }
}