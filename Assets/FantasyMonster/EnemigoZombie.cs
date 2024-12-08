using UnityEngine;

public class EnemigoZombieConAtaque : MonoBehaviour
{
    public float velocidadMovimiento = 2f; // Velocidad de caminar
    public float tiempoMovimiento = 4f;   // Tiempo caminando recto
    public float anguloGiro = 30f;        // Ángulo de giro
    public float velocidadGiro = 100f;    // Velocidad del giro en grados por segundo
    public float distanciaDeteccion = 5f; // Distancia para detectar al jugador
    public float dano = 10f;              // Daño que inflige al jugador
    public float tiempoEntreAtaques = 1f; // Tiempo entre ataques
    private float tiempoAtaqueActual = 0f;

    private float cronometro;             // Cronómetro para medir el tiempo
    private bool girando = false;         // Estado del giro
    private Quaternion rotacionObjetivo;  // Rotación hacia la que girar
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
        cronometro = tiempoMovimiento;  // Inicializar el cronómetro
        rotacionObjetivo = transform.rotation; // Inicializar la rotación objetivo
    }

    void FixedUpdate()
    {
        // Calcular la distancia al jugador
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        // Si el jugador está dentro de la distancia de detección, perseguirlo y atacarlo
        if (distanciaAlJugador <= distanciaDeteccion)
        {
            PerseguirYAtacar(distanciaAlJugador);
        }
        else
        {
            // Realizar el movimiento de divagación
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
                    cronometro = tiempoMovimiento; // Reiniciar el cronómetro
                }
            }
        }
    }

    void PerseguirYAtacar(float distancia)
    {
        // Girar hacia el jugador
        Vector3 direccion = jugador.position - transform.position;
        direccion.y = 0; // Mantener la rotación en el plano horizontal
        Quaternion rotacion = Quaternion.LookRotation(direccion);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * velocidadGiro);

        // Si está lo suficientemente cerca, atacar
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
        // Alternar dirección del giro
        float direccionGiro = Random.Range(0, 2) == 0 ? -anguloGiro : anguloGiro;

        // Calcular la nueva rotación objetivo
        rotacionObjetivo = Quaternion.Euler(0, transform.eulerAngles.y + direccionGiro, 0);
        girando = true;
    }

    void SuavizarGiro()
    {
        // Rotar suavemente hacia la rotación objetivo
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionObjetivo, velocidadGiro * Time.fixedDeltaTime);

        // Verificar si hemos alcanzado la rotación objetivo
        if (Quaternion.Angle(transform.rotation, rotacionObjetivo) < 0.1f)
        {
            transform.rotation = rotacionObjetivo; // Asegurar que la rotación sea exacta
            girando = false; // Terminar el giro
        }
    }
}
