using System.Collections;
using UnityEngine;

public class RecogerLlave : MonoBehaviour
{
    public GameObject handPoint;  // Punto donde se lleva la llave (mano)
    private GameObject pickedLlave = null;  // Llave recogida

    void Update()
    {
        if (pickedLlave != null)
        {
            // Soltar la llave con la tecla "R"
            if (Input.GetKey("r"))
            {
                pickedLlave.GetComponent<Collider>().enabled = true;  // Activar el collider de la llave
                pickedLlave.GetComponent<Rigidbody>().useGravity = true;
                pickedLlave.GetComponent<Rigidbody>().isKinematic = false;
                pickedLlave.transform.SetParent(null);
                pickedLlave = null;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Si el objeto que está cerca es la llave y presionamos "E", la recogemos
        if (other.gameObject.CompareTag("Llave") && Input.GetKey("e") && pickedLlave == null)
        {
            pickedLlave = other.gameObject;

            // Desactivamos el collider para evitar más interacciones
            pickedLlave.GetComponent<Collider>().enabled = false;

            // Colocamos la llave en la mano del jugador
            pickedLlave.transform.position = handPoint.transform.position;
            pickedLlave.transform.SetParent(handPoint.transform);  // La llave se mueve con la mano del jugador

            // Hacer visible la llave
            Renderer renderer = pickedLlave.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = true;  // Aseguramos que el Renderer de la llave esté activado
            }
        }
    }
}

