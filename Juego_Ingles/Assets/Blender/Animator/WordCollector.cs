using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class WordCollector : MonoBehaviour
{
    public Transform wordHolder;
    public TextMeshProUGUI[] spreadsheetCells;
    public GameObject questionPanel;
    public TMP_Text questionText;
    public Button yesButton, noButton;
    public Image questionImage;
    public TMP_InputField inputField;

    private string currentWord;
    private GameObject currentWordObject;

    public Dictionary<string, string> wordDefinitionPairs = new Dictionary<string, string>()
    {
        { "Search-Engine", "System for finding information on the web" },
        { "Network", "Type of network that connects multiple networks" },
        { "Cloud", "Place where files are stored online" },
        { "Bookmarks", "Element to save favorite web addresses" },
        { "Browser", "Platform for viewing web pages" },
        { "Server", "Service that stores files on the Internet" },
        { "Websites", "Collection of web pages accessible via the Internet" },
        { "Hyperlinks", "Links that lead to other web pages" },
        { "Domain", "Domain name of a web page" },
        { "Storage", "Unit of storage in the cloud" },
    };

    private void Start()
    {
        questionPanel.SetActive(false);
        questionImage.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);

        yesButton.onClick.AddListener(OnYesButtonClick);
        noButton.onClick.AddListener(OnNoButtonClick);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colisión detectada con: " + other.gameObject.name);

        if (other.gameObject.layer == LayerMask.NameToLayer("WordCell"))
        {
            TextMeshProUGUI textMeshPro = other.GetComponent<TextMeshProUGUI>();

            if (textMeshPro != null)
            {
                currentWord = textMeshPro.text;
                currentWordObject = other.gameObject;

                if (wordDefinitionPairs.ContainsKey(currentWord))
                {
                    questionText.text = wordDefinitionPairs[currentWord];
                    questionPanel.SetActive(true);
                    questionImage.gameObject.SetActive(true);
                    inputField.gameObject.SetActive(true);
                    questionText.text = "¿Estás seguro de que '" + currentWord + "' es la palabra correcta para esta definición?\n\n" + wordDefinitionPairs[currentWord];
                }
                else
                {
                    Debug.Log("La palabra no tiene una definición asociada.");
                }
            }
            else
            {
                Debug.LogError("El objeto no tiene un componente TextMeshProUGUI adjunto.");
            }
        }
    }

    void OnYesButtonClick()
    {
        if (CheckCorrectWord(currentWord))
        {
            AddWordToSpreadsheet(currentWord);
        }
        else
        {
            ReleaseWord(currentWordObject);
        }

        questionPanel.SetActive(false);
        questionImage.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
    }

    void OnNoButtonClick()
    {
        ReleaseWord(currentWordObject);

        questionPanel.SetActive(false);
        questionImage.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
    }

    bool CheckCorrectWord(string word)
    {
        return wordDefinitionPairs.ContainsKey(word);
    }

    void AddWordToSpreadsheet(string word)
    {
        for (int i = 0; i < spreadsheetCells.Length; i++)
        {
            if (spreadsheetCells[i].text == "Vacío")
            {
                spreadsheetCells[i].text = word;
                Debug.Log("Palabra añadida: " + word);

                // Aquí se eliminaba el objeto, ahora solo lo movemos o lo dejamos en su lugar
                // Por ejemplo, podemos desactivar el objeto si ya no es necesario mostrarlo
                currentWordObject.SetActive(false); // Desactivamos el objeto en vez de destruirlo
                break;
            }
        }
    }

    // Soltar la palabra sin destruirla
    void ReleaseWord(GameObject wordObject)
    {
        Debug.Log("Palabra incorrecta, pero no se destruye el objeto.");
        // Si quieres devolver el objeto a su posición original, puedes añadir un comportamiento aquí
        // Por ejemplo: wordObject.transform.position = new Vector3(0, 0, 0);
        // O simplemente no haces nada si no quieres que se mueva.
    }
}