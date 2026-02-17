using TMPro;
using UnityEngine;
using UnityEngine.U2D.IK;

public class PuzzleStrongBox : Interactuable
{
    public Canvas StrongBoxCanvas;
    public TextMeshProUGUI number1;
    public TextMeshProUGUI number2;
    public TextMeshProUGUI number3;
    public int targetNumber;
    private bool hasBeenInitialized = false;
    private bool solved = false;
    public Sprite openBoxSprite;

    public void ToggleStrongBoxCanvas()
    {
        if (StrongBoxCanvas.gameObject.activeSelf)
        {
            StrongBoxCanvas.gameObject.SetActive(false);
            Time.timeScale = 1f;

            if (!solved)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ShowFloatingText("I need to find the correct code...");
            }
            else
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ShowFloatingText("The safe has opened!");

                // Disable interaction 
                GetComponentInChildren<IndicadorInteracciones>().enabled = false;
                this.enabled = false;

                // update strongbox sprite
                GetComponent<SpriteRenderer>().sprite = openBoxSprite;
            }
        }
        else
        {
            StrongBoxCanvas.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void AddNumber(TextMeshProUGUI number)
    {
        number.text = ((int.Parse(number.text) + 1)%10).ToString();
    }

    public void InitializeStrongBox()
    {
        number1.text = Random.Range(0, 10).ToString();
        number2.text = Random.Range(0, 10).ToString();
        number3.text = Random.Range(0, 10).ToString();

        targetNumber = Random.Range(0, 1000);
    }

    public void CheckStrongBox()
    {
        string targetNumberString = targetNumber.ToString();

        if (number1.text[0] == targetNumberString[0] && number2.text[0] == targetNumberString[1] && number3.text[0] == targetNumberString[2])
        {
            solved = true;
            ToggleStrongBoxCanvas();
        }
    }

    public override void Usar(PlayerController p)
    {
        ToggleStrongBoxCanvas();
    }

    void OnEnable()
    {
        if (!hasBeenInitialized)
        {
            InitializeStrongBox();
            hasBeenInitialized = true;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StrongBoxCanvas.gameObject.SetActive(false);
    }
}
