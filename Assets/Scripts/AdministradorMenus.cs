using UnityEngine;
using UnityEngine.SceneManagement;

public class AdministradorMenus : MonoBehaviour
{
    [Header("Paneles del Menú")]
    public GameObject panelMenuPrincipal;
    public GameObject panelSeleccionNiveles;
    public GameObject panelCreditos;

    [Header("Audio")]
    public AudioSource audioSource;   // Para el sonido de los botones
    public AudioClip sonidoClick;

    public AudioSource musicaMenu;    // Música del menú

    void Start()
    {
        MostrarMenuPrincipal();

        // Reproducir música del menú
        if (musicaMenu != null)
        {
            musicaMenu.loop = true;
            musicaMenu.Play();
        }
    }

    void ReproducirClick()
    {
        if (audioSource != null && sonidoClick != null)
            audioSource.PlayOneShot(sonidoClick);
    }

    // ---------- MENÚ PRINCIPAL ----------
    public void MostrarMenuPrincipal()
    {
        panelMenuPrincipal.SetActive(true);
        panelSeleccionNiveles.SetActive(false);
        panelCreditos.SetActive(false);
    }

    public void AlPresionarJugar()
    {
        ReproducirClick();
        panelMenuPrincipal.SetActive(false);
        panelSeleccionNiveles.SetActive(true);
    }

    public void AlPresionarCreditos()
    {
        ReproducirClick();
        panelMenuPrincipal.SetActive(false);
        panelCreditos.SetActive(true);
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

    // ---------- SELECCIÓN DE NIVELES ----------
    public void AlPresionarVolverNiveles()
    {
        ReproducirClick();
        MostrarMenuPrincipal();
    }

    public void CargarTutorial()
    {
        ReproducirClick();
        SceneManager.LoadScene("Tutorial");
    }

    public void CargarNivel1()
    {
        ReproducirClick();
        SceneManager.LoadScene("Nivel1");
    }

    public void CargarNivel2()
    {
        ReproducirClick();
        SceneManager.LoadScene("Nivel2");
    }

    public void CargarNivel3()
    {
        ReproducirClick();
        SceneManager.LoadScene("Nivel3");
    }

    public void CargarNivel4()
    {
        ReproducirClick();
        SceneManager.LoadScene("Nivel4");
    }

    public void AlPresionarVolverCreditos()
    {
        ReproducirClick();
        MostrarMenuPrincipal();
    }
}
