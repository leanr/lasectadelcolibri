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
            enemy.enemyType = Movement.EnemyType.Inaturdible;
            gameObject.tag = "Inaturdible";
             Debug.Log("Enemy tag: " + gameObject.tag);

            }
            else if (elegido == Color.yellow)
            {
            enemy.enemyType = Movement.EnemyType.SensibleALuz;
            gameObject.tag = "SensibleALuz";
            Debug.Log("Enemy tag: " + gameObject.tag);

            }
            
            else if (elegido == Color.blue)
            {
            enemy.enemyType = Movement.EnemyType.SensibleARuido;
            gameObject.tag = "SensibleARuido";
            Debug.Log("Enemy tag: " + gameObject.tag);

            }
            else if (elegido == Color.green)
            {
            enemy.enemyType = Movement.EnemyType.Veloz;
            gameObject.tag = "Veloz";
            Debug.Log("Enemy tag: " + gameObject.tag);

            }
            else
            {
            enemy.enemyType = Movement.EnemyType.Normal;
            gameObject.tag = "Normal";
            Debug.Log("Enemy tag: " + gameObject.tag);

            }
         
    }

        // Update is called once per frame
        void Update()
        {
        
        }
    }


