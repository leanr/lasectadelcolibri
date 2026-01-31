using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImagenCambioSprite : MonoBehaviour, IPointerClickHandler
{
    public Image imagen;                 // La imagen UI
    public Sprite spriteNormal;          // Primer sprite
    public Sprite spriteAlternativo;     // Segundo sprite

    private Vector3 escalaNormal = Vector3.one;       // Escala del sprite normal
    private Vector3 escalaAlternativa = new Vector3(1.3f, 1.3f, 1f); // Escala del segundo sprite

    bool cambiado = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!cambiado)
        {
            imagen.sprite = spriteAlternativo;
            imagen.rectTransform.localScale = escalaAlternativa;
            cambiado = true;
        }
        else
        {
            imagen.sprite = spriteNormal;
            imagen.rectTransform.localScale = escalaNormal;
            cambiado = false;
        }
    }
}
