using TMPro;
using UnityEngine;

public class Numero : MonoBehaviour
{
    public static int num = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Numero.num+"";
    }

    public static void increase()
    {
        num++;
    }

    public static void decrease()
    {
        num--;
    }
}
