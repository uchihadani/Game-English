using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeController : MonoBehaviour
{
    public float velocidadMovimiento = 5f;
    public Transform camaraPrimeraPersona;

    private Vector3 movimiento;
    private Animator animator;
    private Rigidbody rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Asegurar que tenemos una referencia a la cámara
        if (camaraPrimeraPersona == null)
        {
            camaraPrimeraPersona = Camera.main.transform;
        }
    }

    void Update()
    {
        // Capturar input
        float movimientoHorizontal = Input.GetAxisRaw("Horizontal");
        float movimientoVertical = Input.GetAxisRaw("Vertical");

        // Calcular vector de movimiento relativo a la cámara
        Vector3 forward = camaraPrimeraPersona.forward;
        Vector3 right = camaraPrimeraPersona.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        movimiento = (forward * movimientoVertical + right * movimientoHorizontal).normalized;

        // Activar animaciones
        bool estaMoviendose = movimiento.magnitude > 0.1f;
        animator.SetBool("Caminar", estaMoviendose);
        animator.SetBool("Quieto", !estaMoviendose);
    }

    void FixedUpdate()
    {
        // Aplicar movimiento
        rb.MovePosition(rb.position + movimiento * velocidadMovimiento * Time.fixedDeltaTime);
    }
}