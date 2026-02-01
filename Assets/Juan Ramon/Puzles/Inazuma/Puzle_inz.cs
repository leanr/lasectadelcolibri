using UnityEngine;
using UnityEngine.InputSystem;

public class Puzle_inz : MonoBehaviour
{
    public Canvas PuzleinzCanvas;

    bool abierto = true; // Empieza abierto

    void Start()
    {
        PuzleinzCanvas.enabled = true; // Activo desde el inicio
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // Z → abrir / cerrar
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            if (abierto)
                Cerrar();
            else
                Abrir();
        }

        // M → cerrar si está abierto
        if (abierto && Keyboard.current.mKey.wasPressedThisFrame)
        {
            Cerrar();
        }
    }

    void Abrir()
    {
        PuzleinzCanvas.enabled = true;
        abierto = true;
        Time.timeScale = 1f;
    }

    void Cerrar()
    {
        PuzleinzCanvas.enabled = false;
        abierto = false;
        Time.timeScale = 1f;
    }
}
