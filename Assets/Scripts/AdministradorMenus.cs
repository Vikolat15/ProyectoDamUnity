using UnityEngine;
using UnityEngine.SceneManagement;
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

    public static bool menuActivo = false;

    public int nivel = 0;

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
        ConsultarPuntuacion(0,0);
        textoPuntiacionTutorial.text = "Record : " + record.ToString();
        
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

    public void CargarTutorial()
    {
        ReproducirClick();
        SceneManager.LoadScene("Tutorial");
        
        if (canvasInfo != null)
            Invoke("ActivarCanvasInfo", 0.1f);
    }

    public void CargarNivel1()
    {
        ReproducirClick();
        SceneManager.LoadScene("Nivel1");
        
        if (canvasInfo != null)
            canvasInfo.SetActive(false);
    }

    public void CargarNivel2()
    {
        ReproducirClick();
        SceneManager.LoadScene("Nivel2");
        
        if (canvasInfo != null)
            canvasInfo.SetActive(false);
    }

    public void CargarNivel3()
    {
        ReproducirClick();
        SceneManager.LoadScene("Nivel3");
        
        if (canvasInfo != null)
            canvasInfo.SetActive(false);
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
        {
            canvasInfo.SetActive(true);
        }
        else
        {
            canvasInfo.SetActive(false);
        }
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
        string nombreEscena = scene.name;
        
        if (canvasInfo != null)
        {
            if (nombreEscena == "Tutorial")
            {
                canvasInfo.SetActive(true);
            }
            else
            {
                canvasInfo.SetActive(false);
            }
        }
    }
}