using System;
using UnityEngine;

public class GestorInteractuar : MonoBehaviour
{
    public static void GestorColision(Collider2D c, bool encender)
    {
        Debug.Log("hago trigger");
        if (c.gameObject.layer == 31)
        {
            c.gameObject.GetComponent<Interactuable>().MostrarIndicadorInteraccion(encender);
        }
    }
}
