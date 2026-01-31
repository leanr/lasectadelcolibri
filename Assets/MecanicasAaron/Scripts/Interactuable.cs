using UnityEngine;

public class Interactuable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.gameObject.GetComponentInChildren<IndicadorInteracciones>().gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void MostrarIndicadorInteraccion(bool encender)
    {
        if (this.GetComponentInChildren<IndicadorInteracciones>() != null)
        {
            this.GetComponentInChildren<IndicadorInteracciones>().ToggleVisibilidad(encender);
        }
    }

    public virtual void Usar(PlayerControllerCopy p)
    {
        Debug.Log("usar padre, genericos");
    }
}
