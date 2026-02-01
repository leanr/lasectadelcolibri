using UnityEngine;

public class OpenCanvasESC : MonoBehaviour
{
    public GameObject canvasMenu;   // Asigna aquí tu Canvas

    private void Start()
    {
        // Asegura que el canvas empieza oculto
        canvasMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Alternar visibilidad
            bool estadoActual = canvasMenu.activeSelf;
            canvasMenu.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}
