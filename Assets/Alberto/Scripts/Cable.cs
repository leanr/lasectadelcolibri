using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Cable : Interactuable
{
    public bool isElectrifying;
    public Transform cableSpriteTransform;
    public bool cableHit;
    public FinalBossController finalBoss;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cableHit = false;
    }

    public IEnumerator MoveRotateCable(float animationTime, float timeInPosition)
    {
        Vector2 originalPosition = new Vector2 (cableSpriteTransform.transform.position.x, cableSpriteTransform.transform.position.y);
        Vector3 originalRotation = new Vector3(cableSpriteTransform.transform.rotation.eulerAngles.x, cableSpriteTransform.transform.rotation.eulerAngles.y, 
            cableSpriteTransform.transform.rotation.eulerAngles.z);
        cableSpriteTransform.transform.DOLocalMove(new Vector2(0.5f, 6.75f), animationTime);
        yield return cableSpriteTransform.transform.DOLocalRotate(new Vector3(0f, 0f, 36f), animationTime).WaitForCompletion();
        isElectrifying = true;
        yield return new WaitForSeconds(timeInPosition);
        isElectrifying = false;
        cableHit = false;
        cableSpriteTransform.transform.DOMove(originalPosition, animationTime);
        cableSpriteTransform.transform.DORotate(originalRotation, animationTime);
    }

    public override void Usar(PlayerController p)
    {
        StartCoroutine(MoveRotateCable(1f, 2f));
    }

    private void Update()
    {
        if (isElectrifying)
        {
            if (!cableHit && finalBoss.canHit)
            {
                cableHit = true;
                finalBoss.TakeDamage(25f);
            }
        }
    }
}
