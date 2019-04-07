using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletLifeTime = 5f;
    public Vector3 bulletTrajectory;

    float damage = 1f;
    float bulletSpeed = 500f;

    void Start()
    {
        // Move the bullet as it is created
        MoveBullet();

        // Destroy bullet after its life time
        Invoke("DestroyBullet", bulletLifeTime);
    }

    public void MoveBullet()
    {
        // Get the reference of the ship, so that we can get its up direction all the time.
        GameObject ship = GameObject.FindGameObjectWithTag("Ship");

        // Set the bullet trajectory
        bulletTrajectory = ship.transform.up;

        // Add force to the bullet
        GetComponent<Rigidbody2D>().AddForce(bulletTrajectory * bulletSpeed);
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

}
