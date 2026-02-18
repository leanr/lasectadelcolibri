using System.Collections.Generic;
using UnityEngine;

public class InitialRoomGenerationController : MonoBehaviour
{
    public List<Transform> propsSpawnLocation;
    public List<GameObject> propsToSpawn;
    [HideInInspector]
    public List<GameObject> propsToInsertInBox;
    public int numberProps;
    public bool spawnAllProps;

    public void InitializePropsSpawnLocation()
    {
        // initialize the list of that contains the objects that will be possibly inside the box
        propsToInsertInBox = new List<GameObject>();

        // 1. Safety Check: Ensure lists are assigned and not empty
        if (propsSpawnLocation == null || propsSpawnLocation.Count == 0)
        {
            Debug.LogWarning("No spawn locations assigned!");
            return;
        }

        if (propsToSpawn == null || propsToSpawn.Count == 0)
        {
            Debug.LogWarning("No props to spawn assigned!");
            return;
        }

        // 2. Determine how many objects to spawn based on the boolean toggle
        int amountToSpawn = spawnAllProps ? propsToSpawn.Count : numberProps;

        // 3. Create a temporary copy of the locations list 
        // This allows us to remove used spots without modifying the original Inspector list
        List<Transform> availableLocations = new List<Transform>(propsSpawnLocation);

        for (int i = 0; i < amountToSpawn; i++)
        {
            // Stop if we run out of available spawn points to avoid errors
            if (availableLocations.Count == 0)
            {
                Debug.LogWarning("Ran out of spawn locations before finishing all props.");
                break;
            }

            // Pick a prop from the pool in order
            GameObject propPrefab = propsToSpawn[i];

            // Pick a random index from the remaining available locations
            int randomIndex = Random.Range(0, availableLocations.Count);
            Transform targetTransform = availableLocations[randomIndex];

            // Instantiate the prop at the selected location's position and rotation
            GameObject instantiatedObject = Instantiate(propPrefab, targetTransform.position, targetTransform.rotation, targetTransform.transform);

            // Insert it if its a key or a note that contains information of the enemies
            if (instantiatedObject.GetComponent<Llave>() != null)
            {
                propsToInsertInBox.Add(instantiatedObject);
            }
            else if (instantiatedObject.GetComponent<Note>() != null)
            {
                if (!instantiatedObject.GetComponent<Note>().codeNote)
                {
                    propsToInsertInBox.Add(instantiatedObject);
                }
            }

            // 4. Remove the used location from the temporary list to prevent duplicates
            availableLocations.RemoveAt(randomIndex);
        }
    }

    public void InsertInTheBoxes()
    {
        // 1. Safety Check: Ensure we have enough props to fill the boxes
        if (propsToInsertInBox == null || propsToInsertInBox.Count < 2)
        {
            Debug.LogError("Not enough props in propsToInsertInBox to fill both puzzles!");
            return;
        }

        // 2. Create a temporary copy to handle "no-repetition" logic
        List<GameObject> availableProps = new List<GameObject>(propsToInsertInBox);

        // 3. Pick the first random object for the PuzzleLock
        int index1 = Random.Range(0, availableProps.Count);
        GameObject gameobject1 = availableProps[index1];

        // Remove it so it can't be picked again
        availableProps.RemoveAt(index1);

        // 4. Pick the second random object for the PuzzleStrongBox
        int index2 = Random.Range(0, availableProps.Count);
        GameObject gameobject2 = availableProps[index2];

        // 5. Find the targets and assign the objects
        GameObject lockObj = GameObject.FindWithTag("PuzzleLock");
        if (lockObj != null)
        {
            lockObj.GetComponent<PuzzleLock>().SetGameObjectToSpawn(gameobject1);
            gameobject1.SetActive(false);
            gameobject1.transform.position = lockObj.transform.position;  
        }

        GameObject strongBoxObj = GameObject.FindWithTag("PuzzleStrongBox");
        if (strongBoxObj != null)
        {
            strongBoxObj.GetComponent<PuzzleStrongBox>().SetGameObjectToSpawn(gameobject2);
            gameobject2.SetActive(false);
            gameobject2.transform.position = strongBoxObj.transform.position;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializePropsSpawnLocation();
        InsertInTheBoxes();
    }

    // Update is called once per frame
    void Update()
    {

    }
}