using UnityEngine;

public class MapToggle : MonoBehaviour
{
    public GameObject mapa;   // Panel del mapa en el Canvas
    private bool abierto = false;

    void Start()
    {
        // El mapa empieza oculto
        mapa.SetActive(false);
    }

    void Update()
    {
        // Abrir/cerrar con M
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMapa();
        }

        // Cerrar con Escape o con Tab
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab)) && abierto)
        {
            CerrarMapa();
        }
    }

    void ToggleMapa()
    {
        abierto = !abierto;
        mapa.SetActive(abierto);
    }

    void CerrarMapa()
    {
        abierto = false;
        mapa.SetActive(false);
    }
}
