using UnityEngine;

public class MovimientoAutomaticoTiempo : MonoBehaviour
{
    public float Velocidad = 2f;
    public float tiempoPorDireccion = 2f;

    private Rigidbody2D Rigidbody2D;
    private Animator Animator;

    private float temporizador;
    private int direccion = 1; // 1 = derecha, -1 = izquierda

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        Animator.SetBool("running", true);

        temporizador += Time.deltaTime;

        if (temporizador >= tiempoPorDireccion)
        {
            direccion *= -1;
            temporizador = 0f;
        }

        // Girar sprite
        if (direccion > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2(direccion * Velocidad, Rigidbody2D.velocity.y);
    }
}
