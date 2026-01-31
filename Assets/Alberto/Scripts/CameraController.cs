using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = PlayerController.instance.transform.position;
        transform.position = new Vector3(playerPosition.x, playerPosition.y, -10f);
    }
}
