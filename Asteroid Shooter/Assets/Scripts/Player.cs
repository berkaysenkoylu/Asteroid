using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed = 10.0f;
    public float angleStep = 5.0f;
    public GameObject bullet;
    public Transform muzzle;
    public float fireRate = 0.25f;
    public AudioClip[] audioClips;

    float nextFire;
    Rigidbody2D rb;
    float angle = 0f;
    Vector2 spriteSizes;
    Vector2 force;
    AudioSource playerAudio;
    LevelManager levelManager;
    

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        levelManager = FindObjectOfType<LevelManager>();
        spriteSizes = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x, GetComponent<SpriteRenderer>().bounds.size.y);
        playerAudio = GetComponent<AudioSource>();
    }
	
	void FixedUpdate ()
    {
        Movement();

        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFire)
        {
            FireBullet();

            // Save the last fired time
            nextFire = Time.time + fireRate;
        }
    }

    void Movement()
    {
        // Rotation
        if (Input.GetKey(KeyCode.A))
        {
            angle += angleStep;
        }

        if (Input.GetKey(KeyCode.D))
        {
            angle -= angleStep;
        }

        rb.MoveRotation(angle % 360f);

        // Movement
        if (Input.GetKey(KeyCode.W))
        {
            force = transform.up * speed;
            rb.AddForce(force);
        }

        // Constraint the ship's position
        ConstraintMovement();
    }

    void ConstraintMovement()
    {
        if (transform.position.x - spriteSizes.x / 2.0f > 10.6f)
        {
            transform.position = new Vector2(-(transform.position.x - spriteSizes.x / 2.0f), transform.position.y);
        }
        else if (transform.position.x + spriteSizes.x / 2.0f < -10.6f)
        {
            transform.position = new Vector2(-(transform.position.x + spriteSizes.x / 2.0f), transform.position.y);
        }
        else if (transform.position.y - spriteSizes.y / 2.0f > 5.0f)
        {
            transform.position = new Vector2(transform.position.x, -(transform.position.y - spriteSizes.y / 2.0f));
        }
        else if (transform.position.y + spriteSizes.y / 2.0f < -5.0f)
        {
            transform.position = new Vector2(transform.position.x, -(transform.position.y + spriteSizes.y / 2.0f));
        }
    }

    void FireBullet()
    {
        // Instantiate a new bullet
        GameObject newBullet = Instantiate(bullet, muzzle.position, gameObject.transform.rotation);

        // Add the bullet class to the new bullet
        newBullet.AddComponent<Bullet>();

        // Audio
        playerAudio.PlayOneShot(audioClips[0]);
    }
}
