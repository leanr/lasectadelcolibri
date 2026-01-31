using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Prueba_color : MonoBehaviour
{
    public List<Outline> celdas;   // Arrastra aquí los 12 Outline en orden

    bool modoEdicion = false;
    string inputNumero = "";
    string inputColor = "";

    void Update()
    {
        // Activar modo edición con P + B
        if (Input.GetKey(KeyCode.P) && Input.GetKeyDown(KeyCode.B))
        {
            modoEdicion = true;
            inputNumero = "";
            inputColor = "";
            Debug.Log("Modo edición activado. Escribe número de celda (1-12) y luego el color (negro, blanco, rojo). Pulsa Enter para confirmar.");
        }

        if (!modoEdicion)
            return;

        // Capturar teclas
        foreach (char c in Input.inputString)
        {
            // Números
            if (char.IsDigit(c))
            {
                inputNumero += c;
                Debug.Log("Número actual: " + inputNumero);
            }

            // Letras (para el color)
            if (char.IsLetter(c))
            {
                inputColor += c;
                Debug.Log("Color actual: " + inputColor);
            }

            // Enter → aplicar cambio
            if (c == '\n' || c == '\r')
            {
                AplicarCambio();
            }
        }
    }

    void AplicarCambio()
    {
        // Validar número
        if (!int.TryParse(inputNumero, out int numCelda))
        {
            Debug.Log("Número inválido.");
            modoEdicion = false;
            return;
        }

        if (numCelda < 1 || numCelda > celdas.Count)
        {
            Debug.Log("La celda no existe.");
            modoEdicion = false;
            return;
        }

        Outline borde = celdas[numCelda - 1];
        string color = inputColor.ToLower();

        // Cambiar color
        if (color.Contains("neg"))
            borde.effectColor = Color.black;
        else if (color.Contains("bla"))
            borde.effectColor = Color.white;
        else if (color.Contains("roj"))
            borde.effectColor = Color.red;
        else
        {
            Debug.Log("Color no reconocido. Usa negro, blanco o rojo.");
            modoEdicion = false;
            return;
        }

        Debug.Log($"Celda {numCelda} cambiada a {color}");
        modoEdicion = false;
    }
}
