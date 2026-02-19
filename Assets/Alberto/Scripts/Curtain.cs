using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Curtain : Interactuable
{
    private bool canUse;
    public void ToggleCurtain()
    {
        canUse = false;
        transform.DOScaleX(0.1f, 1f).SetEase(Ease.OutQuad).WaitForCompletion();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canUse = true;
    }

    public override void Usar(PlayerController p)
    {
        if (canUse)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            ToggleCurtain();
        }
    }
}
