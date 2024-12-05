using UnityEngine;

public class MovimientoPersonaje : MonoBehaviour
{
    public CharacterController Controlador;
    public Transform Camara;
    public float Velocidad = 15f;
    public float AlturaSalto = 2f;
    public float Gravedad = -9.81f;

    private Vector3 velocidad;
    private bool enElSuelo;

    public AudioSource audioSource;
    public AudioClip pasosRapidos;  // Sonido de caminar
    public AudioClip pasosLentos;   // Sonido de correr

    private bool corriendo = false;
    private bool caminando = false;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        enElSuelo = Controlador.isGrounded;

        if (enElSuelo && velocidad.y < 0)
        {
            velocidad.y = -2f; // Mantener al personaje pegado al suelo
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Movimiento basado en la cámara
        Vector3 adelante = Camara.forward;
        Vector3 derecha = Camara.right;

        adelante.y = 0f;
        derecha.y = 0f;

        Vector3 mover = (derecha * x + adelante * z).normalized * Velocidad;
        Controlador.Move(mover * Time.deltaTime);

        // Salto
        if (Input.GetButtonDown("Jump") && enElSuelo)
        {
            velocidad.y = Mathf.Sqrt(AlturaSalto * -2f * Gravedad);
        }

        // Aplicar gravedad
        velocidad.y += Gravedad * Time.deltaTime;

        // Aplicar movimiento vertical
        Controlador.Move(velocidad * Time.deltaTime);

        // Detectar movimiento
        if (mover.magnitude > 0)
        {
            // Activar correr o caminar
            if (Input.GetKey(KeyCode.LeftShift))
            {
                corriendo = true;
                caminando = false;
                Velocidad = 8f;
                // Reproducir sonido de correr si no está sonando
                if (!audioSource.isPlaying || audioSource.clip != pasosLentos)
                {
                    audioSource.clip = pasosLentos;
                    audioSource.Play();
                }
            }
            else
            {
                corriendo = false;
                caminando = true;
                Velocidad = 15f;
                // Reproducir sonido de caminar si no está sonando
                if (!audioSource.isPlaying || audioSource.clip != pasosRapidos)
                {
                    audioSource.clip = pasosRapidos;
                    audioSource.Play();
                }
            }
        }
        else
        {
            // Detener el sonido cuando no haya movimiento
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}

