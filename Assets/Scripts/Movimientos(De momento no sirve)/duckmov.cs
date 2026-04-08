using UnityEngine;
using System.Collections;

public class MovimientoSaltoAutomatico : MonoBehaviour
{
    public float fuerzaSalto = 5f;
    public float velocidadHorizontal = 2f;
    public float esperaEntreSaltos = 1.0f; // Un poco más de tiempo ayuda a la estabilidad
    public int saltosPorDireccion = 3;
    
    [Header("Detección de Suelo")]
    public Transform comprobadorSuelo; // Crea un objeto vacío a los pies del enemigo
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;

    private Rigidbody2D rb;
    private Animator animator;
    private int direccion = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(SaltoCiclo());
    }

    IEnumerator SaltoCiclo()
    {
        while (true)
        {
            for (int i = 0; i < saltosPorDireccion; i++)
            {
                // 1. Esperar un frame por si acaso
                yield return new WaitForFixedUpdate();

                // 2. Esperar de forma segura hasta tocar el suelo
                // Usamos una pequeña espera inicial para que no detecte el suelo justo al despegar
                while (!EstaEnElSuelo())
                {
                    yield return new WaitForSeconds(0.1f);
                }

                // 3. Pequeña pausa antes de volver a saltar para que no sea instantáneo
                yield return new WaitForSeconds(esperaEntreSaltos);

                // 4. Saltar
                rb.velocity = new Vector2(direccion * velocidadHorizontal, fuerzaSalto);
                
                if (animator != null) animator.SetTrigger("jump");
                
                // 5. Esperar un poco para que el enemigo despegue del suelo antes de volver a chequear
                yield return new WaitForSeconds(0.2f);
            }

            // Cambiar dirección
            direccion *= -1;
            transform.localScale = new Vector3(direccion > 0 ? -1 : 1, 1, 1);
        }
    }

    bool EstaEnElSuelo()
    {
        // Si no tienes configurado un "comprobadorSuelo", esto usará la velocidad como plan B
        if (comprobadorSuelo != null)
        {
            return Physics2D.OverlapCircle(comprobadorSuelo.position, radioSuelo, capaSuelo);
        }
        return Mathf.Abs(rb.velocity.y) < 0.1f;
    }

    // Dibujar el círculo de detección en el editor
    private void OnDrawGizmos()
    {
        if (comprobadorSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(comprobadorSuelo.position, radioSuelo);
        }
    }
}