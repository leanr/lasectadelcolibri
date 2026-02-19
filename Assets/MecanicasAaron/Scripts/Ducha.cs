using UnityEngine;

public class Ducha : Interactuable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void Usar(PlayerController p)
    {
        GetComponent<BoxCollider2D>().enabled = false;
        p.currentContaminationLevel = p.maxContaminationLevel;
        p.ShowFloatingText("I feel cleaner, I can breathe much better...");
    } 
}
