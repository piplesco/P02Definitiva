using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo1 : MonoBehaviour
{
    public int rutina; // Rutina aleatoria
    public float cronometro; // Tiempo para cambiar rutina
    public Animator Ani; // Referencia al Animator
    public Quaternion angulo; // Ángulo de rotación
    public float grado; // Grado para calcular rotación

    // Start is called before the first frame update
    void Start()
    {
        // Obtiene el Animator del objeto
        Ani = GetComponent<Animator>();
    }

    // Comportamiento del enemigo según rutina
    public void Comportamiento_Enemigo()
    {
        cronometro += 1 * Time.deltaTime;

        // Cambia rutina cada 4 segundos
        if (cronometro >= 4)
        {
            rutina = Random.Range(0, 3); // Rutina aleatoria entre 0 y 2
            cronometro = 0;
        }

        // Lógica de comportamiento según la rutina
        switch (rutina)
        {
            case 0: // Enemigo quieto
                Ani.SetBool("walk", false);
                break;

            case 1: // Gira hacia una dirección aleatoria
                grado = Random.Range(0, 360);
                angulo = Quaternion.Euler(0, grado, 0);
                rutina++; // Pasa automáticamente a la rutina 2
                break;

            case 2: // Camina hacia la dirección girada
                transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 50 * Time.deltaTime);
                transform.Translate(Vector3.forward * 1 * Time.deltaTime);
                Ani.SetBool("walk", true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Comportamiento_Enemigo();
    }
}
