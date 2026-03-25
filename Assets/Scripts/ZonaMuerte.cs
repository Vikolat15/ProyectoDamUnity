using UnityEngine;

public class ZonaMuerte : MonoBehaviour
{
    public Transform puntoReaparicion;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (puntoReaparicion != null)
                other.transform.position = puntoReaparicion.position;
            else
                other.transform.position = Vector3.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (puntoReaparicion != null)
                collision.transform.position = puntoReaparicion.position;
            else
                collision.transform.position = Vector3.zero;
        }
    }
}