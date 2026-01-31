using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImagenCambioSpriteQuit : MonoBehaviour, IPointerClickHandler
{
    public Image imagenquit;                 // La imagen UI
    public Sprite spriteNormalquit;          // Primer sprite
    public Sprite spriteAlternativoquit;     // Segundo sprite

    private Vector3 escalaNormal = Vector3.one;       
    private Vector3 escalaAlternativa = new Vector3(1.3f, 1.3f, 1f);

    bool cambiado = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!cambiado)
        {
            imagenquit.sprite = spriteAlternativoquit;
            imagenquit.rectTransform.localScale = escalaAlternativa;
            cambiado = true;
        }
        else
        {
            imagenquit.sprite = spriteNormalquit;
            imagenquit.rectTransform.localScale = escalaNormal;
            cambiado = false;
        }
    }
}
