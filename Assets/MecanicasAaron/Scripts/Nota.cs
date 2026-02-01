using UnityEngine;

public class Nota : Interactuable
{
    public int code = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (code == 0){
            code = Random.Range(1000, 9999);
        }
    }

    public override void Usar(PlayerController p)
    {
        //TODO: hace que se guarde en el inventario y se pueda releer desde este
        // p.Recoger(this.gameObject);
        // this.gameObject.SetActive(false);
        Debug.Log("Nota mirada pone: \"La contraseña es: "+code+"\".");
        p.ShowFloatingText("It is written in the note: "+code+"");
        foreach (GameObject e in p.objetosRecogidos)
        {
            Debug.Log(e.GetType());
        }
    }
}
