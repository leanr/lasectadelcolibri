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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.currentContaminationDurationInMinutes = playerController.defaultContaminationDurationInMinutes;
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
