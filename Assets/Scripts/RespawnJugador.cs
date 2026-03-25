using UnityEngine;

public class RespawnJugador : MonoBehaviour
{
    public Transform puntoRespawn;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Muerte"))
        {
            transform.position = puntoRespawn.position;
            
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}