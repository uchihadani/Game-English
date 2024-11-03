using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Asegúrate de incluir esta línea
using TMPro;

public class PlayerAction : MonoBehaviour
{
    public List<GameObject> cubes; // Lista de cubos asignados en el Inspector
    public Transform cam;
    public float CubeDistance = 2f;
    public float CubeForce = 250f;
    private bool holdingCube = false;
    private Rigidbody cubeRB; // Cambié a singular ya que se usará para el cubo actual

    public CanvasController canvasScript; // Referencia al CanvasController
    public LayerMask pickableLayer;
    private RaycastHit hit;

    public int currentCubeIndex = 0; // Índice del cubo actual
    private GameObject currentCube; // Variable para el cubo actual
    private Collider currentCubeCollider; // Collider del cubo actual

    public Image aimImage; // Referencia al Image "Aim"
    public Color hoverColor = Color.green; // Color cuando el cursor está sobre un cubo
    private Color originalColor; // Color original del Image

    public TextMeshProUGUI cubeNameText; // Referencia al TextMeshPro para mostrar el nombre del cubo

    void Start()
    {
        if (cubes.Count > 0)
        {
            currentCube = cubes[currentCubeIndex]; // Asignar el primer cubo
            cubeRB = currentCube.GetComponent<Rigidbody>();
            cubeRB.useGravity = true;
            canvasScript.OcultarCursor(false);
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Guardar el color original del Image
        if (aimImage != null)
        {
            originalColor = aimImage.color;
        }

        // Inicializar el texto del nombre del cubo
        if (cubeNameText != null)
        {
            cubeNameText.gameObject.SetActive(false); // Asegúrate de que esté desactivado al inicio
        }
    }

    void Update()
    {
        if (holdingCube)
        {
            // Si el jugador está sosteniendo un cubo
            if (Input.GetMouseButtonDown(0)) // Si el jugador hace clic
            {
                // Dejar de sostener el cubo
                holdingCube = false;
                cubeRB.useGravity = true; // Activar la gravedad
                cubeRB.AddForce(cam.forward * CubeForce); // Lanzar el cubo
                canvasScript.OcultarCursor(false); // Ocultar el canvas del cursor
                cubeNameText.gameObject.SetActive(false); // Ocultar el texto del nombre al soltar
            }
        }
        else
        {
            // Si no está sosteniendo un cubo, verificar si hay uno seleccionable
            if (Physics.Raycast(cam.position, cam.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, pickableLayer))
            {
                if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Pickables")) // Comprobar si el layer es "Pickables"
                {
                    ChangeAimColor(hoverColor); // Cambiar el color del aim al color de hover
                    cubeNameText.gameObject.SetActive(true); // Mostrar el texto del nombre del cubo
                    cubeNameText.text = hit.collider.gameObject.name; // Actualizar el texto con el nombre del cubo

                    // Posicionar el texto cerca del cubo
                    Vector3 textPosition = hit.collider.transform.position + Vector3.up * 1.5f; // Ajusta la altura según sea necesario
                    cubeNameText.transform.position = textPosition;

                    if (Input.GetMouseButtonDown(0)) // Si el jugador hace clic con el botón izquierdo del ratón
                    {
                        holdingCube = true; // Sostener el cubo
                        currentCubeCollider = hit.collider; // Guardar referencia al collider del cubo
                        cubeRB = currentCubeCollider.GetComponent<Rigidbody>(); // Obtener el Rigidbody del cubo
                        cubeRB.useGravity = false; // Desactivar la gravedad
                        cubeRB.velocity = Vector3.zero; // Reiniciar la velocidad
                        cubeRB.angularVelocity = Vector3.zero; // Reiniciar la rotación
                        currentCubeCollider.transform.localRotation = Quaternion.identity; // Reiniciar la rotación local
                        cubeNameText.gameObject.SetActive(false); // Ocultar el texto del nombre al agarrar el cubo
                    }
                }
                else
                {
                    ResetAimColor(); // Restablecer el color original si no está sobre un cubo
                    cubeNameText.gameObject.SetActive(false); // Ocultar el texto si no está sobre un cubo
                }
            }
            else
            {
                ResetAimColor(); // Restablecer el color original si no hay nada en la vista
                cubeNameText.gameObject.SetActive(false); // Ocultar el texto si no hay nada en la vista
            }
        }
    }

    private void LateUpdate()
    {
        if (holdingCube && currentCubeCollider != null) // Verificar si hay un cubo seleccionado
        {
            currentCubeCollider.transform.position = cam.position + cam.forward * CubeDistance; // Posicionar el cubo frente al jugador
        }
    }

    public void ShowCursor()
    {
        canvasScript.OcultarCursor(false); // Método para mostrar el canvas del cursor
    }

    public void SetCurrentCube(GameObject newCube)
    {
        currentCube = newCube; // Cambiar el cubo actual
        if (currentCube != null)
        {
            cubeRB = currentCube.GetComponent<Rigidbody>(); // Asignar el Rigidbody del nuevo cubo
        }
    }

    private void ChangeAimColor(Color newColor)
    {
        if (aimImage != null)
        {
            aimImage.color = newColor; // Cambiar el color del aim
        }
    }

    private void ResetAimColor()
    {
        if (aimImage != null)
        {
            aimImage.color = originalColor; // Restablecer al color original
        }
    }
}