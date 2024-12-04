using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogerObjeto : MonoBehaviour
{
    public GameObject handPoint;
    private GameObject pickedObject = null;

    // Marcador de progreso
    private int objetosEntregados = 0;
    public int totalObjetos = 3; // Total de objetos necesarios

    void Update()
    {
        // Soltar objeto
        if (pickedObject != null)
        {
            if (Input.GetKey("r"))
            {
                pickedObject.GetComponent<Rigidbody>().useGravity = true;
                pickedObject.GetComponent<Rigidbody>().isKinematic = false;
                pickedObject.transform.SetParent(null);
                pickedObject = null;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Recoger objeto
        if (other.gameObject.CompareTag("Objeto"))
        {
            if (Input.GetKey("e") && pickedObject == null)
            {
                other.GetComponent<Rigidbody>().useGravity = false;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = handPoint.transform.position;
                other.transform.SetParent(handPoint.transform);
                pickedObject = other.gameObject;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detectar interacción con el baúl
        if (other.gameObject.CompareTag("Baul") && pickedObject != null)
        {
            // Incrementar marcador y destruir el objeto
            objetosEntregados++;
            Debug.Log("Objetos entregados: " + objetosEntregados + "/" + totalObjetos);

            Destroy(pickedObject);
            pickedObject = null;

            // Comprobar si se entregaron todos los objetos
            if (objetosEntregados >= totalObjetos)
            {
                Debug.Log("¡Todos los objetos han sido entregados!");
                // Aquí puedes implementar lo que sucede al completar la misión
            }
        }
    }
}
