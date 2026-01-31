using System;
using UnityEngine;

public class GestorInteractuar : MonoBehaviour
{
    public static void GestorColision(Collider2D c, bool encender)
    {
        if (c.gameObject.layer == 31)
        {
            if (c.GetComponent<PuertaInteractuable>() != null)
            {
                Debug.Log("Llega a la puerta");
                c.GetComponent<PuertaInteractuable>().MostrarIndicadorInteraccion(encender);
            }
            else
            {
                Debug.Log("No es puerta");
            }
        }
    }
}
