using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuConfiguracion : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Sliders")]
    public Slider sliderMusica;

    [Header("Pantalla completa")]
    public Button botonPantallaCompleta;
    public TMPro.TextMeshProUGUI textoPantallaCompleta;

    private void Start()
    {
        float musica = PlayerPrefs.GetFloat("VolumenMusica", 0.75f);
        sliderMusica.value = musica;
        AplicarVolumenMusica(musica);
        ActualizarTextoPantalla();
        sliderMusica.onValueChanged.AddListener(AplicarVolumenMusica);
    }

    public void AplicarVolumenMusica(float valor)
    {
        float dB = valor > 0.001f ? Mathf.Log10(valor) * 20f : -80f;
        audioMixer.SetFloat("VolumenMusica", dB);
        PlayerPrefs.SetFloat("VolumenMusica", valor);
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