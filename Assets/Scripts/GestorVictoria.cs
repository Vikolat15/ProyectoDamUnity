using UnityEngine;
using TMPro;

public class GestorVictoria : MonoBehaviour
{
    [Header("Referencias Directas (UI)")]
    public GameObject pantallaVictoria;
    public TextMeshProUGUI textoTiempo;
    public TextMeshProUGUI textoPuntuacion;
    [SerializeField] private GameObject objetoCanvas;
    public int nivelActualId = 0;

    private float tiempoTranscurrido = 0f;
    private int puntuacion = 0;

    private string nombrePerfil = "";
    private bool juegoTerminado = false;

    void Start()
    {
        if (pantallaVictoria != null)
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

        int minutos  = (int)(tiempoTranscurrido / 60);
        int segundos = (int)(tiempoTranscurrido % 60);

        textoTiempo.text = $"Tiempo: {minutos:00}:{segundos:00}";
        
        recibirPuntuacion();
        recibirNombrePerfil();
        textoPuntuacion.text = "Puntuacion: " + puntuacion.ToString();

        pantallaVictoria.SetActive(true);

        string nombreNivel = ObtenerNombreNivel();
        
        if (DatabaseManager.Instance != null)
        {
            DatabaseManager.Instance.insertarPunutuacionMaxima(nivelActualId, puntuacion);
        }


        if (ServerDatabaseManager.Instance != null)
        {
            ServerDatabaseManager.Instance.InsertarPuntuacionMaximaServer(nombrePerfil, nivelActualId,puntuacion);
        }


        MarcarProgresoNivel();

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void MarcarProgresoNivel()
    {
        // Acceso directo por Singleton
        if (DatabaseManager.Instance != null)
        {
            DatabaseManager.Instance.MarcarNivelCompletado(nivelActualId);
        }
        else
        {
            Debug.LogError("GestorVictoria: DatabaseManager.Instance no encontrado.");
        }
    }

    private string ObtenerNombreNivel()
    {
        switch (nivelActualId)
        {
            case 0:  return "Tutorial";
            case 1:  return "Nivel1";
            case 2:  return "Nivel2";
            case 3:  return "Nivel3";
            default: return "Nivel" + nivelActualId;
        }
    }

    public void recibirPuntuacion()
    {
        if (objetoCanvas != null && objetoCanvas.TryGetComponent<Puntuacion>(out Puntuacion script))
        {
            puntuacion = script.GetPuntuacionNivel();
        }
    }

    public void recibirNombrePerfil()
    {
        nombrePerfil = DatabaseManager.Instance.GetNombrePerfil(0);
    }
}