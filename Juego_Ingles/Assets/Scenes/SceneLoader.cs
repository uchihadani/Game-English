using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Método para cargar la escena del juego
    public void LoadGameScene()
    {
        SceneManager.LoadScene("SampleScene"); // Cambia "Juego" por el nombre exacto de tu escena
    }
    public AudioClip menuMusic; // El clip de audio que quieres reproducir
    private AudioSource audioSource;
    public string gameSceneName = "Juego"; // Nombre de la escena del juego

    private void Start()
    {
        // Configurar el AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = menuMusic;
        audioSource.loop = false; // No repetir automáticamente
        audioSource.playOnAwake = true; // Reproducir al iniciar
        audioSource.Play();
    }

    private void Update()
    {
        // Verificar si la música ha terminado y volver a reproducirla
        if (!audioSource.isPlaying && !audioSource.mute)
        {
            audioSource.Play();
        }
    }

    public void StartGame()
    {
        // Silenciar la música
        audioSource.mute = true;
        // Cargar la escena del juego
        SceneManager.LoadScene(gameSceneName);
    }
}
