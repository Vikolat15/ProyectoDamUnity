
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoJugador : MonoBehaviour
{
   [SerializeField] private Transform controladorDisparo;
   [SerializeField] private GameObject bala;

   private void Update(){
        if (Input.GetKeyDown(KeyCode.P)){
            Shoot();
        }
   }

       public void Shoot(){

        Vector3 direccion;
        if(transform.localScale.x == 1.0f){ 
            direccion = Vector2.left;
        } else {
            direccion = Vector2.right;
        }

        Vector3 spawnBala = transform.position + new Vector3(0f, 0.8f, 0f);
        spawnBala.z = 0f;

        GameObject bullet = Instantiate(BulletPrefab, spawnBala, Quaternion.identity);
    
        bullet.GetComponent<Bala>().SetDireccion(direccion);
        
        Animator.SetTrigger("shooting");
    }
}*/
