using UnityEngine;

public class Filtros : Interactuable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public override void Usar(PlayerController p)
    {
        //p.Recoger(this.gameObject);
        this.gameObject.SetActive(false);
        p.currentContaminationLevel = p.maxContaminationLevel;
        p.ShowFloatingText("I've found a filter for my mask");

        //Debug.Log("Filtros recogidos");
        //foreach (GameObject e in p.objetosRecogidos)
        //{
        //    Debug.Log(e.name);
        //}
    }
}
