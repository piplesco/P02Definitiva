using UnityEngine;

public class EnemigoZombie : MonoBehaviour
{
    public Animator animator;
    public float velocidadMovimiento = 5f;  // Velocidad de caminar (ahora más rápida)
    public float distanciaDeteccion = 10f;  // Distancia para detectar al jugador
    public Transform jugador;  // El Transform del jugador
    public float velocidadPersecucion = 5f;  // Velocidad de persecución al jugador
    public Rigidbody rb;  // Rigidbody del zombie
    private bool estaPersiguiendo = false;

    // Variables para el movimiento controlado
    public float intervaloGiro = 5f;  // Intervalo de tiempo para cambiar dirección
    private float cronometroGiro = 0f;
    private Vector3 direccionMovimiento;
    private bool estaDetenido = false;  // Para verificar si el zombie está detenido

    // Variables para la detección de obstáculos
    public float distanciaRaycast = 2f;  // Distancia del raycast para detectar obstáculos
    public LayerMask capaObstaculos;  // Capas en las que el raycast detectará obstáculos

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;  // Buscar al jugador por su etiqueta
        animator = GetComponent<Animator>();  // Obtener el Animator del zombie
        rb = GetComponent<Rigidbody>();  // Obtener el Rigidbody del zombie

        // Inicializar dirección de movimiento hacia adelante
        direccionMovimiento = transform.forward;
    }

    void Update()
    {
        // Detectar si el zombie está cerca del jugador
        float distancia = Vector3.Distance(transform.position, jugador.position);  // Calcular distancia entre el zombie y el jugador

        if (distancia < distanciaDeteccion)
        {
            estaPersiguiendo = true;  // Si está cerca, empieza a perseguir
        }
        else
        {
            estaPersiguiendo = false;  // Si se aleja, detiene la persecución
        }

        if (estaPersiguiendo)
        {
            PerseguirJugador();  // Si está persiguiendo, lo sigue
            animator.SetBool("Atacar", true);  // Activar la animación de ataque
        }
        else
        {
            MoverZombie();  // Si no está persiguiendo, camina en una dirección controlada
            animator.SetBool("Atacar", false);  // Desactivar la animación de ataque
        }
    }

    void MoverZombie()
    {
        cronometroGiro += Time.deltaTime;

        // Verificar si han pasado los 5 segundos
        if (cronometroGiro >= intervaloGiro && !estaDetenido)
        {
            // Detener el zombie
            animator.SetBool("Andar", false);  // Desactivar la animación de caminar
            estaDetenido = true;  // Marcar que el zombie está detenido

            // Resetear el cronómetro de giro
            cronometroGiro = 0f;
        }

        if (estaDetenido)
        {
            // Esperar un momento antes de empezar a caminar de nuevo
            // Después de detenerse, realizar el giro y activar el movimiento
            if (cronometroGiro >= 1f)  // Espera 1 segundo antes de girar
            {
                // Girar el zombie a una nueva dirección aleatoria
                float giroAleatorio = Random.Range(-90f, 90f);  // Elige un ángulo aleatorio entre -90 y 90 grados
                direccionMovimiento = Quaternion.Euler(0, giroAleatorio, 0) * transform.forward;  // Rotar la dirección hacia el nuevo ángulo

                // Volver a caminar en la nueva dirección
                estaDetenido = false;  // El zombie ya no está detenido
                animator.SetBool("Andar", true);  // Activar la animación de caminar
            }
        }

        if (!estaDetenido)
        {
            // Detectar obstáculos con un raycast
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direccionMovimiento, out hit, distanciaRaycast, capaObstaculos))
            {
                // Si detecta un obstáculo, girar en la dirección opuesta
                float giroAleatorio = Random.Range(-90f, 90f);  // Girar aleatoriamente
                direccionMovimiento = Quaternion.Euler(0, giroAleatorio, 0) * transform.forward;  // Cambiar dirección
            }

            // Girar suavemente sobre su eje antes de moverse
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);  // Calcular rotación hacia la nueva dirección
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionObjetivo, 180 * Time.deltaTime);  // Rotar suavemente

            // Mover al zombie en la dirección en la que está mirando
            Vector3 movimiento = direccionMovimiento * velocidadMovimiento * Time.deltaTime;  // Mover en la dirección calculada
            rb.MovePosition(rb.position + movimiento);  // Mover el zombie usando el Rigidbody
        }
    }

    void PerseguirJugador()
    {
        // Mover hacia el jugador
        Vector3 direccion = jugador.position - transform.position;
        direccion.y = 0;  // Mantener la rotación en el plano horizontal (evitar cambios de altura)
        Quaternion rotacion = Quaternion.LookRotation(direccion);  // Calcular rotación hacia el jugador
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * 5);  // Rotar suavemente hacia el jugador

        // Mover hacia el jugador
        Vector3 movimiento = direccion.normalized * velocidadPersecucion * Time.deltaTime;
        rb.MovePosition(rb.position + movimiento);  // Mover el zombie usando el Rigidbody

        animator.SetBool("Andar", true);  // Activar animación de caminar
    }
}
