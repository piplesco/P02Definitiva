using UnityEngine;
using UnityEngine.UI; // Importar para usar UI

public class Texto : MonoBehaviour
{
  public float speed;
  public float angle;
  public Vector3 direction;

  public bool puedeAbrir;

 

    void Start()
    {
      angle = transform.eulerAngles.y;
    }

    void Update()
    {
       if(Mathf.Round(transform.eulerAngles.y) != angle){ 
       transform.Rotate(direction * speed);
       }
       if(Input.GetButtonDown("f") && puedeAbrir == true){
       angle= 80;
       direction= Vector3.up;
       }
    }

    void OnTriggerStay(Collider other){
    
    if(other.gameObject.tag == "Player"){
    puedeAbrir = true;
    }
    }
    void OnTriggerExit(Collider other){
    
    if(other.gameObject.tag == "Player"){
    puedeAbrir = false;
    }
    }

    
}

