using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moneda : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

private void OnTriggerEnter2D(Collider2D collision)
{
if (collision.CompareTag("Player"))
    {
        GetComponent<Animator>().SetBool("recogida", true);
        
        GetComponent<Collider2D>().enabled = false;
        
        updatePuntuacion(25);

        Destroy(gameObject, 0.35f);
    }
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
}
