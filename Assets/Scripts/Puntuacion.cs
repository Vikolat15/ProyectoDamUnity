using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Puntuacion : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI TextoPuntuacion;

    public int puntuacion;

    void Start()
    {
        puntuacion = 0;
    }

    void Update()
    {
        TextoPuntuacion.text = puntuacion.ToString();
    }

    public void changePuntuacion(int pt)
    {
        puntuacion += pt;
        TextoPuntuacion.text = puntuacion.ToString();
    }

    public int GetPuntuacionNivel()
    {
        return puntuacion;
    }


}
