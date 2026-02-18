using Mono.Cecil.Cil;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Note : Interactuable
{
    private int code;
    public bool codeNote = false;
    public bool threeCodeNote = false;
    public string noteText;

    public void SetCode(int code)
    {
        this.code = code;
    }

    public override void Usar(PlayerController p)
    {
        if (codeNote)
        {

            if (threeCodeNote)
            {
                SetCode(GameObject.FindWithTag("PuzzleStrongBox").GetComponent<PuzzleStrongBox>().targetNumber);
            }
            else
            {
                SetCode(GameObject.FindWithTag("PuzzleLock").GetComponent<PuzzleLock>().targetNumber);
            }

            p.ShowFloatingText("The following code is written in the note: " + code.ToString());

        }
        else
        {
            p.ShowFloatingText(noteText);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

}
