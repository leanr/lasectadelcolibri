using UnityEngine;
using UnityEngine.InputSystem;


public class Puzle_inz : MonoBehaviour
{
    public Canvas PuzleinzCanvas;
    public static bool active = false;

    void Start()
    {
        PuzleinzCanvas.enabled = false; // Activo desde el inicio
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // Z → abrir / cerrar
        // if (Keyboard.current.zKey.wasPressedThisFrame)
        // {
        //     if (abierto)
        //         Cerrar();
        //     else
        //         Abrir();
        // }

        if (active)
        {
            Abrir();
        }
        else
        {
            Cerrar();
        }

    void Abrir()
    {
        PuzleinzCanvas.enabled = true;
        active = true;
        Time.timeScale = 1f;
    }

    void Cerrar()
    {
        PuzleinzCanvas.enabled = false;
        active = false;
        Time.timeScale = 1f;
    }
}
}