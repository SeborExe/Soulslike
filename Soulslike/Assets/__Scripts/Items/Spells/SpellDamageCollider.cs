using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDamageCollider : DamageCollider
{
    public GameObject impactParticles;
    public GameObject projectileParticles;
    public GameObject muzzleParticles;

    bool hasCollider = false;

    CharacterStats spellTarget;
    Rigidbody rigidbody;

    Vector3 impactNormal; //Use to rotate impact particles

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        projectileParticles = Instantiate(projectileParticles, transform.position, transform.rotation);
        projectileParticles.transform.parent = transform;

        if (muzzleParticles)
        {
            muzzleParticles = Instantiate(muzzleParticles, transform.position, transform.rotation);
            Destroy(muzzleParticles, 2f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasCollider)
        {
            if (collision.gameObject.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.gameObject.GetComponent<IllusionaryWall>();
                illusionaryWall.wallHasBeenHit = true;
            }

            spellTarget = collision.transform.GetComponent<CharacterStats>();

            if (spellTarget != null)
            {
                spellTarget.TakeDamage(currentWeaponDamage);
            }

            hasCollider = true;
            impactParticles = Instantiate(impactParticles, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

            Destroy(projectileParticles);
            Destroy(impactParticles, 3f);
            Destroy(gameObject, 5f);
        }
    }
}
