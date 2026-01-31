using UnityEngine;
using UnityEngine.InputSystem; // 👈 IMPORTANTE

public class InventarioToggle : MonoBehaviour
{
    public Canvas inventarioCanvas;

    bool abierto = false;

    void Start()
    {
        inventarioCanvas.enabled = false;
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

        // ESC → cerrar
        if (abierto && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cerrar();
        }
    }

    void Abrir()
    {
        inventarioCanvas.enabled = true;
        abierto = true;
        Time.timeScale = 0f;

        Debug.Log("Inventario ABIERTO");
    }

    void Cerrar()
    {
        inventarioCanvas.enabled = false;
        abierto = false;
        Time.timeScale = 1f;

        Debug.Log("Inventario CERRADO");
    }
}
