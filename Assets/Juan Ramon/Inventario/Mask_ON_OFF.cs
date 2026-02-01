using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickUI : MonoBehaviour, IPointerClickHandler
{
    public GameObject imagenA;   // Se enciende cuando currentMask == 0
    public GameObject imagenB;   // Se enciende cuando currentMask == 1
    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerController.instance.ToggleMask();

        if (PlayerController.instance.isMaskOn)
        {
            imagenA.SetActive(true);
            imagenB.SetActive(false);
        }
        else
        {
            imagenA.SetActive(false);
            imagenB.SetActive(true);
        }
        //    int mask = PlayerController.currentMask;

        //print(mask);
        //if (mask == 0)// Ponerse Mascara
        //{
        //    imagenA.SetActive(true);
        //    imagenB.SetActive(false);
        //    PlayerController.currentMask = 1;
        //}
        //else if(mask == 1)// Quitar mascara
        //{
        //    imagenA.SetActive(false);
        //    imagenB.SetActive(true);
        //    PlayerController.currentMask = 0;
        //}
        //else if (mask == 2)// Cambiar mascara 
        //{
        //    imagenA.SetActive(true);
        //    imagenB.SetActive(false);
        //    PlayerController.currentMask = 1;
        //}
    }
}

