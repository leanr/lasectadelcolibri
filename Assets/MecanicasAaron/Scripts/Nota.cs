using UnityEngine;

public class Nota : Interactuable
{
    public int code = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (code != 0){
            code = 1234;
        }
    }

    public override void Usar(PlayerController p)
    {
        //TODO: hace que se guarde en el inventario y se pueda releer desde este
        // p.Recoger(this.gameObject);
        // this.gameObject.SetActive(false);
        Debug.Log("Nota mirada pone: \"La contraseña es: "+code+"\".");
        foreach (GameObject e in p.objetosRecogidos)
        {
            //Debug, necesito que imprima todas las clases de los objetos, como lo hago?
            Debug.Log(e.GetType());
        }
    }
}
