using UnityEngine;

public class EnemigoZombie : MonoBehaviour
{
    public Animator animator;
    public float velocidadMovimiento = 100f;  // Velocidad de caminar
    public float distanciaDeteccion = 10f;  // Distancia para detectar al jugador
    public float distanciaAtaque = 2f;  // Distancia m�nima para atacar
    public Transform jugador;  // Transform del jugador
    public float velocidadPersecucion = 5f;  // Velocidad de persecuci�n
    public Rigidbody rb;  // Rigidbody del zombie
    private bool estaPersiguiendo = false;

    // Variables para el movimiento controlado
    public float intervaloGiro = 5f;  // Intervalo de tiempo para cambiar direcci�n
    private float cronometroGiro = 0f;
    private Vector3 direccionMovimiento;
    private bool estaDetenido = false;  // Verificar si el zombie est� detenido

    // Variables para detecci�n de obst�culos
    public float distanciaRaycast = 2f;  // Distancia del raycast
    public LayerMask capaObstaculos;  // Capas para detectar obst�culos

    private bool estaAtacando = false;  // Estado de ataque

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;  // Buscar jugador por etiqueta
        animator = GetComponent<Animator>();  // Obtener Animator
        rb = GetComponent<Rigidbody>();  // Obtener Rigidbody
        direccionMovimiento = transform.forward;  // Direcci�n inicial
    }

    void Update()
    {
        // Calcular distancia con el jugador
        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia < distanciaAtaque)
        {
            AtacarJugador();  // Si est� cerca, ataca
        }
        else if (distancia < distanciaDeteccion)
        {
            estaAtacando = false;  // Dejar de atacar si no est� tan cerca
            animator.SetBool("IsAttacking", false);  // Desactivar animaci�n de ataque
            estaPersiguiendo = true;  // Comienza a perseguir
        }
        else
        {
            estaAtacando = false;  // Dejar de atacar si est� muy lejos
            animator.SetBool("IsAttacking", false);  // Desactivar animaci�n de ataque
            estaPersiguiendo = false;  // Dejar de perseguir
        }

        if (estaPersiguiendo && !estaAtacando)
        {
            PerseguirJugador();  // Persigue al jugador
        }
        else if (!estaPersiguiendo && !estaAtacando)
        {
            MoverZombie();  // Movimiento aleatorio controlado
        }
    }

    void MoverZombie()
    {
        cronometroGiro += Time.deltaTime;

        if (cronometroGiro >= intervaloGiro && !estaDetenido)
        {
            animator.SetBool("IsWalking", false);
            estaDetenido = true;
            cronometroGiro = 0f;
        }

        if (estaDetenido)
        {
            if (cronometroGiro >= 1f)
            {
                float giroAleatorio = Random.Range(-90f, 90f);
                direccionMovimiento = Quaternion.Euler(0, giroAleatorio, 0) * transform.forward;
                estaDetenido = false;
                animator.SetBool("IsWalking", true);
            }
        }

        if (!estaDetenido)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direccionMovimiento, out hit, distanciaRaycast, capaObstaculos))
            {
                float giroAleatorio = Random.Range(-90f, 90f);
                direccionMovimiento = Quaternion.Euler(0, giroAleatorio, 0) * transform.forward;
            }

            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionObjetivo, 180 * Time.deltaTime);

            Vector3 movimiento = direccionMovimiento * velocidadMovimiento * Time.deltaTime;
            rb.MovePosition(rb.position + movimiento);
        }
    }

    void PerseguirJugador()
    {
        Vector3 direccion = jugador.position - transform.position;
        direccion.y = 0;
        Quaternion rotacion = Quaternion.LookRotation(direccion);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * 5);

        Vector3 movimiento = direccion.normalized * velocidadPersecucion * Time.deltaTime;
        rb.MovePosition(rb.position + movimiento);

        animator.SetBool("IsWalking", true);
    }

    void AtacarJugador()
    {
        if (!estaAtacando)  // Solo atacar si no est� ya atacando
        {
            estaAtacando = true;  // Marcar estado de ataque
            animator.SetBool("IsWalking", false);  // Detener animaci�n de caminar
            animator.SetBool("IsAttacking", true);  // Activar animaci�n de ataque

            // Opcional: detener el movimiento mientras ataca
            rb.velocity = Vector3.zero;

            // Salir del estado de ataque despu�s de la animaci�n
            Invoke(nameof(DetenerAtaque), 1.5f);  // Ajusta este tiempo seg�n la duraci�n de tu animaci�n
        }
    }

    void DetenerAtaque()
    {
        estaAtacando = false;
        animator.SetBool("IsAttacking", false);
    }
}
