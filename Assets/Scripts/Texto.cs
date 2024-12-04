using UnityEngine;
using UnityEngine.UI; // Importar para usar UI

public class Texto : MonoBehaviour
{
    public Text contadorTexto; // Referencia al texto del contador
    private int objetosEntregados = 0; // Contador de objetos entregados
    public int totalObjetos = 3; // Total de objetos necesarios

    void Start()
    {
        // Inicializar el texto del contador
        ActualizarContador();
    }

    public void EntregarObjeto()
    {
        // Incrementar el contador de objetos entregados
        objetosEntregados++;
        Debug.Log("Objeto entregado: " + objetosEntregados + "/" + totalObjetos);

        // Actualizar el texto del contador
        ActualizarContador();

        // Verificar si se han entregado todos los objetos
        if (objetosEntregados >= totalObjetos)
        {
            Debug.Log("¡Has entregado todos los objetos!");
            // Aquí puedes añadir la lógica del final del juego
        }
    }

    private void ActualizarContador()
    {
        // Actualizar el texto con la cantidad actual de objetos
        contadorTexto.text = "objetos: " + objetosEntregados + "/" + totalObjetos;
    }
}

