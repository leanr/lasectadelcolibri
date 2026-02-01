using UnityEngine;

public class Candado : Interactuable
{
    public 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public override void Usar(PlayerController p)
    {   
        foreach (GameObject e in p.objetosRecogidos)
        {
            Debug.Log(e.name);
        }
    }
}
