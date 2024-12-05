using System.Collections;
using UnityEngine;

public class AbrirPuerta : MonoBehaviour
{
    public GameObject puerta; // La puerta que se va a abrir
    public Transform puntoApertura; // El punto donde la puerta se abre (p.ej., posición final de apertura)
    public float velocidadApertura = 2f; // Velocidad con la que se abrirá la puerta
    private bool tieneLlave = false; // Para verificar si el jugador tiene la llave

    void Update()
    {
        // Verifica si el jugador tiene la llave y presiona "R" para abrir la puerta
        if (tieneLlave && Input.GetKeyDown(KeyCode.R))
        {
            AbrirPuertaAnimacion();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Si el jugador entra en el área de la puerta y tiene la llave, puede abrirla
        if (other.CompareTag("Player"))
        {
            tieneLlave = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si el jugador sale del área de la puerta, ya no puede abrirla
        if (other.CompareTag("Player"))
        {
            tieneLlave = false;
        }
    }

    private void AbrirPuertaAnimacion()
    {
        // Mueve la puerta a la posición de apertura
        puerta.transform.position = Vector3.MoveTowards(puerta.transform.position, puntoApertura.position, velocidadApertura * Time.deltaTime);
    }
}

