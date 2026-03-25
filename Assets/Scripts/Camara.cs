using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoCamara : MonoBehaviour
{
    public GameObject personaje;
    public float suavizado = 0.3f; 
    public Vector3 offset = new Vector3(0, 1.5f, -10);
    
    private Vector3 velocidad = Vector3.zero;
    

    void Start()
    {
       
        if (personaje != null)
        {
            transform.position = new Vector3(
                personaje.transform.position.x + offset.x,
                personaje.transform.position.y + offset.y,
                offset.z
            );
        }
    }

    void LateUpdate()
    {
        if (personaje != null)
        {
            Vector3 posicionObjetivo = new Vector3(
                personaje.transform.position.x + offset.x,
                personaje.transform.position.y + offset.y,
                offset.z
            );


            transform.position = Vector3.SmoothDamp(
                transform.position,
                posicionObjetivo,
                ref velocidad,
                suavizado
            );
        }
    }
}