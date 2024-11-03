using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CubeDetector : MonoBehaviour
{
    public TextMeshProUGUI preguntasText;
    public TextMeshProUGUI holaText;
    public TextMeshProUGUI puntosText;
    public TextMeshProUGUI respuestasCanvas;
    public TextMeshProUGUI respuestasIncorrectasCanvas;
    public ParticleSystem confettiEffect;

    public AudioClip correctSound1;
    public AudioClip correctSound2;
    public AudioClip incorrectSound;
    public AudioClip superMarioSound;
    private AudioSource audioSource;

    private int cubosCorrectos = 0;
    private int cubosIncorrectos = 0;

    private string[] preguntas = { "System for finding information on the web", "Type of network that connects multiple networks", "Place where files are stored online", "Element to save favorite web addresses" };
    private string[] respuestasCorrectas = { "Search-Engine", "Network", "Cloud", "Bookmarks" };
    private string[] respuestasIncorrectas = { "Sphere", "Triangle", "4", "Green" };
    private int currentPreguntaIndex = 0;

    private void Start()
    {
        holaText.gameObject.SetActive(false);
        preguntasText.text = preguntas[currentPreguntaIndex];

        confettiEffect.Stop();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pickables"))
        {
            string answer = other.gameObject.name;
            string correctAnswer = respuestasCorrectas[currentPreguntaIndex];

            if (answer == correctAnswer)
            {
                StartCoroutine(PlayCorrectSounds());
                StartCoroutine(ShowMessage("The answer is correct", true));
                StartCoroutine(PlayConfettiEffect());
                cubosCorrectos++;
                PlayerPrefs.SetInt("CorrectCubes", cubosCorrectos); // Using consistent key
                UpdatePuntosText();
                StoreCorrectAnswerInCanvas(answer);
                Destroy(other.gameObject);
            }
            else
            {
                PlayIncorrectSound();
                StartCoroutine(ShowMessage("The answer is incorrect", false));
                cubosIncorrectos++;
                UpdatePuntosText();
                StoreIncorrectAnswerInCanvas(answer);
                Destroy(other.gameObject);
            }

            currentPreguntaIndex++;
            CheckForNextQuestion();
        }
    }

    private void UpdatePuntosText()
    {
        puntosText.text = $"Correct cubes = {cubosCorrectos}\nIncorrect cubes = {cubosIncorrectos}";
    }

    private void CheckForNextQuestion()
    {
        if (currentPreguntaIndex < preguntas.Length)
        {
            preguntasText.text = preguntas[currentPreguntaIndex];
        }
        else
        {
            preguntasText.text = "You have answered all the questions!";
            StopBackgroundMusic();
            PlaySuperMarioSound();
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("Fin_Juego");
    }

    private IEnumerator ShowMessage(string message, bool isCorrect)
    {
        holaText.text = message;
        holaText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        holaText.gameObject.SetActive(false);
    }

    private void StoreCorrectAnswerInCanvas(string answer)
    {
        respuestasCanvas.text += answer + "\n";
    }

    private void StoreIncorrectAnswerInCanvas(string answer)
    {
        respuestasIncorrectasCanvas.text += answer + "\n";
    }

    private IEnumerator PlayConfettiEffect()
    {
        confettiEffect.Play();
        yield return new WaitForSeconds(3);
        confettiEffect.Stop();
    }

    private IEnumerator PlayCorrectSounds()
    {
        audioSource.PlayOneShot(correctSound1);
        yield return new WaitForSeconds(correctSound1.length);
        audioSource.PlayOneShot(correctSound2);
    }

    private void PlayIncorrectSound()
    {
        audioSource.PlayOneShot(incorrectSound);
    }

    private void StopBackgroundMusic()
    {
        audioSource.volume = 0;
    }

    private void PlaySuperMarioSound()
    {
        AudioSource.PlayClipAtPoint(superMarioSound, Camera.main.transform.position, 1.0f);
    }
}

