using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuConfiguracion : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Sliders")]
    public Slider sliderMusica;
    public Slider sliderEfectos;

    [Header("Pantalla completa")]
    public Button botonPantallaCompleta;
    public TMPro.TextMeshProUGUI textoPantallaCompleta; 

    private void Start()
    {
        float musica = PlayerPrefs.GetFloat("VolumenMusica", 0.75f);
        float efectos = PlayerPrefs.GetFloat("VolumenEfectos", 0.75f);

        sliderMusica.value = musica;
        sliderEfectos.value = efectos;

        AplicarVolumenMusica(musica);
        AplicarVolumenEfectos(efectos);

        ActualizarTextoPantalla();

        sliderMusica.onValueChanged.AddListener(AplicarVolumenMusica);
        sliderEfectos.onValueChanged.AddListener(AplicarVolumenEfectos);
    }

    public void AplicarVolumenMusica(float valor)
    {
        float dB = valor > 0.001f ? Mathf.Log10(valor) * 20f : -80f;
        audioMixer.SetFloat("VolumenMusica", dB);
        PlayerPrefs.SetFloat("VolumenMusica", valor);
    }

    public void AplicarVolumenEfectos(float valor)
    {
        float dB = valor > 0.001f ? Mathf.Log10(valor) * 20f : -80f;
        audioMixer.SetFloat("VolumenEfectos", dB);
        PlayerPrefs.SetFloat("VolumenEfectos", valor);
    }

    public void TogglePantallaCompleta()
    {
        if (Screen.fullScreen)
            Screen.SetResolution(1280, 720, false);
        else
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);

        ActualizarTextoPantalla();
    }

    private void ActualizarTextoPantalla()
    {
        if (textoPantallaCompleta != null)
            textoPantallaCompleta.text = Screen.fullScreen ? "Pantalla completa: ON" : "Pantalla completa: OFF";
    }
}