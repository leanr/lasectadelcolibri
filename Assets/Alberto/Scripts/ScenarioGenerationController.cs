using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScenarioGenerationController : MonoBehaviour
{
    public List<GameObject> scenarioList; // initialized in inspector
    public List<CorridorController> corridors; // initialized in inspector
    public GameObject obstaclePrefab;
    public GameObject horizontalDoorPrefab;
    public GameObject verticalDoorPrefab;
    [HideInInspector]
    public int darkRoomId;
    public int amountOfLightsOffRooms = 0;
    public List<Transform> possiblePropsSpawnLocations;
    public List<Transform> possibleEnemiesSpawnLocations;
    public List<GameObject> strongBoxPrefabs;
    public GameObject noteThreeCodePrefab;
    public GameObject noteFourCodePrefab;
    public GameObject keyPrefab;

    public bool randomLayout;

    [HideInInspector]
    public static ScenarioGenerationController instance;

    public static event Action OnScenarioFullyGenerated;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void InitializeRoomPositions()
    {
        if (scenarioList == null || scenarioList.Count < 2) return;

        // 1. Guardamos las posiciones tal cual vienen (ya ordenadas)
        Vector3[] fixedPositions = scenarioList.Select(s => s.transform.position).ToArray();

        // 2. Barajamos la lista de objetos inline con LINQ
        if (randomLayout)
        {
            scenarioList = scenarioList.OrderBy(_ => UnityEngine.Random.value).ToList();
        }

        // 3. Asignamos posición e ID en un solo paso
        for (int i = 0; i < scenarioList.Count; i++)
        {
            scenarioList[i].transform.position = fixedPositions[i];

            if (scenarioList[i].TryGetComponent<RoomController>(out var controller))
            {
                controller.roomId = i;
                if (controller.isDarkRoom)
                {
                    darkRoomId = i;
                }
            }
        }
    }

    public void PlacePlayer()
    {
        // get random room
        GameObject randomRoom;
        do
        {
            randomRoom = scenarioList[UnityEngine.Random.Range(0, scenarioList.Count)];
        } while (randomRoom.GetComponent<RoomController>().isDarkRoom);

        // get random spawn point depending on the room the player is located
        List<Vector3> possibleLocalSpawnPositions = new List<Vector3>();
        int roomId = randomRoom.GetComponent<RoomController>().roomId;
        switch (roomId)
        {
            case 0:
                possibleLocalSpawnPositions.Add(new Vector3(-10, 0, 0));
                possibleLocalSpawnPositions.Add(new Vector3(0, 5, 0));
                break;
            case 1:
                possibleLocalSpawnPositions.Add(new Vector3(0, 5, 0));
                break;
            case 2:
                possibleLocalSpawnPositions.Add(new Vector3(0, 5, 0));
                possibleLocalSpawnPositions.Add(new Vector3(10, 0, 0));
                break;
            case 3:
                possibleLocalSpawnPositions.Add(new Vector3(0, -5, 0));
                possibleLocalSpawnPositions.Add(new Vector3(-10, 0, 0));
                break;
            case 4:
                possibleLocalSpawnPositions.Add(new Vector3(0, -5, 0));
                break;
            case 5:
                possibleLocalSpawnPositions.Add(new Vector3(0, -5, 0));
                possibleLocalSpawnPositions.Add(new Vector3(10, 0, 0));
                break;
        }

        GameObject.FindGameObjectWithTag("Player").transform.position = randomRoom.transform.position 
            + possibleLocalSpawnPositions[UnityEngine.Random.Range(0, possibleLocalSpawnPositions.Count)];
    }

    public void SetupDarkRoom()
    {
        GameObject[] darkRoomObstacles = GameObject.FindGameObjectsWithTag("ObstacleRoom" + darkRoomId.ToString());
        foreach (GameObject obstacle in darkRoomObstacles)
        {
            Destroy(obstacle);
        }

        Transform darkRoom = GameObject.FindGameObjectWithTag("DarkRoom").transform;

        // Instantiate all doors in DarkRoom
        GameObject horizontalDoor1 = Instantiate(horizontalDoorPrefab, Vector3.zero, horizontalDoorPrefab.transform.rotation, darkRoom);
        horizontalDoor1.transform.localPosition = new Vector3(-13.5f, 0f, 0f);
        GameObject horizontalDoor2 = Instantiate(horizontalDoorPrefab, Vector3.zero, horizontalDoorPrefab.transform.rotation, darkRoom);
        horizontalDoor2.transform.localPosition = new Vector3(13.5f, 0f, 0f);
        GameObject verticalDoor1 = Instantiate(verticalDoorPrefab, Vector3.zero, verticalDoorPrefab.transform.rotation, darkRoom);
        verticalDoor1.transform.localPosition = new Vector3(0f, 7.5f, 0f);
        GameObject verticalDoor2 = Instantiate(verticalDoorPrefab, Vector3.zero, verticalDoorPrefab.transform.rotation, darkRoom);
        verticalDoor2.transform.localPosition = new Vector3(0f, -7.5f, 0f);

    }

    public void InitializeLightsOffTrigger(int amount)
    {
        // 1. Range validation: If 0 or less, do nothing. If more than 2, cap it at 2.
        if (amount <= 0) return;
        int roomsToSelect = Mathf.Min(amount, 2);

        // 2. List to store selected indices and avoid duplicates
        List<int> selectedIndices = new List<int>();

        for (int i = 0; i < roomsToSelect; i++)
        {
            int randomRoomNumber;
            int attempts = 0; // Safety break for infinite loops

            do
            {
                randomRoomNumber = UnityEngine.Random.Range(0, scenarioList.Count);
                attempts++;
            }
            while ((scenarioList[randomRoomNumber].GetComponent<RoomController>().isDarkRoom ||
                    selectedIndices.Contains(randomRoomNumber)) && attempts < 100);

            // Only add if it's a valid, non-duplicate selection
            if (!selectedIndices.Contains(randomRoomNumber))
            {
                selectedIndices.Add(randomRoomNumber);
                Debug.Log("Random room selected: Index " + randomRoomNumber);
            }
        }

        // 3. Notify corridors with the chosen room indices
        foreach (CorridorController corridor in corridors)
        {
            foreach (int index in selectedIndices)
            {
                corridor.InitializeLightTrigger(index);
            }
        }
    }

    public void InitializeFactoryStrongbox()
    {
        // Spawn StrongBox
        Transform randomLocation = possiblePropsSpawnLocations[UnityEngine.Random.Range(0, possiblePropsSpawnLocations.Count)];
        possiblePropsSpawnLocations.Remove(randomLocation);
        GameObject instantiatedStrongBox = Instantiate(strongBoxPrefabs[UnityEngine.Random.Range(0, strongBoxPrefabs.Count)], 
            randomLocation.position, randomLocation.rotation, randomLocation.transform);
        GameObject instantiatedKey = Instantiate(keyPrefab, randomLocation.position, randomLocation.rotation, instantiatedStrongBox.transform);
        instantiatedKey.SetActive(false);

        if (instantiatedStrongBox.GetComponent<PuzzleLock>() == null)
        {
            instantiatedStrongBox.GetComponent<PuzzleStrongBox>().SetGameObjectToSpawn(instantiatedKey);

            // Spawn 3-code Note
            Transform randomNoteLocation = possiblePropsSpawnLocations[UnityEngine.Random.Range(0, possiblePropsSpawnLocations.Count)];
            possiblePropsSpawnLocations.Remove(randomNoteLocation);
            GameObject instantiatedNote = Instantiate(noteThreeCodePrefab, randomNoteLocation.position, randomNoteLocation.rotation, randomNoteLocation.transform);
        }
        else
        {
            instantiatedStrongBox.GetComponent<PuzzleLock>().SetGameObjectToSpawn(instantiatedKey);

            // Spawn 4-Code Note
            Transform randomNoteLocation = possiblePropsSpawnLocations[UnityEngine.Random.Range(0, possiblePropsSpawnLocations.Count)];
            possiblePropsSpawnLocations.Remove(randomNoteLocation);
            GameObject instantiatedNote = Instantiate(noteFourCodePrefab, randomNoteLocation.position, randomNoteLocation.rotation, randomNoteLocation.transform);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeRoomPositions();
        //if (randomLayout)
        //{
        //    PlacePlayer();
        //}
        SetupDarkRoom();
        InitializeLightsOffTrigger(amountOfLightsOffRooms);
        OnScenarioFullyGenerated?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
