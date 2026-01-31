using System;
using Unity.VisualScripting;
using UnityEngine;

public class PuertaInteractuable : Interactuable
{
    [Header("Configuracion Puerta")]
    public bool llave = false;

    public float wherex = 0;
    public float wherey = 0;
    
    // Añade estas propiedades para personalizar el gizmo
    public Color gizmoColor = Color.cyan;
    public float gizmoRadius = 1f;
    public bool mostrarGizmo = true;

    public override void Usar(PlayerControllerCopy p)
    {
        if (llave)
        {
            if (p.GastarLlave())
            {
                llave = false;
                //Playear un sonido
            }
            else
            {
                //Playear un sonido
            }
        }
        else
        {
            p.transform.position = this.transform.position + new Vector3(wherex, wherey, p.transform.position.z);
        }
        
    }

    // Método para dibujar gizmos en el editor
    private void OnDrawGizmos()
    {
        if (!mostrarGizmo) return;
        
        // Guarda el color original
        Color originalColor = Gizmos.color;
        
        // Establece el color para el círculo
        Gizmos.color = gizmoColor;
        
        // Calcula la posición de destino
        Vector3 destino = this.transform.position + new Vector3(wherex, wherey, 0);
        
        // Dibuja un círculo en la posición de destino
        DrawCircleGizmo(destino, gizmoRadius);
        
        // Dibuja una línea desde la puerta hasta el destino
        Gizmos.DrawLine(this.transform.position, destino);
        
        // Restaura el color original
        Gizmos.color = originalColor;
    }

    // Método para dibujar un círculo con gizmos
    private void DrawCircleGizmo(Vector3 center, float radius, int segments = 20)
    {
        if (radius <= 0) return;
        
        float angle = 0f;
        Vector3 lastPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;
        
        for (int i = 0; i <= segments; i++)
        {
            angle = i / (float)segments * Mathf.PI * 2;
            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;
            
            Vector3 point = center + new Vector3(x, y, 0);
            
            if (i > 0)
            {
                Gizmos.DrawLine(lastPoint, point);
            }
            else
            {
                firstPoint = point;
            }
            
            lastPoint = point;
        }
        
        // Conectar el último punto con el primero
        Gizmos.DrawLine(lastPoint, firstPoint);
    }
}