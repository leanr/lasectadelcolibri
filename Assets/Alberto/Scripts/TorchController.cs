using UnityEngine;

public class TorchController : MonoBehaviour
{
    public bool isOn;

    public void ToggleTorch()
    {
        if (isOn)
        {
            Debug.Log("Torch off");
        }
        else
        {
            Debug.Log("Torch on");
        }
        isOn = !isOn;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
