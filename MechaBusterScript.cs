using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class MechaBusterScript : MonoBehaviour
{
    public SteamVR_Action_Boolean fireAction;
    public handEnum whichHand;
    public GameObject bulletPrefab;
    public GameObject bigbulletPrefab;
    public float bulletLifespan;
    public float bulletForce;
    public float bigbulletLifespan;
    public float bigbulletForce;
    public GameObject startBulletHere;
    public GameObject startBigBulletHere;
    private Interactable interactable;

    public float chargeTimer;
    private float timer;

    [Header("Audio")]
    public AudioSource soundfire;
    public AudioSource soundcharge;
    public AudioSource soundcancel;
    public AudioSource soundchargefire;
    [Header("Particles")]
    public GameObject particlecharging;
    public GameObject particlenormalfire;
    public GameObject particlechargefire;
    public GameObject particleexhaust;
    private ParticleSystem PS;

    public GameObject chargeparticlepoint;
    public GameObject fireparticlepoint;
    public GameObject exhaustpoint1;
    public GameObject exhaustpoint2;
    public GameObject exhaustpoint3;
    public GameObject exhaustpoint4;

    public GameObject chargingparticle;
    private bool charging;


    void Start()
    {
        interactable = GetComponent<Interactable>();
        timer = chargeTimer;

        charging = false;
    }

    void Update()
    {
        if (fireAction.stateDown)
        {
            bang();
            soundfire.Play();


            // main particle
            GameObject tempfireparticle;
            tempfireparticle = Instantiate(particlenormalfire, fireparticlepoint.transform.position, fireparticlepoint.transform.rotation) as GameObject;
            tempfireparticle.transform.parent = gameObject.transform;
            Destroy(tempfireparticle, 1f);
        }

        else if (fireAction.state)
        {
            timer -= Time.deltaTime;
            if (timer < (chargeTimer - 0.25f) && charging == false)
            {
                // charge particle
                soundcharge.Play();
                chargingparticle = Instantiate(particlecharging, fireparticlepoint.transform.position, fireparticlepoint.transform.rotation) as GameObject;
                chargingparticle.transform.parent = gameObject.transform;
                charging = true;
            }
        }
        else
        {
            if (timer < 0)
            {
                bigbang();
                soundchargefire.Play();

                // main particle
                GameObject tempfireparticle;
                tempfireparticle = Instantiate(particlechargefire, fireparticlepoint.transform.position, fireparticlepoint.transform.rotation) as GameObject;
                tempfireparticle.transform.parent = gameObject.transform;
                Destroy(tempfireparticle, 1f);

                // exhaust particles
                tempfireparticle = Instantiate(particleexhaust, exhaustpoint1.transform.position, exhaustpoint1.transform.rotation) as GameObject;
                tempfireparticle.transform.parent = gameObject.transform;
                Destroy(tempfireparticle, 1f);
                tempfireparticle = Instantiate(particleexhaust, exhaustpoint2.transform.position, exhaustpoint2.transform.rotation) as GameObject;
                tempfireparticle.transform.parent = gameObject.transform;
                Destroy(tempfireparticle, 1f);
                tempfireparticle = Instantiate(particleexhaust, exhaustpoint3.transform.position, exhaustpoint3.transform.rotation) as GameObject;
                tempfireparticle.transform.parent = gameObject.transform;
                Destroy(tempfireparticle, 1f);
                tempfireparticle = Instantiate(particleexhaust, exhaustpoint4.transform.position, exhaustpoint4.transform.rotation) as GameObject;
                tempfireparticle.transform.parent = gameObject.transform;
                Destroy(tempfireparticle, 1f);

                PS = chargingparticle.GetComponent<ParticleSystem>();
                PS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                Destroy(chargingparticle, 1f);
                charging = false;
            }
            else //if (timer < (chargeTimer - 0.25f))
            {
                if(timer < (chargeTimer - 0.25f))
                {
                    soundcancel.Play();
                    soundcharge.Stop();
                    PS = chargingparticle.GetComponent<ParticleSystem>();
                    PS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    Destroy(chargingparticle, 1f);
                }
                charging = false;
            }
            timer = chargeTimer;
        }
    }

    void bang()
    {
        GameObject Temporary_Bullet_Handler;
        Temporary_Bullet_Handler = Instantiate(bulletPrefab, startBulletHere.transform.position, startBulletHere.transform.rotation) as GameObject;

        BulletScript BS = Temporary_Bullet_Handler.GetComponent<BulletScript>();
        if (whichHand == handEnum.Left)
        {
            BS.FiredFrom = "Left";
        }
        else
        {
            BS.FiredFrom = "Right";
        }

        Rigidbody Temporary_RigidBody;
        Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();

        Temporary_RigidBody.AddForce(Temporary_Bullet_Handler.transform.forward * bulletForce);

        Destroy(Temporary_Bullet_Handler, bulletLifespan);
    }

    void bigbang()
    {
        GameObject Temporary_Bullet_Handler;
        Temporary_Bullet_Handler = Instantiate(bigbulletPrefab, startBigBulletHere.transform.position, startBigBulletHere.transform.rotation) as GameObject;

        BulletScript BS = Temporary_Bullet_Handler.GetComponent<BulletScript>();
        if (whichHand == handEnum.Left)
        {
            BS.FiredFrom = "Left";
        }
        else
        {
            BS.FiredFrom = "Right";
        }

        Rigidbody Temporary_RigidBody;
        Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();

        Temporary_RigidBody.AddForce(Temporary_Bullet_Handler.transform.forward * bigbulletForce);

        Destroy(Temporary_Bullet_Handler, bigbulletLifespan);
    }

    public enum handEnum
    {
        Left,
        Right
    }
}
