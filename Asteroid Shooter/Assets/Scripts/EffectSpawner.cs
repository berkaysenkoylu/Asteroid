using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : MonoBehaviour {

    public GameObject asteroidColliisonEffect;
    public GameObject asteroidExplosion;
    public GameObject mediumAsteroidExplosion;
    public GameObject playerExplosion;
    public GameObject enemyExplosion;
    public AudioClip[] audioClips;

    AudioSource asteroidSource;

    private void Start()
    {
        asteroidSource = GetComponent<AudioSource>();
    }

    public void AsteroidCollision(Vector3 position)
    {
        Instantiate(asteroidColliisonEffect, position, asteroidColliisonEffect.transform.rotation);
    }

    public void AsteroidsBlowingUp(Vector3 position, GameObject explosion, int index)
    {
        Instantiate(explosion, position, Quaternion.identity);

        asteroidSource.PlayOneShot(audioClips[index]);
    }

    public void EnemyBlowingUp(Vector3 position, GameObject explosion, int index)
    {
        Instantiate(explosion, position, Quaternion.identity);

        asteroidSource.PlayOneShot(audioClips[index]);
    }
}
