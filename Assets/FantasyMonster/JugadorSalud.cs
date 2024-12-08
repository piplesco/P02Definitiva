using UnityEngine;

public class JugadorSalud : MonoBehaviour
{
    public float salud = 100f;  // Salud del jugador

    public void RecibirDano(float dano)
    {
        salud -= dano;  // Resta el daño a la salud
        if (salud <= 0)
        {
            Muerte();  // Llamar a la función de muerte si la salud llega a cero
        }
    }

    void Muerte()
    {
        // Aquí puedes agregar lo que suceda cuando el jugador muere (por ejemplo, reiniciar la escena o mostrar una animación de muerte)
        Debug.Log("El jugador ha muerto");
    }
}
