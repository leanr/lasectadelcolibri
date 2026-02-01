using UnityEngine;

public class CandadoVisual : MonoBehaviour
{
    public int codigoCandado = Nota.code; 

    public GameObject prefab = null;

    public int currentCode = 0000;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCode == codigoCandado)
        {
            
        }
    }


}
