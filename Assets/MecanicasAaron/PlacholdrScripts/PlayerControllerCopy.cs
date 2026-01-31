//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using UnityEngine;

//public class PlayerControllerCopy : MonoBehaviour
//{
//    private Rigidbody2D rb;
//    public float speed;

//    [HideInInspector]
//    public float currentHealth;
//    [HideInInspector]
//    public float currentContaminationLevel;
//    public float maxHealth;
//    public float maxContaminationLevel;

//    [HideInInspector]
//    public TorchController torch;
    
//    [HideInInspector]
//    //NOTE: MOD PLAYERCONTROLLER A PLAYERCONTROLLERCOPY
//    public static PlayerControllerCopy instance;
//    //NOTE: FIN MOD PLAYERCONTROLLER A PLAYERCONTROLLERCOPY

//    //NOTE: AÑADIDO
//    public List<GameObject> objetosColsiones = new List<GameObject>();
//    public List<GameObject> objetosRecogidos = new List<GameObject>();

//    public void Recoger(GameObject go)
//    {
//        this.objetosRecogidos.Add(go);
//    }

//    public bool GastarLlave()
//    {
//        List<GameObject> objetosACopiar = new List<GameObject>(objetosRecogidos);
            
//            foreach (GameObject e in objetosACopiar)
//            {
//                if (e.GetComponent<Llave>() != null)
//                {
//                    objetosRecogidos.Remove(e);

//                    return true;
//                }
//            }
//        return false;
//    }

//    //NOTE: FIN AÑADIDO

//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//        }
//    }

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();

//        // Hardcoded physics settings to ensure it works Top-Down
//        rb.gravityScale = 0f;
//        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
//        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

//        //Initialize player attributes
//        maxHealth = 100f;
//        maxContaminationLevel = 100f;

//        torch = GetComponent<TorchController>();

//    }

//    void FixedUpdate()
//    {
//        Vector2 direction = Vector2.zero;

//        // Hardcoded WASD Detection
//        if (Input.GetKey(KeyCode.W)) direction.y = 1;
//        if (Input.GetKey(KeyCode.S)) direction.y = -1;
//        if (Input.GetKey(KeyCode.A)) direction.x = -1;
//        if (Input.GetKey(KeyCode.D)) direction.x = 1;

//        // Normalizing manually to prevent fast diagonal movement
//        if (direction.magnitude > 1)
//        {
//            direction.Normalize();
//        }

//        // Torch detection
//        if (Input.GetKey(KeyCode.E))
//        {
//            torch.ToggleTorch();
//        }

//        if (Input.GetKey(KeyCode.F))
//        {
//            //NOTE: AÑADIDO
//            // Crear una copia de la lista para evitar modificación durante iteración
//            List<GameObject> objetosACopiar = new List<GameObject>(objetosColsiones);
            
//            foreach (GameObject e in objetosACopiar)
//            {
//                if (e != null && e.layer == 31) // Verificar que el objeto no sea null
//                {
//                    if (e.GetComponent<PuertaInteractuable>() != null)
//                    {
//                        e.GetComponent<PuertaInteractuable>().Usar(this);
//                    }
//                    else if(e.GetComponent<Llave>() != null)
//                    {
//                        e.GetComponent<Llave>().Usar(this);
//                    }
//                }
//            }
//            //NOTE: FIN AÑADIDO
//        }

//        // Hardcoded speed value: 5.0f
//        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
//    }

//    //NOTE: AÑADIDO
//    void OnTriggerEnter2D(Collider2D c)
//    {
//        GestorInteractuar.GestorColision(c ,true);
//        objetosColsiones.Add(c.gameObject);
//    }

//    void OnTriggerExit2D(Collider2D c)
//    {
//        GestorInteractuar.GestorColision(c ,false);
//        objetosColsiones.Remove(c.gameObject);
//    }
//    //NOTE: FIN AÑADIDO
//}
