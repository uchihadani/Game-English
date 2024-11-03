using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinJuegoManager : MonoBehaviour
{
    public AudioClip finJuegoSound;
    private AudioSource audioSource;
    public TextMeshProUGUI puntajeText;
    public TextMeshProUGUI mensajeText;
    private int cubosCorrectos;

    public string nombreEscenaJuego = "Juego"; // Nombre de la escena del juego
    public string nombreEscenaMenu = "Menu"; // Nombre de la escena del menú

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = finJuegoSound;

        if (finJuegoSound != null)
        {
            audioSource.Play();
        }

        // Cargar la cantidad correcta de cubos desde PlayerPrefs
        cubosCorrectos = PlayerPrefs.GetInt("CorrectCubes", 0); // Usar una clave consistente
        DisplayPuntajeYMensaje();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void DisplayPuntajeYMensaje()
    {
        puntajeText.text = $"You got {cubosCorrectos} correct cubes!!";

        if (cubosCorrectos == 4)
        {
            mensajeText.text = "Very good, you are very good!";
        }
        else if (cubosCorrectos == 3)
        {
            mensajeText.text = "Good, keep it up!";
        }
        else if (cubosCorrectos >= 1 && cubosCorrectos <= 2)
        {
            mensajeText.text = "Cheer up, you can improve!";
        }
        else
        {
            mensajeText.text = "You got no correct cubes.";
        }
    }

    public void VolverAlJuego()
    {
        PlayerPrefs.SetInt("CorrectCubes", 0);
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void VolverAlMenu()
    {
        PlayerPrefs.SetInt("CorrectCubes", 0);
        SceneManager.LoadScene(nombreEscenaMenu); // Cargar la escena del menú
    }

    public void SalirDelJuego()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
