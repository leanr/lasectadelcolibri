using DG.Tweening;
using UnityEngine;

public class PoisonGasController : MonoBehaviour
{
    public float gasContaminationLevelMultiplier = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.currentContaminationDurationInMinutes =
                playerController.defaultContaminationDurationInMinutes / gasContaminationLevelMultiplier;
            playerController.GetComponent<SpriteRenderer>().color = new Color32(145, 255, 119, 255);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.currentContaminationDurationInMinutes = playerController.defaultContaminationDurationInMinutes;
            playerController.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
