using UnityEngine;

public class PuzzleCandado : Interactuable
{
    public 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public override void Usar(PlayerController p)
    {
        Puzle_cand.active = !Puzle_cand.active; 
        p.ShowFloatingText("Un mecanismo 2");
        foreach (GameObject e in p.objetosRecogidos)
        {
            Debug.Log(e.name);
        }
    }
}
