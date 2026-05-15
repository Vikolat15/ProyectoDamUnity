using UnityEngine;

public class Moneda : MonoBehaviour
{
    [SerializeField] private GameObject objetoCanvas;

    private Animator animator;
    private Collider2D col;

    void Start()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetBool("recogida", true);
            
            col.enabled = false;
            
            updatePuntuacion(25);

            Destroy(gameObject, 0.35f);
        }
    }

    public void updatePuntuacion(int pt)
    {
        // Usamos la referencia directa asignada en lugar de Find
        if (objetoCanvas != null && objetoCanvas.TryGetComponent<Puntuacion>(out Puntuacion script))
        {
            script.changePuntuacion(pt);
        }
    }
}