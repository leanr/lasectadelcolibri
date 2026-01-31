using UnityEngine;
using UnityEngine.InputSystem;

public class InventarioToggle : MonoBehaviour
{
    public Canvas inventarioCanvas;

    bool abierto = false;

    void Start()
    {
        inventarioCanvas.enabled = false; // Empieza oculto
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // TAB → abrir / cerrar
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            if (abierto)
                Cerrar();
            else
                Abrir();
        }

        // ESC o M → cerrar si está abierto
        if (abierto &&
            (Keyboard.current.mKey.wasPressedThisFrame))
        {
            Cerrar();
        }
    }

    void Abrir()
    {
        inventarioCanvas.enabled = true;
        abierto = true;
        Time.timeScale = 1f;
    }

    void Cerrar()
    {
        inventarioCanvas.enabled = false;
        abierto = false;
        Time.timeScale = 1f;
    }
}
