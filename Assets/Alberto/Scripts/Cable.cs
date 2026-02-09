using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Cable : Interactuable
{
    public bool isElectrifying;
    public Transform cableSpriteTransform;
    public bool cableHit;
    public FinalBossController finalBoss;
    public string animatorBrokenStateString;
    public bool isCableLeft;
    private Animator cableAnimator;
    private bool isUsing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isUsing = false;
        cableHit = false;
        cableAnimator = GetComponentInChildren<Animator>();
    }

    public IEnumerator MoveRotateCableLegacy(float animationTime, float timeInPosition)
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

    public IEnumerator MoveLeftSideOfCable()
    {
        //transform.GetChild(2).transform.DOScaleY(0.3177915f, 1f);
        yield return transform.GetChild(2).transform.DOLocalMoveY(transform.GetChild(2).transform.localPosition.y + 0.15f, 1f).WaitForCompletion();
        yield return new WaitForSeconds(0.5f);
        //yield return transform.GetChild(2).transform.DOLocalMoveY(transform.GetChild(2).transform.localPosition.y - 0.2f, 0.5f).WaitForCompletion();
    }

    public IEnumerator BreakCable(float hitDelay, float timeInPosition)
    {
        isUsing = true;
        cableAnimator.SetTrigger("break");
        if (isCableLeft)
        {
            StartCoroutine(MoveLeftSideOfCable());
        }
        yield return new WaitForSeconds(hitDelay);
        isElectrifying = true;
        yield return new WaitForSeconds(timeInPosition);
        isElectrifying = false;
        isUsing = false;
    }

    public override void Usar(PlayerController p)
    {
        //StartCoroutine(MoveRotateCableLegacy(1f, 2f));
        if (!isUsing && !cableHit)
        {
            StartCoroutine(BreakCable(0.5f, 2f));
        }
    }

    private void Update()
    {
        if (isElectrifying)
        {
            if (!cableAnimator.GetCurrentAnimatorStateInfo(0).IsName(animatorBrokenStateString) && !cableHit && finalBoss.canHit)
            {
                cableHit = true;
                finalBoss.TakeDamage(34f);
            }
        }
        if (cableAnimator.GetCurrentAnimatorStateInfo(0).IsName(animatorBrokenStateString) && !cableHit && !isElectrifying)
        {
            cableAnimator.SetTrigger("restoreCable");
            if (isCableLeft)
            {
                Transform targetTransform = transform.GetChild(2).transform;
                targetTransform.localPosition = new Vector3(targetTransform.localPosition.x, targetTransform.localPosition.y - 0.2f, targetTransform.localPosition.z);
                //targetTransform.localScale = new Vector3(targetTransform.localScale.x, 0.3631182f, targetTransform.localScale.z);
            }
        }
    }
}
