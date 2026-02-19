using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BossDoor : Interactuable
{

    public bool showOpenDoorFloatingText = true;
    private Light2D globalLight;

    public void EnterBoss()
    {
        globalLight.intensity = 0.6f;
        GoToBossRoom();
        CameraController.instance.UpdateCameraLimitsBoss();
        FinalBossController.instance.gameObject.SetActive(true);
        FinalBossController.instance.ShowBossHealthSlider();
    }

    public void GoToBossRoom()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().transform.position = new Vector3(0, 150f, 0);
    }

    public override void Usar(PlayerController p)
    {
        bool llave = false;

        foreach (GameObject GO in p.objetosRecogidos)
        {
            if (GO.GetComponent<Llave>() != null)
            {
                llave = true;
            }
        }

        if (llave)
        {
            p.GastarLlave();
            Inventario.instance.keyReference.SetActive(false);
            if (showOpenDoorFloatingText)
            {
                p.ShowFloatingText("What... is this? Adam... is that you?");
            }

            EnterBoss();
        }
        else
        {
            p.ShowFloatingText("The door is locked...");
        }

    }

    void Start()
    {
        globalLight = GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>();
    }
}
