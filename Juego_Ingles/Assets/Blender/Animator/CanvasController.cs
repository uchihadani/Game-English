using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public Image imageSelector;

    public void ChangePickableCubeColor(bool isSelect)
    {
        if (isSelect)
        {
            imageSelector.color = Color.green; // Cambia a verde cuando el cubo es seleccionable
        }
        else
        {
            imageSelector.color = Color.white; // Vuelve a blanco cuando no es seleccionable
        }
    }

    public void OcultarCursor(bool ocultar)
    {
        imageSelector.enabled = !ocultar; // Activa o desactiva el canvas del cursor
    }
}