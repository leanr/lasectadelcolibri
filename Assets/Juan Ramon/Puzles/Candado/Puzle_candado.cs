using UnityEngine;
using UnityEngine.InputSystem;

public class Puzle_cand : MonoBehaviour
{
    public Canvas PuzlecandCanvas;
    public static bool active = false;

    void Start()
    {
        PuzlecandCanvas.enabled = false; // Activo desde el inicio
        Time.timeScale = 1f;
    }

    void Update()
    {
        // if (Keyboard.current == null) return;

        // // N → abrir / cerrar
        // if (Keyboard.current.nKey.wasPressedThisFrame)
        // {
        //     if (abierto)
        //         Cerrar();
        //     else
        //         Abrir();
        // }
        if (active == true)
        {
            Abrir();
        }
        else
        {
            Cerrar();
        }
    }

    void Abrir()
    {
        PuzlecandCanvas.enabled = true;
        active = true;
        Time.timeScale = 1f;
    }

    void Cerrar()
    {
        PuzlecandCanvas.enabled = false;
        active = false;
        Time.timeScale = 1f;
    }
}
