using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CubeCollision : MonoBehaviour
{
    Cube cube;

    private void Awake()
    {
        cube = GetComponent<Cube>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Cube otherCube = collision.gameObject.GetComponent<Cube>();
        //diðer küplere temas edip etmeme durumunu kontrol et

        if (otherCube != null && cube.CubeID < otherCube.CubeID)      //iki küp de ayný numaraya mý sahip ?
        {
            if (cube.CubeNumber == otherCube.CubeNumber)
           {
                Debug.Log("HIT: " + cube.CubeNumber);
                Vector3 contactPoint = collision.contacts[0].point; 
            

                //Cubespawner'da küp sayýsýnýn maksimum sayýdan az olup olmadýðýný kontrol et

                if (otherCube.CubeNumber < CubeSpawner.Instance.maxCubeNumber)
                {
                    Cube newCube = CubeSpawner.Instance.Spawn(cube.CubeNumber * 2, contactPoint + Vector3.up*1.6f);
                    float pushForce = 2.5f;
                    newCube.CubeRigidbody.AddForce(new Vector3(0, .3f, 1f)*pushForce, ForceMode.Impulse);

                    //tork ekleme
                    float randomValue = Random.Range(-20f, 20f);
                    Vector3 randomDirection = Vector3.one * randomValue;
                    newCube.CubeRigidbody.AddTorque(randomDirection);

        
                }
                //patlama
                Collider[] surroundedCubes = Physics.OverlapSphere(contactPoint, 2f);
                float explosionForce = 400;
                float explosionRadius = 1.5f;

                foreach (Collider coll in surroundedCubes)
                {
                    if (coll.attachedRigidbody != null)
                    {
                        coll.attachedRigidbody.AddExplosionForce(explosionForce, contactPoint, explosionRadius);
                    }
                }
                FX.Instance.PlayCubeExplosionFX(contactPoint, cube.CubeColor);

                //Destroy the two cubes
                CubeSpawner.Instance.DestroyCube(cube);
                CubeSpawner.Instance.DestroyCube(otherCube);
            }
        }
    }
}
