using System;
using UnityEngine;

public class GestorInteractuar : MonoBehaviour
{
    public static void GestorColision(Collider2D c, bool encender)
    {
        Debug.Log("hago trigger");
        Debug.Log(c.gameObject.layer);
        if (c.gameObject.layer == 31)
        {
            Debug.Log(c.gameObject.GetComponent<Interactuable>() != null);
            c.gameObject.GetComponent<Interactuable>().MostrarIndicadorInteraccion(encender);
        }
    }
}
