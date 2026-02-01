using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CambioSpriteCerrarCanvas : MonoBehaviour, IPointerClickHandler
{
    [Header("Imagen y Sprites")]
    public Image imagen;              // La imagen UI
    public Sprite spriteNormal;       // Sprite por defecto
    public Sprite spriteAlternativo;  // Sprite al hacer clic

    [Header("Canvas a cerrar")]
    public GameObject canvasCerrar;   // El canvas que se cerrará

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(CambiarYVolver());
    }

    private IEnumerator CambiarYVolver()
    {
        // Cambiar al sprite alternativo
        imagen.sprite = spriteAlternativo;

        // Esperar 100ms
        yield return new WaitForSeconds(0.05f);

        // Volver al sprite original
        imagen.sprite = spriteNormal;

        // Cerrar canvas
        if (canvasCerrar != null)
        {
            canvasCerrar.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }
}

