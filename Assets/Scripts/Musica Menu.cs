using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicaMenu : MonoBehaviour
{
    void Start()
    {
        SceneManager.sceneLoaded += AlCargarEscena;
    }

    void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        if (escena.name != "Menu")
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= AlCargarEscena;
    }
}
