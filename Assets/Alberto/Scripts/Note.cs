using UnityEngine;

public class Note : Interactuable
{
    private int code;
    public bool codeNote = false;
    public bool threeCodeNote = false;
    public string noteText;
    public int associatedEnemy;

    public void SetCode(int code)
    {
        this.code = code;
    }

    public void SetNoteText(string noteText)
    {
        this.noteText = noteText;
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
            if (noteText == "")
            {
                EnemyType enemyType = EnemySpawnController.instance.spawnedEnemies[associatedEnemy].GetComponent<EnemyController>().enemyType;
                SetNoteText("Enemy number " + associatedEnemy + " - " + enemyType.ToString() + " - " + EnemySpawnController.instance.randomBehaviourDict[enemyType]);
            }
            p.ShowFloatingText(noteText);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

}
