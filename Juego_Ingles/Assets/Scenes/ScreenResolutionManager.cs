using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolutionManager : MonoBehaviour
{
    void Start()
    {
        // Ajusta la resolución automáticamente a la resolución nativa de la pantalla y activa el modo de pantalla completa
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
    }
}
