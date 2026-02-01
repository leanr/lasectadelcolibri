using UnityEngine;

public class ThrowingObject : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
             PlayerController player = collision.gameObject.GetComponent<PlayerController>();
             player.currentHealth -= 25f;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // El segundo parámetro es el retraso en segundos
        Destroy(gameObject, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
