using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraControllerPrimeraPersona : MonoBehaviour
{
    public float sensibilidadMouse = 2f;
    public Transform cuerpoPersonaje;
    public float limiteRotacionX = 90f;

    private float rotacionX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Obtener input del mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        // Rotar el cuerpo del personaje horizontalmente
        cuerpoPersonaje.Rotate(Vector3.up * mouseX);

        // Rotar la cámara verticalmente
        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -limiteRotacionX, limiteRotacionX);
        transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
    }
}
