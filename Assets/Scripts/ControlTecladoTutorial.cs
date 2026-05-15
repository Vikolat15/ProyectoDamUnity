using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlTecladoTutorial : MonoBehaviour
{
    [SerializeField] private GameObject objetoGestorVictoria; 
    
    void Start()
    {
        if (objetoGestorVictoria == null)
        {
            Debug.LogWarning("ControlTecladoTutorial: No se ha asignado el objeto del GestorVictoria.");
        }
    }
    
    void Update()
    {
        if (objetoGestorVictoria != null && objetoGestorVictoria.TryGetComponent<GestorVictoria>(out GestorVictoria script))
        {
            if (script.pantallaVictoria != null && script.pantallaVictoria.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    ReiniciarNivel(script);
                }
                
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    IrAlMenu();
                }
            }
        }
    }
    
    void ReiniciarNivel(GestorVictoria script)
    {
        ResetearTiempo();
        
        if (script != null)
        {
            script.ResetearTiempo();
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    void IrAlMenu()
    {
        ResetearTiempo();
        SceneManager.LoadScene(0);
    }
    
    void ResetearTiempo()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}