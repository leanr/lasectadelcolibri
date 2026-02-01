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

    [Header("Prefab del enemigo")]
    public GameObject enemyPrefab;

    [Header("Cantidad de enemigos")]
    public int numeroDeEnemigos = 4;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Probabilidades (suman 100)")]
    public int probAmarillo = 50;
    public int probAzul = 25;
    public int probVerde = 15;
    public int probRojo = 10;

    public Color colorAmarillo = Color.yellow;
    public Color colorAzul = Color.blue;
    public Color colorVerde = Color.green;
    public Color colorRojo = Color.red;

    [Header("Aura")]
    public GameObject auraPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {

        Debug.Log("Colour.Start() SE EJECUTÓ");

        if (probAmarillo + probAzul + probVerde + probRojo != 100)
        {
            Debug.LogWarning(
                "Las probabilidades no suman 100. Ajustando valores por defecto."
            );

            probAmarillo = 50;
            probAzul = 25;
            probVerde = 15;
            probRojo = 10;
        }

        for (int i = 0; i < numeroDeEnemigos; i++)
        {
            SpawnEnemy();
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnEnemy()

    {
        Debug.Log("SpawnEnemy() llamado");
        if (enemyPrefab == null)
        {
            Debug.LogError("❌ enemyPrefab es NULL");
            return;
        }

        if (spawnPoints == null)
        {
            Debug.LogError("❌ spawnPoints es NULL");
            return;
        }


        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];



        GameObject enemyObj =
                Instantiate(enemyPrefab, spawn.position, Quaternion.identity);

        Debug.Log("✅ Enemigo INSTANCIADO");

        AsignarColorYTipo(enemyObj);
    }

    void AsignarColorYTipo(GameObject enemyObj)
    {




        Movement enemy = enemyObj.GetComponent<Movement>();
        SpriteRenderer[] renderers =
    enemyObj.GetComponentsInChildren<SpriteRenderer>();

        if (enemy == null || renderers.Length == 0)
        {
            Debug.LogError("Enemy sin Movement o SpriteRenderer");
            return;
        }

        Debug.Log(
    $"SPAWN {enemyObj.name} | Pos {enemyObj.transform.position}"
);

        int roll = Random.Range(0, 100);

        int limiteAmarillo = probAmarillo;
        int limiteAzul = limiteAmarillo + probAzul;
        int limiteVerde = limiteAzul + probVerde;
        int limiteRojo = limiteVerde + probRojo;

        Color colorFinal;
        Movement.EnemyType tipoFinal;
        string tagFinal;

        if (roll < limiteAmarillo)
        {
            colorFinal = colorAmarillo;
            tipoFinal = Movement.EnemyType.SensibleALuz;
            tagFinal = "SensibleALuz";
        }
        else if (roll < limiteAzul)
        {
            colorFinal = colorAzul;
            tipoFinal = Movement.EnemyType.SensibleARuido;
            tagFinal = "SensibleARuido";
        }
        else if (roll < limiteVerde)
        {
            colorFinal = colorVerde;
            tipoFinal = Movement.EnemyType.Veloz;
            tagFinal = "Veloz";
        }
        else
        {
            colorFinal = colorRojo;
            tipoFinal = Movement.EnemyType.Inaturdible;
            tagFinal = "Inaturdible";
        }

        // 🔥 APLICAR A TODOS LOS SPRITES

        enemy.enemyType = tipoFinal;
        enemyObj.tag = tagFinal;

        Debug.Log($"ASIGNADO → {tipoFinal} | {colorFinal}");
        /*
        foreach (var r in renderers)
        {
            Color c = colorFinal;
            c.a = 1f;
            r.color = c;
        }
        */

        if (auraPrefab != null)
        {
            GameObject aura =
     Instantiate(auraPrefab, enemyObj.transform);

            aura.transform.localPosition = Vector3.zero;
            aura.transform.localScale = Vector3.one * 1.4f;

            SpriteRenderer sr = aura.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                Color c = Color.Lerp(colorFinal, Color.white, 0.4f);
                c.a = 0.25f;
                sr.color = c;
            }
            else
            {
                Debug.LogError("NO HAY SpriteRenderer EN EL AURA");
            }
        }

    }
}