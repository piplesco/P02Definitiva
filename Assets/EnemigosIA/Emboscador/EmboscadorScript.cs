using UnityEngine;

public class Emboscador : MonoBehaviour
{
    public Transform jugador;              // Referencia al jugador
    public float tiempoDeEspera = 5f;      // Tiempo de espera antes de que el Emboscador aparezca nuevamente
    public float tiempoDePermanencia = 3f; // Tiempo de permanencia del Emboscador en la escena
    public float rangoDeVision = 5f;       // Rango máximo al que puede aparecer el Emboscador desde el jugador
    public float distanciaCercana = 1f;    // Distancia mínima entre el Emboscador y el jugador

    void Start()
    {
        // Busca el objeto con la etiqueta "Player" para asignar el transform del jugador
        jugador = GameObject.FindGameObjectWithTag("Player").transform;

        // Asegúrate de que el jugador sea encontrado
        if (jugador == null)
        {
            Debug.LogError("Jugador no encontrado. Asegúrate de que el jugador tenga la etiqueta 'Player'.");
            return;
        }

        // Inicia el proceso de aparición
        InvokeRepeating("AparecerEmboscador", 0f, tiempoDeEspera);
    }

    void AparecerEmboscador()
    {
        // Debug para comprobar que el código está siendo llamado
        Debug.Log("Emboscador aparece");

        // Obtiene una posición accesible y visible cerca del jugador
        Vector3 posicionAparicion = ObtenerPosicionVisibleFrenteAlJugador();

        // Teletransporta al Emboscador a la nueva posición
        transform.position = posicionAparicion;

        // Debug para ver la posición calculada
        Debug.Log("Posición del Emboscador: " + transform.position);

        // Hace que el Emboscador se quede durante 3 segundos en ese lugar
        Invoke("DesaparecerEmboscador", tiempoDePermanencia);
    }

    void DesaparecerEmboscador()
    {
        // Desaparece o se teletransporta a otro punto
        transform.position = new Vector3(0, -5, 0);  // Cambia esta posición por la que prefieras

        // Debug para confirmar desaparición
        Debug.Log("Emboscador desaparece");
    }

    Vector3 ObtenerPosicionVisibleFrenteAlJugador()
    {
        // Obtiene la dirección hacia la que está mirando el jugador
        Vector3 direccionDelJugador = jugador.forward; // Esto es lo que el jugador está mirando

        // Calcula una posición un poco más adelante del jugador, dentro de su campo de visión
        Vector3 posicionFrenteAlJugador = jugador.position + direccionDelJugador * Random.Range(distanciaCercana, rangoDeVision);

        // Asegurarse de que la altura (Y) sea la misma que la del jugador
        posicionFrenteAlJugador.y = jugador.position.y;

        return posicionFrenteAlJugador;
    }
}
