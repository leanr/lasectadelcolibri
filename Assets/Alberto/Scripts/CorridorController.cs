using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CorridorController : MonoBehaviour
{
    public List<LightsOffInfo> lightsOffElements; // initialized in the inspector

    [System.Serializable]
    public struct LightsOffInfo
    {
        public BoxCollider2D corridorLightTrigger;
        public int adjacentRoomId;
    }

    public void InitializeLightTrigger(int roomIdParam)
    {
        foreach (var lightTrigger in lightsOffElements)
        {
            if (lightTrigger.adjacentRoomId == roomIdParam)
            {
                lightTrigger.corridorLightTrigger.enabled = true;
                ScenarioGenerationController.instance.scenarioList[roomIdParam].GetComponent<RoomController>().LightsOffTriggerRoom.enabled = true;
            }
        }
    }
}
