using UnityEngine;

public class EnemyAttention  : MonoBehaviour
{

    public float attention = 0f;
    public float attentionMax = 100f;
    public float incrementPerSecond = 10f; // cuánto sube la atención por segundo
    public float decrementPerSecond = 5f;  // cuánto baja si el player está lejos

    public float maxVisionBonus = 5f;  // máximo aumento del rango de visión
    public float maxSpeedBonus = 2f;   // máximo aumento de velocidad




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAttention(float distanceToPlayer, float baseVisionRange, float deltaTime)
    {
        // Aumentar atención si el player está dentro del rango base
        if (distanceToPlayer <= baseVisionRange)
        {
            attention += incrementPerSecond * deltaTime;
            if (attention > attentionMax) attention = attentionMax;
        }
        else
        {
            // Reducir atención si el player está lejos
            attention -= decrementPerSecond * deltaTime;
            if (attention < 0f) attention = 0f;
        }
    }
    public float GetVisionBonus()
    {
        return (attention / attentionMax) * maxVisionBonus;
    }

    // Calcula bonificación de velocidad según atención
    public float GetSpeedBonus()
    {
        return (attention / attentionMax) * maxSpeedBonus;
    }

}