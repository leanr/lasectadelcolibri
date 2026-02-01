using UnityEngine;

public class Vendas : Interactuable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public override void Usar(PlayerController p)
    {
        p.Recoger(this.gameObject);
        this.gameObject.SetActive(false);
        Debug.Log("Vendas recogidas");
        p.ShowFloatingText("I will cover my wounds with this bandages...");
        foreach (GameObject e in p.objetosRecogidos)
        {
            Debug.Log(e.name);
        }
    }
}
