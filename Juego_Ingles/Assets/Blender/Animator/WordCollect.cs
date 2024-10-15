using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Importa la librer�a para TextMeshPro

public class WordCollect : MonoBehaviour
{
    public GameObject player;  // Asigna tu personaje desde el Inspector
    public GameObject[] wordCells;  // Asigna las celdas de palabras desde el Inspector
    public Image spreadsheetImage;  // Imagen del UI que simula el Spreadsheet
    public Sprite[] spreadsheetStages;  // Las diferentes im�genes del spreadsheet

    // Cambi� el tipo de Text a TextMeshProUGUI
    public TextMeshProUGUI wordUIText;  // UI Text para mostrar mensajes
    public TMP_InputField answerInputField;  // Cambi� InputField a TMP_InputField
    public Button submitButton;  // Bot�n para enviar la respuesta

    private int currentStage = 0;
    private string collectedWords = "";

    // Define las respuestas correctas aqu�
    public string[] correctAnswers = {
        "Search-Engine",  // Pregunta 1
        "Network",        // Pregunta 2
        "Cloud",          // Pregunta 3
        "Bookmarks",      // Pregunta 4
        "Browser",        // Pregunta 5
        "Server",         // Pregunta 6
        "Websites",       // Pregunta 7
        "Hyperlinks",     // Pregunta 8
        "Search-Engine",  // Pregunta 9
        "Storage"         // Pregunta 10
    };

    public string[] questions = {
        "System for finding information on the web",  // Pregunta 1
        "Type of network that connects multiple networks",  // Pregunta 2
        "Place where files are stored online",  // Pregunta 3
        "Element to save favorite web addresses",  // Pregunta 4
        "Platform for viewing web pages",  // Pregunta 5
        "Service that stores files on the Internet",  // Pregunta 6
        "Collection of web pages accessible via the Internet",  // Pregunta 7
        "Links that lead to other web pages",  // Pregunta 8
        "Engine used to search for information on the web",  // Pregunta 9
        "Unit of storage in the cloud"  // Pregunta 10
    };

    private int currentQuestionIndex = -1;  // Para rastrear la palabra que est�s preguntando

    void Update()
    {
        foreach (GameObject cell in wordCells)
        {
            float distance = Vector3.Distance(player.transform.position, cell.transform.position);

            if (distance < 3f)  // Si el jugador est� cerca de la celda
            {
                ShowMessage("Press 'F' to collect the word");

                if (Input.GetKeyDown(KeyCode.F))
                {
                    CollectWord(cell);
                }
            }
            else
            {
                HideMessage();
            }
        }
    }

    void ShowMessage(string message)
    {
        wordUIText.text = message;
        wordUIText.gameObject.SetActive(true);
    }

    void HideMessage()
    {
        wordUIText.gameObject.SetActive(false);
    }

    void CollectWord(GameObject wordCell)
    {
        string word = wordCell.GetComponent<WordCell>().word;  // Toma la palabra de la celda
        collectedWords += word + "\n";  // Acumula las palabras recogidas

        // Encuentra el �ndice de la palabra recogida
        currentQuestionIndex = System.Array.IndexOf(wordCells, wordCell);

        // Activa el campo de entrada para responder la pregunta
        answerInputField.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(true);

        // Muestra la pregunta correspondiente
        ShowMessage(questions[currentQuestionIndex]);

        // A�ade el evento del bot�n para comprobar la respuesta
        submitButton.onClick.RemoveAllListeners();  // Aseg�rate de limpiar cualquier listener previo
        submitButton.onClick.AddListener(CheckAnswer);

        Destroy(wordCell);  // Destruye la celda tras recoger la palabra
    }

    void CheckAnswer()
    {
        string playerAnswer = answerInputField.text;

        if (playerAnswer == correctAnswers[currentQuestionIndex])
        {
            // Respuesta correcta
            ShowMessage("Correct!");

            // Actualiza la imagen del spreadsheet si es la respuesta correcta
            currentStage++;
            if (currentStage < spreadsheetStages.Length)
            {
                spreadsheetImage.sprite = spreadsheetStages[currentStage];
            }
        }
        else
        {
            // Respuesta incorrecta
            ShowMessage("Incorrect!");
        }

        // Limpia el campo de entrada y oculta la UI de pregunta
        answerInputField.text = "";
        answerInputField.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
    }
}
