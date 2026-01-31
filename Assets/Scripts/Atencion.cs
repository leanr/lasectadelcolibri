using UnityEngine;

public class Atencion : MonoBehaviour
{

    public int maxAtencion = 100;
    public int currentAtencion;
    public bool reaccion = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseAtencion(int damage)
    {
        currentAtencion -= damage;
        if (currentAtencion <= 0)
            AtencionMaxima();
    }

    void AtencionMaxima()
    {

        reaccion = true;



    }

}