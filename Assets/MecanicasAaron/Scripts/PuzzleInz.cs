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
        this.gameObject.SetActive(false);
        Debug.Log("Es un numpad");
        
        foreach (GameObject e in p.objetosRecogidos)
        {
            Debug.Log(e.name);
        }
    }
}
