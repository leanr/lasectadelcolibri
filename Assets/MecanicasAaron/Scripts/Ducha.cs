using UnityEngine;

public class Ducha : Interactuable
{
    public bool usable = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void Usar(PlayerController p)
    {
        p.currentContaminationLevel = 100;
        usable = false;
    } 
}
