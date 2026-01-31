using UnityEngine;
using UnityEngine.UI;

public class CeldaMapa : MonoBehaviour
{
    public Outline borde;

    public enum ColorBorde
    {
        Negro,
        Blanco,
        Rojo
    }

    void Awake()
    {
        // Asegura que el borde existe
        if (borde == null)
            borde = GetComponent<Outline>();
        //Inicia el borde en negro
        GetComponent<Outline>().effectColor = Color.black;
    }

    public void SetColor(ColorBorde color)
    {
        switch (color)
        {
            case ColorBorde.Negro:
                borde.effectColor = Color.black;
                break;

            case ColorBorde.Blanco:
                borde.effectColor = Color.white;
                break;

            case ColorBorde.Rojo:
                borde.effectColor = Color.red;
                break;
        }
    }
}
