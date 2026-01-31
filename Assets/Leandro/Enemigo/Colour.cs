using UnityEngine;

public class Colour : MonoBehaviour
{


   public Color[] colores = {
    Color.red,
    Color.blue,
    Color.green,
    Color.yellow,
};

    SpriteRenderer smr;
    Movement enemy;


    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        enemy = GetComponent<Movement>();
        smr = GetComponentInChildren<SpriteRenderer>();

        if (smr == null)
        {
            Debug.LogError("No se encontró SkinnedMeshRenderer");
            return;
        }

        if (enemy == null)
        {
            Debug.LogError("No se encontró EnemyMovement");
            return;
        }


        
        Debug.Log("SpriteRenderer encontrado: " + (smr != null));
        int index = Random.Range(0, colores.Length);
        Color elegido = colores[index];

        smr.color = elegido;


        // =========================
        // ASIGNAR TIPO SEGÚN COLOR
        // =========================
        if (elegido == Color.red)
        {
            enemy.enemyType = Movement.EnemyType.SensibleARuido;
        }
        else if (elegido == Color.yellow)
        {
            enemy.enemyType = Movement.EnemyType.SensibleALuz;
        }
        else
        {
            enemy.enemyType = Movement.EnemyType.Normal;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


