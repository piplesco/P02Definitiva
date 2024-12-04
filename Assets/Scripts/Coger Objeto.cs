using System.Collections;
using UnityEngine;
using TMPro; // Importar TextMeshPro para el marcador

public class CogerObjeto : MonoBehaviour
{
    public GameObject handPoint; // El punto en la mano donde se lleva el objeto
    private GameObject pickedObject = null; // El objeto que el jugador está sosteniendo

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
            pickedObject.transform.SetParent(null); // El objeto ya no será hijo de la mano
            pickedObject = null; // Ya no hay objeto en las manos
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Recoger objeto con la tecla "E"
        if (other.gameObject.CompareTag("Objeto") && Input.GetKey("e") && pickedObject == null)
        {
            other.GetComponent<Rigidbody>().useGravity = false; // Desactivar la gravedad para que el objeto no caiga
            other.GetComponent<Rigidbody>().isKinematic = true; // Hacer que el objeto no se vea afectado por la física
            other.transform.position = handPoint.transform.position; // Colocar el objeto en la mano
            other.transform.SetParent(handPoint.transform); // El objeto se convierte en hijo de la mano
            pickedObject = other.gameObject; // Guardamos el objeto en la variable pickedObject

            other.gameObject.GetComponent<Renderer>().enabled = true;
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

    // Asegúrate de que este método esté fuera de otros métodos
    private void ActualizarContador()
    {
        // Actualizar el texto del marcador
        contadorTexto.text = "OBJETOS: " + objetosEntregados + "/" + totalObjetos;
    }
}


