using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class HoverInventarioUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image imagenObjeto;            // arrastra aquí "mascara"
    public GameObject tooltip;            // arrastra aquí "Tooltip" panel
    public TextMeshProUGUI tooltipText;   // arrastra aquí "descripcion Mascara"

    [TextArea]
    public string descripcion;            // escribe la descripción de la máscara aquí

    Color colorOriginal;

    void Start()
    {
        colorOriginal = imagenObjeto.color;  // guardamos color original
        tooltip.SetActive(false);            // tooltip empieza oculto
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Oscurecer la imagen
        imagenObjeto.color = colorOriginal * 0.7f;

        // Mostrar tooltip con descripción
        tooltip.SetActive(true);
        tooltipText.text = descripcion;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Restaurar color
        imagenObjeto.color = colorOriginal;

        // Ocultar tooltip
        tooltip.SetActive(false);
    }
}
