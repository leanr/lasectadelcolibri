using TMPro;
using UnityEngine;

public class PuzzleLock: Interactuable
{
    public Canvas lockCanvas;
    public TextMeshProUGUI number1;
    public TextMeshProUGUI number2;
    public TextMeshProUGUI number3;
    public TextMeshProUGUI number4;
    public int targetNumber;
    private bool hasBeenInitialized = false;
    private bool solved = false;
    public Sprite openLockSprite;
    [HideInInspector]
    public GameObject gameObjectToSpawn;

    public void ToggleLockCanvas()
    {
        if (lockCanvas.gameObject.activeSelf)
        {
            lockCanvas.gameObject.SetActive(false);
            Time.timeScale = 1f;

            if (!solved)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ShowFloatingText("I need to find the correct code...");
            }
            else
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ShowFloatingText("The lock has been unlocked!");

                // Disable interaction 
                GetComponentInChildren<IndicadorInteracciones>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;

                // update lock sprite
                GetComponent<SpriteRenderer>().sprite = openLockSprite;

                this.enabled = false;
            }
        }
        else
        {
            lockCanvas.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void AddNumber(TextMeshProUGUI number)
    {
        number.text = ((int.Parse(number.text) + 1) % 10).ToString();
        CheckStrongBox();
    }

    public void SubstractNumber(TextMeshProUGUI number)
    {
        int result = (int.Parse(number.text) - 1);
        if (result < 0)
        {
            result = 9;
        }
        number.text = result.ToString();
        CheckStrongBox();
    }

    public void InitializeLock()
    {
        number1.text = Random.Range(0, 10).ToString();
        number2.text = Random.Range(0, 10).ToString();
        number3.text = Random.Range(0, 10).ToString();
        number4.text = Random.Range(0, 10).ToString();

        targetNumber = Random.Range(0, 10000);
    }

    public void CheckStrongBox()
    {
        string targetNumberString = targetNumber.ToString("D4");

        if (number1.text[0] == targetNumberString[0] && number2.text[0] == targetNumberString[1] && number3.text[0] == targetNumberString[2] 
            && number4.text[0] == targetNumberString[3])
        {
            solved = true;
            ToggleLockCanvas();
            gameObjectToSpawn.transform.localPosition = new Vector3(gameObjectToSpawn.transform.localPosition.x, gameObjectToSpawn.transform.localPosition.y - 2,
                gameObjectToSpawn.transform.localPosition.z);
            gameObjectToSpawn.SetActive(true);
        }
    }

    public void SetGameObjectToSpawn(GameObject gameObjectToSpawnParam)
    {
        gameObjectToSpawn = gameObjectToSpawnParam;
    }

    public override void Usar(PlayerController p)
    {
        ToggleLockCanvas();
    }

    void OnEnable()
    {
        if (!hasBeenInitialized)
        {
            InitializeLock();
            hasBeenInitialized = true;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lockCanvas.gameObject.SetActive(false);
    }
}
