using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [Header("Referencias Directas")]
    [SerializeField] private GameObject objetoMenuPausa;
    [SerializeField] private GameObject objetoJugador; 

    private bool estaPausado = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (estaPausado) Reanudar();
            else Pausar();
        }
    }

    public void Pausar()
    {
        estaPausado = true;
        if (objetoMenuPausa != null) objetoMenuPausa.SetActive(true);
        
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        SetExisteMenu(true);
    }

    public void Reanudar()
    {
        estaPausado = false;
        if (objetoMenuPausa != null) objetoMenuPausa.SetActive(false);
        
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        SetExisteMenu(false);
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;                          
        Cursor.lockState = CursorLockMode.None;         
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MenuPrincipal");
    }

    private void SetExisteMenu(bool estado)
    {
        if (objetoJugador != null && objetoJugador.TryGetComponent<Movimientojugador>(out Movimientojugador script))
        {
            script.SetExisteMenu(estado);
        }
    }
}