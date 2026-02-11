using DG.Tweening;
using System.Collections;
using UnityEngine;

public class HidingObject: Interactuable
{
    private Vector3 playerOriginalPosition;
    private bool allowStopHiding;
    public float hidingSpriteTime = 0.5f;
    public float stopHidingSpriteTime = 0.5f;
    public bool isBackground = true;
    public Vector3 localHidingPosition = Vector3.zero;
    private Vector3 hidingPosition;

    public override void Usar(PlayerController p)
    {
        // si no se está escondiendo, se guarda la posición, se agacha, avanza un poquito hacia el centro del transform y se pone el sortingorder a -1 se bloquean los inputs excepto F.
        if (!p.isHiding)
        {
            playerOriginalPosition = new Vector3(p.transform.position.x, p.transform.position.y, p.transform.position.z);
            p.isRunning = false;
            p.isCrouching = true;
            p.isHiding = true;
            p.UpdateAnimatorState();

            // force the animation change
            if (p.isMaskOn)
            {
                p.playerAnimator.Play("Base Layer.CrouchMascara");
            }
            else if (p.isNightVisionOn)
            {
                p.playerAnimator.Play("Base Layer.CrouchVisionNocturna");
            }
            else
            {
                p.playerAnimator.Play("Base Layer.CrouchNormal");
            }

            StartCoroutine(Hide(p));
        }
        else if (p.isHiding && allowStopHiding)
        {
            StartCoroutine(StopHiding(p));
        }
        // si se está escondiendo, se cambia el sorting order, se devuelve a la posición desde donde entró, se pone en idle y se devuelve el control de los inputs
    }

    public IEnumerator Hide(PlayerController p)
    {
        transform.parent.GetComponent<BoxCollider2D>().enabled = false;
        Tween tw = p.transform.DOMove(hidingPosition, 3f);
        yield return new WaitForSeconds(hidingSpriteTime);

        if (isBackground)
        {
            p.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            p.GetComponent<SpriteRenderer>().sortingOrder = 3;
        }

        yield return tw.WaitForCompletion();

        if (!isBackground)
        {
            p.playerAnimator.speed = 0f;
        }
        allowStopHiding = true;
    }

    public IEnumerator StopHiding(PlayerController p)
    {
        allowStopHiding = false;
        p.playerAnimator.speed = 1f;
        Tween tw = p.transform.DOMove(playerOriginalPosition, 3f);
        yield return new WaitForSeconds(stopHidingSpriteTime);
        if (isBackground)
        {
            p.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            p.GetComponent<SpriteRenderer>().sortingOrder = 5;
        }
        yield return tw.WaitForCompletion();
        transform.parent.GetComponent<BoxCollider2D>().enabled = true;        
        p.isCrouching = false;
        p.isHiding = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allowStopHiding = false;

        if (isBackground)
        {
            hidingPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            hidingPosition = transform.TransformPoint(localHidingPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
