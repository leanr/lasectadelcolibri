using UnityEngine;
using UnityEngine.InputSystem;

public class Puzle_cand : MonoBehaviour
{
    public Canvas PuzlecandCanvas;

    bool abierto = false; // Empieza cerrado

    void Start()
    {
        PuzlecandCanvas.enabled = false; // Activo desde el inicio
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // N → abrir / cerrar
        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            if (abierto)
                Cerrar();
            else
                Abrir();
        }
    }

    void Abrir()
    {
        PuzlecandCanvas.enabled = true;
        abierto = true;
        Time.timeScale = 1f;
    }

    void Cerrar()
    {
        PuzlecandCanvas.enabled = false;
        abierto = false;
        Time.timeScale = 1f;
    }
}
