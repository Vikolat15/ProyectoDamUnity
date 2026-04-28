using UnityEngine;
using TMPro;

public class GestorVictoria : MonoBehaviour
{
    public GameObject pantallaVictoria;
    public TextMeshProUGUI textoTiempo;
    public TextMeshProUGUI textoPuntuacion;

    // ---------------------------------------------------------------
    // IMPORTANTE: Asigna en el Inspector el nivelId de esta escena:
    //   Tutorial = 0 | Nivel1 = 1 | Nivel2 = 2 | Nivel3 = 3
    // ---------------------------------------------------------------
    [Header("Progreso de niveles")]
    [Tooltip("0=Tutorial  1=Nivel1  2=Nivel2  3=Nivel3")]
    public int nivelActualId = 0;

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

        int minutos  = (int)(tiempoTranscurrido / 60);
        int segundos = (int)(tiempoTranscurrido % 60);

        textoTiempo.text = $"Tiempo: {minutos:00}:{segundos:00}";
        recibirPuntuacion();
        textoPuntuacion.text = "Puntuacion: " + puntuacion.ToString();

        pantallaVictoria.SetActive(true);

        // Guardar puntuación (lógica original)
        insertarPunutuacion(nivelActualId, 0, ObtenerNombreNivel(), puntuacion);
        InsertarPuntuacionServer(nivelActualId,0,ObtenerNombreNivel(),puntuacion);

        // --- NUEVO: marcar este nivel como completado y desbloquear el siguiente ---
        MarcarProgresoNivel();

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // ---------------------------------------------------------------
    //  PROGRESO
    // ---------------------------------------------------------------

    /// <summary>
    /// Busca el DatabaseManager y marca el nivel actual como completado.
    /// </summary>
    private void MarcarProgresoNivel()
    {
        DatabaseManager db = DatabaseManager.Instance;
        if (db == null)
        {
            GameObject adminObj = GameObject.FindWithTag("Admin");
            if (adminObj != null) adminObj.TryGetComponent(out db);
        }

        if (db != null)
        {
            db.MarcarNivelCompletado(nivelActualId);
            Debug.Log($"Nivel {nivelActualId} ({ObtenerNombreNivel()}) completado.");
        }
        else
        {
            Debug.LogError("GestorVictoria: No se encontró DatabaseManager.");
        }
    }

    /// <summary>
    /// Devuelve el nombre legible del nivel actual según nivelActualId.
    /// </summary>
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

    // ---------------------------------------------------------------
    //  MÉTODOS EXISTENTES (sin cambios)
    // ---------------------------------------------------------------

    public void recibirPuntuacion()
    {
        GameObject objetoEncontrado = GameObject.FindWithTag("Canvas");

        if (objetoEncontrado != null)
        {
            if (objetoEncontrado.TryGetComponent<Puntuacion>(out Puntuacion script))
            {
                puntuacion = script.GetPuntuacionNivel();
            }
        }
        else
        {
            Debug.LogError("Canvas no encontrado");
        }
    }

    public void insertarPunutuacion(int id, int idJuego, string nombre, int puntos)
    {
        DatabaseManager script = DatabaseManager.Instance;
        if (script == null)
        {
            GameObject obj = GameObject.FindWithTag("Admin");
            if (obj != null) obj.TryGetComponent(out script);
        }

        if (script != null)
            script.insertarPunutuacionMaxima(id, idJuego, nombre, puntos);
        else
            Debug.LogError("GestorVictoria: Admin no encontrado al insertar puntuación.");
    }

    public void InsertarPuntuacionServer(int id, int idJuego, string nombre, int puntos)
    {
        ServerDatabaseManager script = ServerDatabaseManager.Instance;
        if (script == null)
        {
            GameObject objetoEncontrado = GameObject.FindWithTag("Admin");
            if (objetoEncontrado != null) 
            {
                objetoEncontrado.TryGetComponent(out script);
            }
        }

        if (script != null) 
        {
            script.InsertarPuntuacionMaximaServer(id, idJuego, nombre, puntos);
        }
        else 
        {
            Debug.LogError("ServerDatabaseManager no encontrado.");
        }
    }   
}