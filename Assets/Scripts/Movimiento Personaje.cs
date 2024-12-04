using System.Collections;
using System.Collections.Generic;
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

    void Update()
    {
        // Comprobar si está en el suelo con un margen de tolerancia
        enElSuelo = Controlador.isGrounded;

        if (enElSuelo && velocidad.y < 0)
        {
            velocidad.y = -2f; // Mantener al personaje pegado al suelo
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Movimiento horizontal basado en la cámara
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
    }
}
