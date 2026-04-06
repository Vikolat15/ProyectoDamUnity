using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimientojugador : Entidad
{
    public GameObject BulletPrefabBase;
    public GameObject BulletPrefabFuego;
    public GameObject BulletPrefabHielo;
    public RuntimeAnimatorController animacionBase;
    public RuntimeAnimatorController animacionFuego;
    public RuntimeAnimatorController animacionHielo;
    public float Velocidad;
    public float Salto;
    private Rigidbody2D Rigidbody2D;
    private float Horizontal;
    private Animator Animator;
    public Vector2 tamannoBox;
    public float distanciaBox = 0.001f;
    public LayerMask capaSuelo;
    public int Habilidad;
    public int puntuacionMaxima = 0;
    public float tiempoInvulnerable = 1.0f; 
    private float siguienteDanoPosible = 0f;
    private float tiempoSinDisparar = 0.7f;
    private float SiguienteDisparoPosible = 0f;
    private int vidaDB;
    public float coyoteTime = 0.1f;
    private float coyoteTimeCounter;
    public float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    public override int VidaMaxima {
        get { return vidaDB; } 
        protected set { base.VidaMaxima = value; }
    }

    private bool existeMenu = false;

    void Start()
    {
        ConsultarPersonage(0,0);
        vida = VidaMaxima; 
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Animator.SetBool("running", Horizontal != 0.0f);

        if (Horizontal > 0.0f) {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        } else if (Horizontal < 0.0f) {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        bool enSuelo = tocaSuelo();

        if (enSuelo) {
            coyoteTimeCounter = coyoteTime; 
        } else {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            jumpBufferCounter = jumpBufferTime;
        } else {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && !existeMenu) {
            if (enSuelo || coyoteTimeCounter > 0f) {
                Jump();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && Rigidbody2D.velocity.y > 0) {
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, Rigidbody2D.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }

        if (Input.GetMouseButtonDown(0) && !existeMenu) {
            if (Time.time >= SiguienteDisparoPosible) {
                SiguienteDisparoPosible = Time.time + tiempoSinDisparar;
                Shoot();
            }
        }

        if (Habilidad == 0) {
            Animator.runtimeAnimatorController = animacionBase;
        } else if (Habilidad == 1) {
            Animator.runtimeAnimatorController = animacionFuego;
        } else if (Habilidad == 2) {
            Animator.runtimeAnimatorController = animacionHielo;
        }
    }

    void FixedUpdate(){
        Rigidbody2D.velocity = new Vector2((Velocidad * Horizontal), Rigidbody2D.velocity.y);
    }

    void Jump(){
        Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, 0f);
        Rigidbody2D.AddForce(Vector2.up * Salto, ForceMode2D.Impulse);

        jumpBufferCounter = 0f;
        coyoteTimeCounter = 0f;
    }

    public void Shoot()
    {
        StartCoroutine(DispararConRetraso());
    }

    private IEnumerator DispararConRetraso()
    {
        if (Animator != null) 
            Animator.SetTrigger("shooting");

        yield return new WaitForSeconds(0.4f);

        Vector3 direccion;
        if (transform.localScale.x == 1.0f)
        {
            direccion = Vector2.left;
        }
        else
        {
            direccion = Vector2.right;
        }

        Vector3 spawnBala = transform.position + new Vector3(0f, 0.8f, 0f);
        spawnBala.z = -1.0f;
        
        GameObject bullet = null;

        if (Habilidad == 0)
        {
            bullet = Instantiate(BulletPrefabBase, spawnBala, Quaternion.identity);
            bullet.GetComponent<BalaBase>().SetDireccion(direccion);
        }
        else if (Habilidad == 1)
        {
            bullet = Instantiate(BulletPrefabFuego, spawnBala, Quaternion.identity);
            bullet.GetComponent<BalaFuego>().SetDireccion(direccion);
        }
        else if (Habilidad == 2)
        {
            bullet = Instantiate(BulletPrefabHielo, spawnBala, Quaternion.identity);
            bullet.GetComponent<BalaHielo>().SetDireccion(direccion);
        }
    }

    public bool tocaSuelo()
    {
        if (Physics2D.BoxCast(transform.position, tamannoBox, 0,
        -transform.up, distanciaBox, capaSuelo))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void recibirDano(int damage)
    {
        if (Time.time >= siguienteDanoPosible)
        {
            siguienteDanoPosible = Time.time + tiempoInvulnerable;
            vida -= damage;
            updateHealthBar(vida);
            
            if (vida <= 0)
            {
                Respawn();
            }
        }
    }

    private void Respawn()
    {
        vida = 100;
        updateHealthBar(vida);
        
        GameObject respawnObj = GameObject.FindGameObjectWithTag("Respawn");
        if (respawnObj != null)
        {
            transform.position = respawnObj.transform.position;
            
            if (Rigidbody2D != null)
            {
                Rigidbody2D.velocity = Vector2.zero;
                Rigidbody2D.angularVelocity = 0f;
            }
        }
        else
        {
            transform.position = Vector3.zero;
        }
    }

    public void updateHealthBar(int hp)
    {
        GameObject objetoEncontrado = GameObject.FindWithTag("Canvas");
    
        if (objetoEncontrado != null) 
        {
            if (objetoEncontrado.TryGetComponent<HealthBarScript>(out HealthBarScript script)) {
                script.changeHealthBar(hp);
            }
        } else
        {
            Debug.LogError("Canvas no encontrado");
            return;
        }
    }

    public void ConsultarPersonage(int id, int idNivel)
    {
        GameObject objetoEncontrado = GameObject.FindWithTag("Admin");

        if (objetoEncontrado != null) 
        {
            if (objetoEncontrado.TryGetComponent<DatabaseManager>(out DatabaseManager script)) 
            {
                vidaDB = script.GetSaludPersonaje(id, idNivel);
            }
        }
        else 
        {
            Debug.LogError("Admin no encontrado");
        }
    }

    public void setHabilidad(int habilidad) {
        Habilidad = habilidad;
    }
}
