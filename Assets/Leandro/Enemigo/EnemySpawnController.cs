using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public List<Color> colors = new List<Color> { Color.yellow, Color.blue, Color.green, Color.red, Color.magenta };
    public int enemyNum;
    public GameObject EnemyPrefab;
    public Dictionary<EnemyType, Color> randomBehaviourDict;
    List<EnemyType> enemyTypeList;
    public List<GameObject> spawnedEnemies;

    [HideInInspector]
    public static EnemySpawnController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        AssignBehaviourRandomly();
    }

    private void OnEnable()
    {
        ScenarioGenerationController.OnScenarioFullyGenerated += SpawnEnemies;
    }

    private void OnDisable()
    {
        ScenarioGenerationController.OnScenarioFullyGenerated -= SpawnEnemies;
    }

    public void AssignBehaviourRandomly()
    {
        randomBehaviourDict = new Dictionary<EnemyType, Color>();

        enemyTypeList = Enum.GetValues(typeof(EnemyType)).Cast<EnemyType>().OrderBy(x => UnityEngine.Random.value).ToList();

        for (int i = 0; i < colors.Count; i++)
        {
            randomBehaviourDict.Add(enemyTypeList[i], colors[i]);
        }
    }

    public void SpawnEnemies()
    {
        var shuffledTypes = enemyTypeList.OrderBy(x => UnityEngine.Random.value).ToList();
        List<Transform> spawnEnemiesTransformList = ScenarioGenerationController.instance.possibleEnemiesSpawnLocations;

        for (int i = 0; i < enemyNum; i++)
        {
            Transform randomEnemyTransform = spawnEnemiesTransformList[UnityEngine.Random.Range(0, spawnEnemiesTransformList.Count)];
            EnemyController enemy = Instantiate(EnemyPrefab, randomEnemyTransform).GetComponent<EnemyController>();
            spawnEnemiesTransformList.Remove(randomEnemyTransform);
            EnemyType randomEnemyType = shuffledTypes[i];
            enemy.enemyType = randomEnemyType;
            enemy.SetBaseSpeedByType();
            enemy.GetComponentInChildren<EnemyColour>().InitializeAuraRandom(randomBehaviourDict[randomEnemyType]);
            spawnedEnemies.Add(enemy.gameObject);
        }
    }
}
