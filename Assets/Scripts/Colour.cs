using UnityEngine;

public class Colour : MonoBehaviour
{


   public Color[] colores = {
    Color.red,
    Color.blue,
    Color.green,
    Color.yellow,
};

    SkinnedMeshRenderer smr;


    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        smr.material = new Material(smr.material);
        Debug.Log("SpriteRenderer encontrado: " + (smr != null));
        int index = Random.Range(0, colores.Length);
        smr.material.color = colores[index];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
