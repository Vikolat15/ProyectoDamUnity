using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdministradorMenus : MonoBehaviour
{
    [Header("Paneles del Menú")]
    public GameObject panelMenuPrincipal;
    public GameObject panelSeleccionNiveles;
    public GameObject panelCreditos;

    void Start()
    {
        MostrarMenuPrincipal();
    }

    // Menu principal
    
    public void MostrarMenuPrincipal()
    {
        panelMenuPrincipal.SetActive(true);
        panelSeleccionNiveles.SetActive(false);
        panelCreditos.SetActive(false);
    }

    public void AlPresionarJugar()
    {
        panelMenuPrincipal.SetActive(false);
        panelSeleccionNiveles.SetActive(true);
    }

    public void AlPresionarCreditos()
    {
        panelMenuPrincipal.SetActive(false);
        panelCreditos.SetActive(true);
    }

    public void AlPresionarSalir()
    {
        Debug.Log("Saliendo del juego...");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    //Seleccionar nivel    
    public void AlPresionarVolverNiveles()
    {
        MostrarMenuPrincipal();
    }

    //cargar cada nivel
    public void CargarTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void CargarNivel1()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void CargarNivel2()
    {
        SceneManager.LoadScene("Nivel2");
    }

    public void CargarNivel3()
    {
        SceneManager.LoadScene("Nivel3");
    }

    public void CargarNivel4()
    {
        SceneManager.LoadScene("Nivel4");
    }

    
    public void AlPresionarVolverCreditos()
    {
        MostrarMenuPrincipal();
    }
}