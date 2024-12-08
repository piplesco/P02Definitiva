using UnityEngine;
using UnityEngine.UI;

public class Barradevida : MonoBehaviour
{
    public Image barraDeVida;  // Imagen de la barra de vida
    private JugadorSalud jugadorSalud;  // Referencia al script JugadorSalud

    void Start()
    {
        jugadorSalud = GameObject.FindGameObjectWithTag("Player").GetComponent<JugadorSalud>();
    }

    void Update()
    {
        if (jugadorSalud != null)
        {
            barraDeVida.fillAmount = jugadorSalud.salud / 100f;  // Actualizar la barra en base a la salud
        }
    }
}
