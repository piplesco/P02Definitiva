using System.Collections;
using UnityEngine;
using TMPro; // Importar TextMeshPro

public class CogerObjeto : MonoBehaviour
{
    public GameObject handPoint; // Punto donde se lleva el objeto
    private GameObject pickedObject = null; // Objeto recogido actualmente

    // Referencia al marcador en la UI
    public TMP_Text contadorTexto;
    private int objetosEntregados = 0; // Contador de objetos entregados
    public int totalObjetos = 3; // Total de objetos necesarios

    void Update()
    {
        // Soltar objeto con la tecla "R"
        if (pickedObject != null && Input.GetKey("r"))
        {
            pickedObject.GetComponent<Rigidbody>().useGravity = true;
            pickedObject.GetComponent<Rigidbody>().isKinematic = false;
            pickedObject.transform.SetParent(null);
            pickedObject = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Recoger objeto con la tecla "E"
        if (other.gameObject.CompareTag("Objeto") && Input.GetKey("e") && pickedObject == null)
        {
            other.GetComponent<Rigidbody>().useGravity = false;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.position = handPoint.transform.position;
            other.transform.SetParent(handPoint.transform);
            pickedObject = other.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detectar interacción con el baúl
        if (other.gameObject.CompareTag("Baul") && pickedObject != null)
        {
            // Incrementar el contador y actualizar la UI
            objetosEntregados++;
            ActualizarContador();

            // Destruir el objeto entregado
            Destroy(pickedObject);
            pickedObject = null;

            // Verificar si se han entregado todos los objetos
            if (objetosEntregados >= totalObjetos)
            {
            contadorTexto.text = "¡Todos los objetos han sido entregados!";
                
            }
        }
    }

    private void ActualizarContador()
    {
        // Actualizar el texto del marcador
        contadorTexto.text = "OBJETOS: " + objetosEntregados + "/" + totalObjetos;
    }
}

