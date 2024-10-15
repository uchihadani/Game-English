using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{

    public GameObject ObjectToPickUp;
    public GameObject PickedObject;
    public Transform interactionZone;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (ObjectToPickUp != null && ObjectToPickUp.GetComponent<PickableObject>().isPickabe == true && PickedObject == null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                PickedObject = ObjectToPickUp;
                PickedObject.GetComponent<PickableObject>().isPickabe = false;
                PickedObject.transform.SetParent(interactionZone);
                PickedObject.transform.localPosition = Vector3.zero; // Coloca el objeto en el centro de la zona de interacción
                PickedObject.transform.localRotation = Quaternion.identity; // Resetea la rotación del objeto
                PickedObject.GetComponent<Rigidbody>().useGravity = false;
                PickedObject.GetComponent<Rigidbody>().isKinematic = true;
                animator.SetBool("Agarrar_mantenido", true);
            }
        }
        else if (PickedObject != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                PickedObject.GetComponent<PickableObject>().isPickabe = true;
                PickedObject.transform.SetParent(null);
                PickedObject.GetComponent<Rigidbody>().useGravity = true;
                PickedObject.GetComponent<Rigidbody>().isKinematic = false;
                PickedObject = null;
                animator.SetBool("Agarrar_mantenido", false);
                animator.SetBool("Caminar_agarrado", false);
            }
            else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                animator.SetBool("Caminar_agarrado", true);
                animator.SetBool("Agarrar_mantenido", false);
            }
            else
            {
                animator.SetBool("Caminar_agarrado", false);
                animator.SetBool("Agarrar_mantenido", true);
            }
        }
    }
}
