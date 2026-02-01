using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NightVOnOff : MonoBehaviour, IPointerClickHandler
{
    public GameObject imagenA;   // Se enciende cuando currentMask == 0
    public GameObject imagenB;   // Se enciende cuando currentMask == 2
    public GameObject imagenC;   // Se enciende cuando currentMask == 1

    private void Start()
    {
        // Ambas ocultas al inicio
        imagenA.SetActive(false);
        imagenB.SetActive(false);
        imagenB.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int mask = PlayerController.currentMask;
        print(mask);

        if (mask == 0) // Activar Night Vision
        {
            imagenA.SetActive(true);
            imagenB.SetActive(false);
            PlayerController.currentMask = 2;
        }
        else if (mask == 2) // Desactivar Night Vision
        {
            imagenA.SetActive(false);
            imagenB.SetActive(true);
            PlayerController.currentMask = 0;
        }
        else if (mask == 1) // Cambiar Night Vision (si lo usas)
        {
            imagenA.SetActive(true);
            imagenB.SetActive(false);
            imagenC.SetActive(false);
            PlayerController.currentMask = 2;
        }
    }
}
