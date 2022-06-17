using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDamageCollider : DamageCollider
{
    [Header("Explosion damage and radius")]
    public int explosiveRadius = 2;
    public int explosionDamage;
    public int explosionSplashDamage;
    //magic damage etc...

    public Rigidbody bombRigidbody;
    public GameObject impactParticles;

    private bool hasCollided = false;


    protected override void Awake()
    {
        damageCollider = GetComponent<Collider>();
        bombRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided)
        {
            hasCollided = true;
            impactParticles = Instantiate(impactParticles, transform.position, Quaternion.identity);
            Explode();

            CharacterStatsManager character = collision.transform.GetComponent<CharacterStatsManager>();

            if (character != null)
            {
                if (character.teamIDNumber != teamIDNumber)
                {
                    character.TakeDamage(0, explosionDamage);
                }
            }

            Destroy(impactParticles, 5f);
            Destroy(transform.parent.parent.gameObject);
        }
    }

    private void Explode()
    {
        Collider[] characters = Physics.OverlapSphere(transform.position, explosiveRadius);

        foreach (Collider objectsInExplosion in characters)
        {
            CharacterStatsManager character = objectsInExplosion.GetComponent<CharacterStatsManager>();

            if (character != null)
            {
                if (character.teamIDNumber != teamIDNumber)
                {
                    character.TakeDamage(0, explosionSplashDamage);
                }
            }
        }
    }
}
