using UnityEngine;

public class Llave : Interactuable
{
    public GameObject inventoryReference;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public override void Usar(PlayerController p)
    {
        p.Recoger(this.gameObject);
        this.gameObject.SetActive(false);
        inventoryReference.SetActive(true);
        p.ShowFloatingText("I've picked up a key...");
        
        //Debug.Log("Llave recogida");
        //foreach (GameObject e in p.objetosRecogidos)
        //{
        //    //Debug, necesito que imprima todas las clases de los objetos, como lo hago?
        //    Debug.Log(e.name);
        //}
    }
}
