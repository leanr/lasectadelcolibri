using UnityEngine;

public class Llave : Interactuable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public override void Usar(PlayerController p)
    {
        p.Recoger(this.gameObject);
        this.gameObject.SetActive(false);
        Debug.Log("Llave recogida");
        foreach (GameObject e in p.objetosRecogidos)
        {
            //Debug, necesito que imprima todas las clases de los objetos, como lo hago?
            Debug.Log(e.GetType());
        }
    }
}
