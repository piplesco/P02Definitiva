using UnityEngine;


public class EnemigoZombie : MonoBehaviour
{
    public Animator animator;
    public float velocidadMovimiento = 3f;  // Velocidad de caminar
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
            estaPersiguiendo = true;  // Comienza la persecuci�n
        }
        else
        {
            estaPersiguiendo = false;  // Detiene la persecuci�n si se aleja
        }

        if (estaPersiguiendo)
        {
            PerseguirJugador();  // Mueve hacia el jugador
            if (distancia <= 2f)  // Si est� cerca del jugador (por ejemplo, a 2 metros)
            {
                animator.SetBool("Atacar", true);  // Activar la animaci�n de ataque
            }
            else
            {
                animator.SetBool("Atacar", false);  // Si no est� lo suficientemente cerca, desactivar la animaci�n de ataque
            }
        }
        else
        {
            MoverZombie();  // Mueve al zombie de manera controlada
            animator.SetBool("Atacar", false);  // Desactivar la animaci�n de ataque cuando no est� persiguiendo
        }
    }

    void MoverZombie()
    {
        cronometroGiro += Time.deltaTime;

        // Comprobar si han pasado los 5 segundos
        if (cronometroGiro >= intervaloGiro && !estaDetenido)
        {
            animator.SetBool("Andar", false);  // Desactivar animaci�n de caminar
            estaDetenido = true;  // Marcar al zombie como detenido
            cronometroGiro = 0f;
        }

        if (estaDetenido)
        {
            // Esperar un momento antes de empezar a caminar de nuevo
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
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionObjetivo, 180 * Time.deltaTime);

            Vector3 movimiento = direccionMovimiento * velocidadMovimiento * Time.deltaTime;
            rb.MovePosition(rb.position + movimiento);
        }
    }

    void PerseguirJugador()
    {
        Vector3 direccion = jugador.position - transform.position;
        direccion.y = 0;  // Mantener la rotaci�n en el plano horizontal (evitar cambios de altura)
        Quaternion rotacion = Quaternion.LookRotation(direccion);  // Calcular rotaci�n hacia el jugador
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * 5);  // Rotar suavemente hacia el jugador

        Vector3 movimiento = direccion.normalized * velocidadPersecucion * Time.deltaTime;
        rb.MovePosition(rb.position + movimiento);  // Mover el zombie usando el Rigidbody

        animator.SetBool("Andar", true);  // Activar animaci�n de caminar
    }
}
