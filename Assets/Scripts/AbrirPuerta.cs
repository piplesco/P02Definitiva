using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirPuerta : MonoBehaviour
{
    public Transform puerta;    // Asigna este campo en el inspector con el objeto de la puerta
    public float speed = 2f;    // Velocidad de apertura de la puerta
    public float openPositionX = 5f;   // La posici�n a la que la puerta se mover� (ajusta seg�n lo necesites)
    
    private bool puertaAbierta = false;

    void Update()
    {
        // Verificar si la llave est� en la mano y presionar "R" para abrir la puerta
        if (puerta != null && Input.GetKeyDown(KeyCode.R) && GameObject.Find("Llave") == null)
        {
            // Mover la puerta hacia una nueva posici�n (en este caso hacia la derecha)
            if (!puertaAbierta)
            {
                puerta.position = new Vector3(puerta.position.x + openPositionX, puerta.position.y, puerta.position.z);
                puertaAbierta = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Aqu� puedes agregar cualquier condici�n que necesites para detectar si el jugador est� cerca de la puerta
        // Por ejemplo:
        if (other.CompareTag("Player"))
        {
            // Hacer que se abra la puerta solo cuando el jugador tiene la llave en la mano
            if (GameObject.Find("Llave") == null) // Verificar si la llave est� en la mano
            {
                puertaAbierta = false;  // Se abre la puerta
            }
        }
    }
}


