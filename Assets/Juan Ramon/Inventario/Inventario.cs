using UnityEngine;
using UnityEngine.InputSystem;

public enum InventoryObject { Mask, NightVisionGoogles, Key }

public class Inventario: MonoBehaviour
{
    public Canvas inventarioCanvas;
    bool abierto = false;

    public GameObject maskReference;
    public GameObject nightVisionReference;
    public GameObject keyReference;

    [HideInInspector]
    public static Inventario instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        abierto = false;
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
        Time.timeScale = 0f;
    }

    void Cerrar()
    {
        inventarioCanvas.enabled = false;
        abierto = false;
        Time.timeScale = 1f;
    }
}
