using System;
using UnityEngine;

public class PuertaInteractuable : MonoBehaviour
{
    public void MostrarIndicadorInteraccion(bool encender)
    {
        if (this.GetComponentInChildren<IndicadorInteracciones>() != null)
        {
            this.GetComponentInChildren<IndicadorInteracciones>().ToggleVisibilidad(encender);
        }
    }
}
