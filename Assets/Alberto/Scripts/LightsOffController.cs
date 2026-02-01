using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;


public class LightsOffController : MonoBehaviour
{
    public Light2D globalLight;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 0.002f, 3f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 0.4f, 3f);
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
