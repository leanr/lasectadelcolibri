using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickUI : MonoBehaviour, IPointerClickHandler
{
    public GameObject imageNoMask;   
    public GameObject imageMask;
    public GameObject imageNV;

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerController p = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
        if (p.isNightVisionOn)
        {
            p.ToggleNightVision();
        }

        p.ToggleMask();

        if (p.isMaskOn)
        {
            imageNoMask.SetActive(false);
            imageMask.SetActive(true);
            imageNV.SetActive(false);
        }
        else
        {
            imageNoMask.SetActive(true);
            imageMask.SetActive(false);
            imageNV.SetActive(false);
        }
    }
}

