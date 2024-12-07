using UnityEngine;

public class EnemigoZombie : MonoBehaviour
{
    public Animator animator;
    public float velocidadMovimiento = 5f;  // Velocidad de caminar (ahora m�s r�pida)
    public float distanciaDeteccion = 10f;  // Distancia para detectar al jugador
    public Transform jugador;  // El Transform del jugador
    public float velocidadPersecucion = 5f;  // Velocidad de persecuci�n al jugador
    public Rigidbody rb;  // Rigidbody del zombie
    private bool estaPersiguiendo = false;

    // Variables para el movimiento controlado
    public float intervaloGiro = 5f;  // Intervalo de tiempo para cambiar direcci�n
    private float cronometroGiro = 0f;
    private Vector3 direccionMovimiento;
    private bool estaDetenido = false;  // Para verificar si el zombie est� detenido

    // Variables para la detecci�n de obst�culos
    public float distanciaRaycast = 2f;  // Distancia del raycast para detectar obst�culos
    public LayerMask capaObstaculos;  // Capas en las que el raycast detectar� obst�culos

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;  // Buscar al jugador por su etiqueta
        animator = GetComponent<Animator>();  // Obtener el Animator del zombie
        rb = GetComponent<Rigidbody>();  // Obtener el Rigidbody del zombie

        // Inicializar direcci�n de movimiento hacia adelante
        direccionMovimiento = transform.forward;
    }

    void Update()
    {
        // Detectar si el zombie est� cerca del jugador
        float distancia = Vector3.Distance(transform.position, jugador.position);  // Calcular distancia entre el zombie y el jugador

        if (distancia < distanciaDeteccion)
        {
            estaPersiguiendo = true;  // Si est� cerca, empieza a perseguir
        }
        else
        {
            estaPersiguiendo = false;  // Si se aleja, detiene la persecuci�n
        }

        if (estaPersiguiendo)
        {
            PerseguirJugador();  // Si est� persiguiendo, lo sigue
            animator.SetBool("Atacar", true);  // Activar la animaci�n de ataque
        }
        else
        {
            MoverZombie();  // Si no est� persiguiendo, camina en una direcci�n controlada
            animator.SetBool("Atacar", false);  // Desactivar la animaci�n de ataque
        }
    }

    void MoverZombie()
    {
        cronometroGiro += Time.deltaTime;

        // Verificar si han pasado los 5 segundos
        if (cronometroGiro >= intervaloGiro && !estaDetenido)
        {
            // Detener el zombie
            animator.SetBool("Andar", false);  // Desactivar la animaci�n de caminar
            estaDetenido = true;  // Marcar que el zombie est� detenido

            // Resetear el cron�metro de giro
            cronometroGiro = 0f;
        }

        if (estaDetenido)
        {
            // Esperar un momento antes de empezar a caminar de nuevo
            // Despu�s de detenerse, realizar el giro y activar el movimiento
            if (cronometroGiro >= 1f)  // Espera 1 segundo antes de girar
            {
                // Girar el zombie a una nueva direcci�n aleatoria
                float giroAleatorio = Random.Range(-90f, 90f);  // Elige un �ngulo aleatorio entre -90 y 90 grados
                direccionMovimiento = Quaternion.Euler(0, giroAleatorio, 0) * transform.forward;  // Rotar la direcci�n hacia el nuevo �ngulo

                // Volver a caminar en la nueva direcci�n
                estaDetenido = false;  // El zombie ya no est� detenido
                animator.SetBool("Andar", true);  // Activar la animaci�n de caminar
            }
        }

        if (!estaDetenido)
        {
            // Detectar obst�culos con un raycast
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direccionMovimiento, out hit, distanciaRaycast, capaObstaculos))
            {
                // Si detecta un obst�culo, girar en la direcci�n opuesta
                float giroAleatorio = Random.Range(-90f, 90f);  // Girar aleatoriamente
                direccionMovimiento = Quaternion.Euler(0, giroAleatorio, 0) * transform.forward;  // Cambiar direcci�n
            }

            // Girar suavemente sobre su eje antes de moverse
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);  // Calcular rotaci�n hacia la nueva direcci�n
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionObjetivo, 180 * Time.deltaTime);  // Rotar suavemente

            // Mover al zombie en la direcci�n en la que est� mirando
            Vector3 movimiento = direccionMovimiento * velocidadMovimiento * Time.deltaTime;  // Mover en la direcci�n calculada
            rb.MovePosition(rb.position + movimiento);  // Mover el zombie usando el Rigidbody
        }
    }

    void PerseguirJugador()
    {
        // Mover hacia el jugador
        Vector3 direccion = jugador.position - transform.position;
        direccion.y = 0;  // Mantener la rotaci�n en el plano horizontal (evitar cambios de altura)
        Quaternion rotacion = Quaternion.LookRotation(direccion);  // Calcular rotaci�n hacia el jugador
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * 5);  // Rotar suavemente hacia el jugador

        // Mover hacia el jugador
        Vector3 movimiento = direccion.normalized * velocidadPersecucion * Time.deltaTime;
        rb.MovePosition(rb.position + movimiento);  // Mover el zombie usando el Rigidbody

        animator.SetBool("Andar", true);  // Activar animaci�n de caminar
    }
}
