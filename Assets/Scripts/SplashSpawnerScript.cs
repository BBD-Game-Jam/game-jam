using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SplashSpawnerScript : MonoBehaviour
{
    public GameObject entityToSpawn;

      public SpriteShapeController terrainShape;

    private bool canSpawn = true;

    void Start(){
        StartCoroutine(CanSpawn());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Environment") && canSpawn)
        {
            Vector2 collisionPoint = collision.GetContact(0).point;
            Vector3 spawnPosition = new Vector3(collisionPoint.x, collisionPoint.y, 1);
            
            canSpawn = false;

            Vector2 collisionNormal = collision.contacts[0].normal;


            Vector2 spriteVelocity = GetComponent<Rigidbody2D>().velocity;
            float collisionAngle = Vector2.Angle(collisionNormal, spriteVelocity);
           // Debug.Log("Collision Angle: " + collisionAngle + " degrees");
            Instantiate(entityToSpawn, spawnPosition, Quaternion.Euler(0,0,collisionAngle - 90f));
        }
    }

    private IEnumerator CanSpawn(){
        while(true){
        yield return new WaitForSeconds(2f);
        canSpawn = true;
}    }




}
