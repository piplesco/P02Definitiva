using UnityEngine;

public class Emboscador : MonoBehaviour
{
    public Transform jugador;              // Referencia al jugador
    public float tiempoDeEspera = 5f;      // Tiempo de espera antes de que el Emboscador aparezca nuevamente
    public float tiempoDePermanencia = 3f; // Tiempo de permanencia del Emboscador en la escena
    public float rangoDeVision = 5f;       // Rango m�ximo al que puede aparecer el Emboscador desde el jugador
    public float distanciaCercana = 1f;    // Distancia m�nima entre el Emboscador y el jugador

    void Start()
    {
        // Busca el objeto con la etiqueta "Player" para asignar el transform del jugador
        jugador = GameObject.FindGameObjectWithTag("Player").transform;

        // Aseg�rate de que el jugador sea encontrado
        if (jugador == null)
        {
            Debug.LogError("Jugador no encontrado. Aseg�rate de que el jugador tenga la etiqueta 'Player'.");
            return;
        }

        // Inicia el proceso de aparici�n
        InvokeRepeating("AparecerEmboscador", 0f, tiempoDeEspera);
    }

    void AparecerEmboscador()
    {
        // Debug para comprobar que el c�digo est� siendo llamado
        Debug.Log("Emboscador aparece");

        // Obtiene una posici�n accesible y visible cerca del jugador
        Vector3 posicionAparicion = ObtenerPosicionVisibleFrenteAlJugador();

        // Teletransporta al Emboscador a la nueva posici�n
        transform.position = posicionAparicion;

        // Debug para ver la posici�n calculada
        Debug.Log("Posici�n del Emboscador: " + transform.position);

        // Hace que el Emboscador se quede durante 3 segundos en ese lugar
        Invoke("DesaparecerEmboscador", tiempoDePermanencia);
    }

    void DesaparecerEmboscador()
    {
        // Desaparece o se teletransporta a otro punto
        transform.position = new Vector3(0, -5, 0);  // Cambia esta posici�n por la que prefieras

        // Debug para confirmar desaparici�n
        Debug.Log("Emboscador desaparece");
    }

    Vector3 ObtenerPosicionVisibleFrenteAlJugador()
    {
        // Obtiene la direcci�n hacia la que est� mirando el jugador
        Vector3 direccionDelJugador = jugador.forward; // Esto es lo que el jugador est� mirando

        // Calcula una posici�n un poco m�s adelante del jugador, dentro de su campo de visi�n
        Vector3 posicionFrenteAlJugador = jugador.position + direccionDelJugador * Random.Range(distanciaCercana, rangoDeVision);

        // Asegurarse de que la altura (Y) sea la misma que la del jugador
        posicionFrenteAlJugador.y = jugador.position.y;

        return posicionFrenteAlJugador;
    }
}
