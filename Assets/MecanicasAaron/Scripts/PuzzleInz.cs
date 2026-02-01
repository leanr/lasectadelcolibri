using UnityEngine;

public class PuzzleInz : Interactuable
{
    public 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public override void Usar(PlayerController p)
    {
        p.ShowFloatingText("Un mecanismo");
        Puzle_inz.active = !Puzle_inz.active; 
        foreach (GameObject e in p.objetosRecogidos)
        {
            Debug.Log(e.name);
        }
    }
}
