using UnityEngine;

public class EnemigoZombieConAtaque : MonoBehaviour
{
    public float velocidadMovimiento = 2f; // Velocidad de caminar
    public float tiempoMovimiento = 4f;   // Tiempo caminando recto
    public float anguloGiro = 30f;        // �ngulo de giro
    public float velocidadGiro = 100f;    // Velocidad del giro en grados por segundo
    public float distanciaDeteccion = 5f; // Distancia para detectar al jugador
    public float dano = 10f;              // Da�o que inflige al jugador
    public float tiempoEntreAtaques = 1f; // Tiempo entre ataques
    private float tiempoAtaqueActual = 0f;

    private float cronometro;             // Cron�metro para medir el tiempo
    private bool girando = false;         // Estado del giro
    private Quaternion rotacionObjetivo;  // Rotaci�n hacia la que girar
    private Rigidbody rb;                 // Rigidbody para el movimiento

    private Transform jugador;            // Referencia al jugador
    private JugadorSalud jugadorSalud;    // Referencia al script de salud del jugador
    private Animator animator;            // Animator para las animaciones del enemigo

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtener el Rigidbody del enemigo
        animator = GetComponent<Animator>(); // Obtener el Animator
        jugador = GameObject.FindGameObjectWithTag("Player").transform; // Buscar al jugador por etiqueta
        jugadorSalud = jugador.GetComponent<JugadorSalud>(); // Obtener el script de salud del jugador
        cronometro = tiempoMovimiento;  // Inicializar el cron�metro
        rotacionObjetivo = transform.rotation; // Inicializar la rotaci�n objetivo
    }

    void FixedUpdate()
    {
        // Calcular la distancia al jugador
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        // Si el jugador est� dentro de la distancia de detecci�n, perseguirlo y atacarlo
        if (distanciaAlJugador <= distanciaDeteccion)
        {
            PerseguirYAtacar(distanciaAlJugador);
        }
        else
        {
            // Realizar el movimiento de divagaci�n
            if (girando)
            {
                SuavizarGiro();
            }
            else
            {
                MoverRecto();
                cronometro -= Time.fixedDeltaTime;

                if (cronometro <= 0)
                {
                    ComenzarGiro();
                    cronometro = tiempoMovimiento; // Reiniciar el cron�metro
                }
            }
        }
    }

    void PerseguirYAtacar(float distancia)
    {
        // Girar hacia el jugador
        Vector3 direccion = jugador.position - transform.position;
        direccion.y = 0; // Mantener la rotaci�n en el plano horizontal
        Quaternion rotacion = Quaternion.LookRotation(direccion);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * velocidadGiro);

        // Si est� lo suficientemente cerca, atacar
        if (distancia <= 2f)
        {
            animator.SetBool("Atacar", true);

            // Controlar tiempo entre ataques
            tiempoAtaqueActual += Time.deltaTime;
            if (tiempoAtaqueActual >= tiempoEntreAtaques)
            {
                tiempoAtaqueActual = 0f;
                jugadorSalud.RecibirDano(dano); // Restar vida al jugador
            }
        }
        else
        {
            // Caminar hacia el jugador
            animator.SetBool("Atacar", false);
            rb.MovePosition(rb.position + transform.forward * velocidadMovimiento * Time.deltaTime);
        }
    }

    void MoverRecto()
    {
        rb.MovePosition(rb.position + transform.forward * velocidadMovimiento * Time.fixedDeltaTime);
    }

    void ComenzarGiro()
    {
        // Alternar direcci�n del giro
        float direccionGiro = Random.Range(0, 2) == 0 ? -anguloGiro : anguloGiro;

        // Calcular la nueva rotaci�n objetivo
        rotacionObjetivo = Quaternion.Euler(0, transform.eulerAngles.y + direccionGiro, 0);
        girando = true;
    }

    void SuavizarGiro()
    {
        // Rotar suavemente hacia la rotaci�n objetivo
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionObjetivo, velocidadGiro * Time.fixedDeltaTime);

        // Verificar si hemos alcanzado la rotaci�n objetivo
        if (Quaternion.Angle(transform.rotation, rotacionObjetivo) < 0.1f)
        {
            transform.rotation = rotacionObjetivo; // Asegurar que la rotaci�n sea exacta
            girando = false; // Terminar el giro
        }
    }
}
