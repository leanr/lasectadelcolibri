using System;
using UnityEngine;

public class GestorInteractuar : MonoBehaviour
{
    public static void GestorColision(Collider2D c, bool encender)
    {
        Debug.Log("hago trigger");
        if (c.gameObject.layer == 31)
        {
            Debug.Log("el trigger es intertactuable");
            if (c.GetComponent<PuertaInteractuable>() != null)
            {
                c.GetComponent<PuertaInteractuable>().MostrarIndicadorInteraccion(encender);
            }
            else if (c.GetComponent<Llave>() != null)
            {
                Debug.Log("el trigger es llave");
                c.GetComponent<Llave>().MostrarIndicadorInteraccion(encender);
            }
        }
    }
}
