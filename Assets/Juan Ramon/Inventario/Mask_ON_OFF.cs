using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickUI : MonoBehaviour, IPointerClickHandler
{
    public Image imagenA;   // Se enciende cuando MaskOn == 0
    public Image imagenB;   // Se enciende cuando MaskOn == 1

    private int MaskOn = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (MaskOn == 0)
        {
            // Estado 0 → A encendida, B apagada
            imagenA.enabled = true;
            imagenB.enabled = false;
            MaskOn = 1;
        }
        else
        {
            // Estado 1 → B encendida, A apagada
            imagenA.enabled = false;
            imagenB.enabled = true;
            MaskOn = 0;
        }
    }
}
