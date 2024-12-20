using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonajeController : MonoBehaviour
{
    public float velocidadMovimiento = 5f;
    public float velocidadCorrer = 10f;
    public float fuerzaSalto = 5f;
    public Transform camaraPrimeraPersona;
    private Vector3 movimiento;
    private Animator animator;
    private Rigidbody rb;
    private bool enSuelo = true;

    // Apartado para la m�sica de fondo
    public AudioClip musicaFondo; // Clip de audio para la m�sica de fondo
    private AudioSource audioSource; // Componente AudioSource para reproducir la m�sica

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        if (camaraPrimeraPersona == null)
        {
            camaraPrimeraPersona = Camera.main.transform;
        }

        // Configuraci�n del AudioSource para la m�sica de fondo
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicaFondo;
        audioSource.loop = true; // Hacer que la m�sica se reproduzca en bucle
        audioSource.playOnAwake = true; // Reproducir autom�ticamente al iniciar
        audioSource.volume = 0.5f; // Ajustar el volumen (puedes cambiarlo si es necesario)
        audioSource.Play(); // Reproducir la m�sica de fondo
    }

    void Update()
    {
        // Capturar input
        float movimientoHorizontal = Input.GetAxisRaw("Horizontal");
        float movimientoVertical = Input.GetAxisRaw("Vertical");

        // Calcular vector de movimiento relativo a la c�mara
        Vector3 forward = camaraPrimeraPersona.forward;
        Vector3 right = camaraPrimeraPersona.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        movimiento = (forward * movimientoVertical + right * movimientoHorizontal).normalized;

        // Detectar si est� corriendo
        bool estaCorriendo = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Actualizar animaciones
        float velocidad = movimiento.magnitude;
        animator.SetFloat("Velocidad", velocidad);
        animator.SetBool("Corriendo", estaCorriendo);

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            Saltar();
        }
    }

    void FixedUpdate()
    {
        // Aplicar movimiento
        float velocidadActual = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? velocidadCorrer : velocidadMovimiento;
        rb.MovePosition(rb.position + movimiento * velocidadActual * Time.fixedDeltaTime);
    }

    void Saltar()
    {
        rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        enSuelo = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }
    }
}
