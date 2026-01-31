using UnityEngine;

public class IndicadorInteracciones : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void ToggleVisibilidad(bool encender)
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = encender;
    }
}
