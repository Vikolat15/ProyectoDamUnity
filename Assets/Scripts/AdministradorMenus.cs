using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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

    public static AdministradorMenus Instance;
    public int nivel = 0;
    public int record = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        MostrarMenuPrincipal();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MenuPrincipal")
        {
            // Volvemos al menú: mostrarlo y resetear estado
            Time.timeScale = 1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            gameObject.SetActive(true);
            MostrarMenuPrincipal();
        }
        else
        {
            // Entramos a un nivel: ocultar el menú
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if ((panelSeleccionNiveles != null && panelSeleccionNiveles.activeSelf) ||
                (panelCreditos != null && panelCreditos.activeSelf))
                MostrarMenuPrincipal();
        }
    }

    void ReproducirClick()
    {
        if (audioSource != null && sonidoClick != null)
            audioSource.PlayOneShot(sonidoClick);
    }

    public void MostrarMenuPrincipal()
    {
        if (panelMenuPrincipal != null)    panelMenuPrincipal.SetActive(true);
        if (panelSeleccionNiveles != null) panelSeleccionNiveles.SetActive(false);
        if (panelCreditos != null)         panelCreditos.SetActive(false);
        if (canvasInfo != null)            canvasInfo.SetActive(false);
    }

    public void AlPresionarJugar()
    {
        ReproducirClick();
        if (panelMenuPrincipal != null)    panelMenuPrincipal.SetActive(false);
        if (panelSeleccionNiveles != null) panelSeleccionNiveles.SetActive(true);
        ConsultarPuntuacion(0, 0);
        if (textoPuntiacionTutorial != null)
            textoPuntiacionTutorial.text = "Record : " + record.ToString();
        ActualizarBotonesNiveles();
        if (canvasInfo != null) canvasInfo.SetActive(false);
    }

    private void ActualizarBotonesNiveles()
    {
        DatabaseManager db = DatabaseManager.Instance;
        if (db == null) return;
        if (botonNivel1 != null) botonNivel1.interactable = db.IsNivelDesbloqueado(1);
        if (botonNivel2 != null) botonNivel2.interactable = db.IsNivelDesbloqueado(2);
        if (botonNivel3 != null) botonNivel3.interactable = db.IsNivelDesbloqueado(3);
    }

    public void CargarTutorial()
    {
        ReproducirClick();
        gameObject.SetActive(false);
        SceneManager.LoadScene("Tutorial");
    }

    public void CargarNivel1()
    {
        DatabaseManager db = DatabaseManager.Instance;
        if (db != null && !db.IsNivelDesbloqueado(1)) { Debug.Log("Nivel 1 bloqueado."); return; }
        ReproducirClick();
        gameObject.SetActive(false);
        SceneManager.LoadScene("Nivel1");
    }

    public void CargarNivel2()
    {
        DatabaseManager db = DatabaseManager.Instance;
        if (db != null && !db.IsNivelDesbloqueado(2)) { Debug.Log("Nivel 2 bloqueado."); return; }
        ReproducirClick();
        gameObject.SetActive(false);
        SceneManager.LoadScene("Nivel2");
    }

    public void CargarNivel3()
    {
        DatabaseManager db = DatabaseManager.Instance;
        if (db != null && !db.IsNivelDesbloqueado(3)) { Debug.Log("Nivel 3 bloqueado."); return; }
        ReproducirClick();
        gameObject.SetActive(false);
        SceneManager.LoadScene("Nivel3");
    }

    public void AlPresionarCreditos()
    {
        ReproducirClick();
        if (panelMenuPrincipal != null) panelMenuPrincipal.SetActive(false);
        if (panelCreditos != null)      panelCreditos.SetActive(true);
        if (canvasInfo != null)         canvasInfo.SetActive(false);
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

    public void AlPresionarVolverNiveles()  { ReproducirClick(); MostrarMenuPrincipal(); }
    public void AlPresionarVolverCreditos() { ReproducirClick(); MostrarMenuPrincipal(); }

    public void ConsultarPuntuacion(int id, int idNivel)
    {
        DatabaseManager db = DatabaseManager.Instance;
        if (db != null) record = db.GetPuntuacionNivel(id, idNivel);
    }
}