using UnityEngine;

public class Mirada_Camara : MonoBehaviour
{
    public float Velocidad = 100f;
    private float RotacionX = 0f;
    private float RotacionY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float MauseX = Input.GetAxis("Mouse X") * Velocidad * Time.deltaTime;
        float MauseY = Input.GetAxis("Mouse Y") * Velocidad * Time.deltaTime;

        // Rotación vertical
        RotacionX -= MauseY;
        RotacionX = Mathf.Clamp(RotacionX, -90f, 90f);

        // Rotación horizontal
        RotacionY += MauseX;

        // Aplicar las rotaciones
        transform.localRotation = Quaternion.Euler(RotacionX, RotacionY, 0f);
    }
}
