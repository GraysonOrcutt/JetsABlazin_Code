using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage;
    public string SpecialProperty = "N/A";
    public string FiredFrom;
    public GameObject destructionParticle;
    public bool particleOnDestroy;

    public void DestroyBullet()
    {
        if (particleOnDestroy)
        {
            GameObject tempparticle;
            tempparticle = Instantiate(destructionParticle, transform.position, transform.rotation) as GameObject;
            Destroy(tempparticle, 2f);
        }

        Destroy(gameObject);
    }
}
