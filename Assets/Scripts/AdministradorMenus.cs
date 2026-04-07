using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
// prueba git 
public class AdministradorMenus : MonoBehaviour
{
    public GameObject panelMenuPrincipal;
    public GameObject panelSeleccionNiveles;
    public GameObject panelCreditos;

    public GameObject canvasInfo;

    public AudioSource audioSource;
    public AudioClip sonidoClick;
    public TextMeshProUGUI textoPuntiacionTutorial;

    [Header("Botones de niveles (para bloqueo)")]
    public Button botonNivel1;
    public Button botonNivel2;
    public Button botonNivel3;

    public static bool menuActivo = false;

    public int nivel  = 0;
    public int record = 0;

    public static AdministradorMenus Instance { get; private set; }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        MostrarMenuPrincipal();
        if (canvasInfo != null)
            canvasInfo.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelSeleccionNiveles.activeSelf || panelCreditos.activeSelf)
            {
                MostrarMenuPrincipal();
            }
        }
    }

    void ReproducirClick()
    {
        if (audioSource != null && sonidoClick != null)
            audioSource.PlayOneShot(sonidoClick);
    }

    public void MostrarMenuPrincipal()
    {
        panelMenuPrincipal.SetActive(true);
        panelSeleccionNiveles.SetActive(false);
        panelCreditos.SetActive(false);

        if (canvasInfo != null)
            canvasInfo.SetActive(false);
    }

    public void AlPresionarJugar()
    {
        ReproducirClick();
        panelMenuPrincipal.SetActive(false);
        panelSeleccionNiveles.SetActive(true);
        ConsultarPuntuacion(0, 0);
        textoPuntiacionTutorial.text = "Record : " + record.ToString();

        ActualizarBotonesNiveles();

        if (canvasInfo != null)
            canvasInfo.SetActive(false);
    }

    private void ActualizarBotonesNiveles()
    {
        DatabaseManager db = ObtenerDatabaseManager();
        if (db == null) return;

        if (botonNivel1 != null) botonNivel1.interactable = db.IsNivelDesbloqueado(1);
        if (botonNivel2 != null) botonNivel2.interactable = db.IsNivelDesbloqueado(2);
        if (botonNivel3 != null) botonNivel3.interactable = db.IsNivelDesbloqueado(3);
    }

    private DatabaseManager ObtenerDatabaseManager()
    {
        GameObject adminObj = GameObject.FindWithTag("Admin");
        if (adminObj != null && adminObj.TryGetComponent<DatabaseManager>(out DatabaseManager db))
            return db;

        Debug.LogError("AdministradorMenus: No se encontró el objeto con Tag 'Admin'.");
        return null;
    }

    public void CargarTutorial()
    {
        ReproducirClick();
        SceneManager.LoadScene("Tutorial");

        if (canvasInfo != null)
            Invoke("ActivarCanvasInfo", 0.1f);
    }

    public void CargarNivel1()
    {
        DatabaseManager db = ObtenerDatabaseManager();
        if (db != null && !db.IsNivelDesbloqueado(1))
        {
            Debug.Log("Nivel 1 bloqueado. Completa el Tutorial primero.");
            return;
        }
        ReproducirClick();
        SceneManager.LoadScene("Nivel1");

        if (canvasInfo != null)
            canvasInfo.SetActive(false);
    }

    public void CargarNivel2()
    {
        DatabaseManager db = ObtenerDatabaseManager();
        if (db != null && !db.IsNivelDesbloqueado(2))
        {
            Debug.Log("Nivel 2 bloqueado. Completa el Nivel 1 primero.");
            return;
        }
        ReproducirClick();
        SceneManager.LoadScene("Nivel2");

        if (canvasInfo != null)
            canvasInfo.SetActive(false);
    }

    public void CargarNivel3()
    {
        DatabaseManager db = ObtenerDatabaseManager();
        if (db != null && !db.IsNivelDesbloqueado(3))
        {
            Debug.Log("Nivel 3 bloqueado. Completa el Nivel 2 primero.");
            return;
        }
        ReproducirClick();
        SceneManager.LoadScene("Nivel3");

        if (canvasInfo != null)
            canvasInfo.SetActive(false);
    }

    public void AlPresionarCreditos()
    {
        ReproducirClick();
        panelMenuPrincipal.SetActive(false);
        panelCreditos.SetActive(true);

        if (canvasInfo != null)
            canvasInfo.SetActive(false);
    }

    public void AlPresionarSalir()
    {
        ReproducirClick();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void AlPresionarVolverNiveles()
    {
        ReproducirClick();
        MostrarMenuPrincipal();
    }

    public void AlPresionarVolverCreditos()
    {
        ReproducirClick();
        MostrarMenuPrincipal();
    }

    public void ConsultarPuntuacion(int id, int idNivel)
    {
        GameObject objetoEncontrado = GameObject.FindWithTag("Admin");

        if (objetoEncontrado != null)
        {
            if (objetoEncontrado.TryGetComponent<DatabaseManager>(out DatabaseManager script))
            {
                record = script.GetPuntuacionNivel(id, idNivel);
            }
        }
        else
        {
            Debug.LogError("¡No encontré ningún objeto con el Tag 'Admin'!");
        }
    }

    private void ActivarCanvasInfo()
    {
        if (canvasInfo == null) return;

        string nombreEscena = SceneManager.GetActiveScene().name;

        if (nombreEscena == "Tutorial")
            canvasInfo.SetActive(true);
        else
            canvasInfo.SetActive(false);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (canvasInfo != null)
        {
            if (scene.name == "Tutorial")
                canvasInfo.SetActive(true);
            else
                canvasInfo.SetActive(false);
        }
    }
}