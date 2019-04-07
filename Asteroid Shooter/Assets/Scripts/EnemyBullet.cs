using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    public Vector3 direction; // The enemy ship will pass this information to the bullet

    EffectSpawner effectSpawner;
    float lifeTime = 3.0f;
    float speed = 300.0f;
    Rigidbody2D rb;
    Transform target;

	void Start ()
    {
        // Get the reference to the rigidbody component
        rb = GetComponent<Rigidbody2D>();

        //
        effectSpawner = FindObjectOfType<EffectSpawner>();

        //
        target = GameObject.FindGameObjectWithTag("Ship").transform;

        // Move the bullet
        MoveBullet();

        // If it fails to hit the player ship, then when its lifetime is up, destroy it
        Invoke("DestroyBullet", lifeTime);
    }
	
    void MoveBullet()
    {
        //direction = target.position - transform.position;
        direction = Vector3.Normalize(target.position - transform.position);

        rb.AddForce(direction * speed);
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ship")
        {
            effectSpawner.AsteroidsBlowingUp(collision.gameObject.transform.position, effectSpawner.playerExplosion, 1);
            Destroy(collision.gameObject);
            FindObjectOfType<LevelManager>().OnPlayerDeath();
            DestroyBullet();
        }
    }
}
