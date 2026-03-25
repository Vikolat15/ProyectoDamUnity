using UnityEngine;
using TMPro;

public class GestorVictoria : MonoBehaviour
{
    public GameObject pantallaVictoria;
    public TextMeshProUGUI textoTiempo;
    
    public TextMeshProUGUI textoPuntuacion;
    private float tiempoTranscurrido = 0f; 

    private int puntuacion = 0;
    private bool juegoTerminado = false;
    
    void Start()
    {        
        pantallaVictoria.SetActive(false);
        ResetearTiempo();
    }
    
    void Update()
    {
        if (!juegoTerminado)
        {
            tiempoTranscurrido += Time.deltaTime;
        }
    }
    
    public void ResetearTiempo()
    {
        tiempoTranscurrido = 0f; 
        juegoTerminado = false;
        Time.timeScale = 1f;
    }
    
    public void MostrarPantallaVictoria()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;
        
        int minutos = (int)(tiempoTranscurrido / 60);
        int segundos = (int)(tiempoTranscurrido % 60);
        
        textoTiempo.text = $"Tiempo: {minutos:00}:{segundos:00}";
        recibirPuntuacion();
        textoPuntuacion.text = "Puntuacion: " + puntuacion.ToString();

        pantallaVictoria.SetActive(true);

        insertarPunutuacion(0,0,"Tutorial",puntuacion);
        
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void recibirPuntuacion()
    {
        GameObject objetoEncontrado = GameObject.FindWithTag("Canvas");
    
        if (objetoEncontrado != null) 
        {
            if (objetoEncontrado.TryGetComponent<Puntuacion>(out Puntuacion script)) {
                puntuacion = script.GetPuntuacionNivel();
            }
        } else
        {
            Debug.LogError("Canvas no encontrado");
        }
    }

    public void insertarPunutuacion(int id, int idJuego, string nombre, int puntos)
    {
        GameObject objetoEncontrado = GameObject.FindWithTag("Admin");

        if (objetoEncontrado != null) 
        {
            if (objetoEncontrado.TryGetComponent<DatabaseManager>(out DatabaseManager script)) 
            {
                script.insertarPunutuacionMaxima(id, idJuego, nombre, puntos);
            }
        }
        else 
        {
            Debug.LogError("Admin no encontrado");
        }
    }
}