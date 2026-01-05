using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float Velocidad = 8f;
    public float FuerzaSalto = 12f;
    
    [Header("Detección de Suelo")]
    public Transform puntoSuelo; // Arrastra un GameObject hijo aquí
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;
    
    [Header("Configuración de Salto")]
    public bool dobleSalto = true;
    
    // Variables privadas
    private float movimientoHorizontal;
    private bool quiereSaltar;
    private int saltosRestantes;
    private Rigidbody2D cuerpo;
    private Animator animator; // Opcional para animaciones
    private bool mirandoDerecha = true;

    void Start()
    {
        cuerpo = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Si tienes animaciones
        
        // Crear automáticamente el punto de suelo si no existe
        if (puntoSuelo == null)
        {
            CrearPuntoSuelo();
        }
        
        saltosRestantes = dobleSalto ? 2 : 1;
    }

    void Update()
    {
        // MOVIMIENTO HORIZONTAL
        movimientoHorizontal = Input.GetAxisRaw("Horizontal"); // Raw para movimiento instantáneo
        
        // SALTO
        if (Input.GetButtonDown("Jump"))
        {
            if (tocaSuelo())
            {
                // Primer salto desde el suelo
                quiereSaltar = true;
                saltosRestantes = dobleSalto ? 1 : 0;
            }
            else if (dobleSalto && saltosRestantes > 0)
            {
                // Doble salto en el aire
                quiereSaltar = true;
                saltosRestantes--;
            }
        }
        
        // Actualizar animaciones (si tienes Animator)
        if (animator != null)
        {
            animator.SetFloat("Velocidad", Mathf.Abs(movimientoHorizontal));
            animator.SetBool("EnSuelo", tocaSuelo());
            animator.SetFloat("VelocidadY", cuerpo.velocity.y);
        }
        
        // Voltear sprite según dirección
        if (movimientoHorizontal > 0 && !mirandoDerecha)
        {
            Voltear();
        }
        else if (movimientoHorizontal < 0 && mirandoDerecha)
        {
            Voltear();
        }
    }

    void FixedUpdate()
    {
        // APLICAR MOVIMIENTO HORIZONTAL
        cuerpo.velocity = new Vector2(movimientoHorizontal * Velocidad, cuerpo.velocity.y);
        
        // APLICAR SALTO
        if (quiereSaltar)
        {
            // Fórmula mejorada para salto consistente
            cuerpo.velocity = new Vector2(cuerpo.velocity.x, 0); // Resetear velocidad Y
            cuerpo.AddForce(Vector2.up * FuerzaSalto, ForceMode2D.Impulse);
            
            Debug.Log("¡Saltando! Fuerza: " + FuerzaSalto);
            quiereSaltar = false;
        }
        
        // Resetear saltos cuando toca el suelo
        if (tocaSuelo())
        {
            saltosRestantes = dobleSalto ? 2 : 1;
        }
    }

    bool tocaSuelo()
    {
        // Chequeo de suelo con círculo (más eficiente que BoxCast)
        if (puntoSuelo == null) return false;
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(puntoSuelo.position, radioSuelo, capaSuelo);
        
        foreach (Collider2D col in colliders)
        {
            if (col.gameObject != gameObject) // Asegurarse de no detectarse a sí mismo
            {
                return true;
            }
        }
        return false;
    }

    void Voltear()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    void CrearPuntoSuelo()
    {
        GameObject punto = new GameObject("PuntoSuelo");
        punto.transform.SetParent(transform);
        punto.transform.localPosition = new Vector3(0, -0.5f, 0); // Medio cuerpo hacia abajo
        puntoSuelo = punto.transform;
        
        Debug.Log("Punto de suelo creado automáticamente");
    }

    void OnDrawGizmosSelected()
    {
        // Visualizar área de detección de suelo
        if (puntoSuelo != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(puntoSuelo.position, radioSuelo);
        }
    }
}