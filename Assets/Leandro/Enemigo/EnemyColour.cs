using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyColour : MonoBehaviour
{
    public Light2D aura;

    public void InitializeAuraRandom(Color color)
    {
        aura.color = color;
    }
}