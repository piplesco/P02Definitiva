using UnityEngine;

public class JugadorSalud : MonoBehaviour
{
    public float salud = 100f;  // Salud del jugador

    public void RecibirDano(float dano)
    {
        salud -= dano;  // Resta el da�o a la salud
        if (salud <= 0)
        {
            Muerte();  // Llamar a la funci�n de muerte si la salud llega a cero
        }
    }

    void Muerte()
    {
        // Aqu� puedes agregar lo que suceda cuando el jugador muere (por ejemplo, reiniciar la escena o mostrar una animaci�n de muerte)
        Debug.Log("El jugador ha muerto");
    }
}
