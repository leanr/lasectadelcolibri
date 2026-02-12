using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class NightVOnOff : MonoBehaviour, IPointerClickHandler
{
    public GameObject imageNoMask;
    public GameObject imageMask;
    public GameObject imageNV;

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerController p = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (p.isMaskOn)
        {
            p.ToggleMask();
        }

        p.ToggleNightVision();

        if (p.isNightVisionOn)
        {
            imageNoMask.SetActive(false);
            imageMask.SetActive(false);
            imageNV.SetActive(true);
        }
        else
        {
            imageNoMask.SetActive(true);
            imageMask.SetActive(false);
            imageNV.SetActive(false);
        }
    }
}
