using UnityEngine;

public class MantenerAnimatorActivo : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        // Cuando el objeto se activa, forzamos al Animator a continuar
        if (anim != null)
        {
            anim.enabled = true;
            anim.Update(0f); // Refresca el estado inmediatamente
        }
    }

    void Update()
    {
        // Asegura que el Animator nunca se desactive
        if (anim != null && !anim.enabled)
        {
            anim.enabled = true;
        }
    }
}
