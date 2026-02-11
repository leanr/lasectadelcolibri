using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScenarioGenerationController : MonoBehaviour
{
    public List<GameObject> scenarioList;
    public GameObject obstaclePrefab;
    public GameObject horizontalDoorPrefab;
    public GameObject verticalDoorPrefab;
    [HideInInspector]
    public int darkRoomId;

    public bool randomLayout;

    public void InitializeRoomPositions()
    {
        if (scenarioList == null || scenarioList.Count < 2) return;

        // 1. Guardamos las posiciones tal cual vienen (ya ordenadas)
        Vector3[] fixedPositions = scenarioList.Select(s => s.transform.position).ToArray();

        // 2. Barajamos la lista de objetos inline con LINQ
        if (randomLayout)
        {
            scenarioList = scenarioList.OrderBy(_ => Random.value).ToList();
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
        GameObject randomRoom;
        do
        {
            randomRoom = scenarioList[Random.Range(0, scenarioList.Count)];
        } while (randomRoom.GetComponent<RoomController>().isDarkRoom);

        GameObject.FindGameObjectWithTag("Player").transform.position = randomRoom.transform.position;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeRoomPositions();
        if (randomLayout)
        {
            PlacePlayer();
        }
        SetupDarkRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
