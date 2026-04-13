using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicaPersistente : MonoBehaviour
{
    private static MusicaPersistente instance;

    public AudioClip musicaMenu;
    public AudioClip musicaTutorial;
    public AudioClip musicaNivel1;
    public AudioClip musicaNivel2;
    public AudioClip musicaNivel3;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()  => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void Start() => CambiarMusica(SceneManager.GetActiveScene().name);

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) => CambiarMusica(scene.name);

    void CambiarMusica(string escena)
    {
        AudioClip clip = escena switch
        {
            "MenuPrincipal" => musicaMenu,
            "Tutorial"      => musicaTutorial,
            "Nivel1"        => musicaNivel1,
            "Nivel2"        => musicaNivel2,
            "Nivel3"        => musicaNivel3,
            _               => null
        };

        if (clip == null || (audioSource.clip == clip && audioSource.isPlaying)) return;

        audioSource.clip = clip;
        audioSource.Play();
    }
}