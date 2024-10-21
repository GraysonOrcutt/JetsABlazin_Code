using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class RyeGunScript : MonoBehaviour
{
    [Header("Technical Stuff")]
    public SteamVR_Action_Boolean fireAction;
    public GameObject bulletPrefab;
    public GameObject hotBulletPrefab;
    public float bulletLifespan;
    public float bulletForce;
    public GameObject startBulletHere;
    private Interactable interactable;
    public handEnum whichHand;
    public int ShotsToOverheat;
    public int ShotsUntilOverheat;
    private bool rapidlyShooting;
    public int timesToShoot;
    public float shotSpacing;
    private int shotCount;

    private bool shouldFire;
    private bool shouldFireRapidly;

    [Header("Particles")]
    public GameObject normalMuzzleShot;
    public GameObject flamingMuzzleShot;

    public GameObject smallFireParticle;
    public GameObject bigFireParticle;
    public GameObject activelyFlamingParticle;

    public GameObject smallFirePosition;
    public GameObject bigFirePosition;
    public GameObject activelyFlamingPosition;

    private GameObject smallFireObject;
    private GameObject bigFireObject;
    private GameObject activelyFlamingObject;
    private ParticleSystem PS;

    [Header("Audio")]
    public AudioSource soundfire;
    public AudioSource soundhot;
    public AudioSource soundhotter;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        rapidlyShooting = false;
        shouldFire = false;
        shouldFireRapidly = false;
    }

    void Update()
    {
        if (fireAction.stateDown)
        {
            if(ShotsUntilOverheat < ShotsToOverheat && !rapidlyShooting)
            {
                //yeehaw();
                shouldFire = true;
                ShotsUntilOverheat++;
                Debug.Log("NORMAL SHOT TRIGGERED");
            }
            else if (!rapidlyShooting)
            {
                ShotsUntilOverheat = 0;
                rapidlyShooting = true;
                shotCount = 0;
                //yeehawder();
                shouldFireRapidly = true;
                soundhotter.Play();
                if (activelyFlamingObject == null)
                {
                    activelyFlamingObject = Instantiate(activelyFlamingParticle, activelyFlamingPosition.transform.position, activelyFlamingPosition.transform.rotation) as GameObject;
                    activelyFlamingObject.transform.parent = gameObject.transform;
                }
                Debug.Log("FIRE SHOT TRIGGERED");
            }
        }

        //FireStuff
        if (ShotsUntilOverheat >= ShotsToOverheat)
        {
            if (!soundhot.isPlaying)
            {
                soundhot.Play();
            }
            if (bigFireObject == null)
            {
                bigFireObject = Instantiate(bigFireParticle, bigFirePosition.transform.position, bigFirePosition.transform.rotation) as GameObject;
                bigFireObject.transform.parent = gameObject.transform;
            }

            if (smallFireObject != null)
            {
                PS = smallFireObject.GetComponent<ParticleSystem>();
                PS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                Destroy(smallFireObject, 2f);
            }
        }
        else if (ShotsUntilOverheat >= (ShotsToOverheat * 0.5))
        {
            if (!soundhot.isPlaying)
            {
                soundhot.Play();
            }
            if (smallFireObject == null)
            { 
                smallFireObject = Instantiate(smallFireParticle, smallFirePosition.transform.position, smallFirePosition.transform.rotation) as GameObject;
                smallFireObject.transform.parent = gameObject.transform;
            }

            if (bigFireObject != null)
            {
                PS = bigFireObject.GetComponent<ParticleSystem>();
                PS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                Destroy(bigFireObject, 2f);
            }
        }
        else
        {
            if (soundhot.isPlaying)
            {
                soundhot.Stop();
            }
            if (bigFireObject != null)
            {
                PS = bigFireObject.GetComponent<ParticleSystem>();
                PS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                Destroy(bigFireObject, 2f);
            }

            if (smallFireObject != null)
            {
                PS = smallFireObject.GetComponent<ParticleSystem>();
                PS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                Destroy(smallFireObject, 2f);
            }
        }
    }

    void FixedUpdate()
    {
        if (shouldFire)
        {
            yeehaw();
            shouldFire = false;
        }
        
        if (shouldFireRapidly)
        {
            yeehawder();
            shouldFireRapidly = false;
        }
    }

    void yeehaw()
    {
        soundfire.Play();

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

        GameObject tempfireparticle;
        tempfireparticle = Instantiate(normalMuzzleShot, startBulletHere.transform.position, startBulletHere.transform.rotation) as GameObject;
        tempfireparticle.transform.parent = gameObject.transform;
        Destroy(tempfireparticle, 1f);
    }

    void yeehawder()
    {
        soundfire.Play();

        GameObject Temporary_Bullet_Handler;
        Temporary_Bullet_Handler = Instantiate(hotBulletPrefab, startBulletHere.transform.position, startBulletHere.transform.rotation) as GameObject;

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

        Temporary_Bullet_Handler.transform.SetParent(null);
        Destroy(Temporary_Bullet_Handler, bulletLifespan);

        GameObject tempfireparticle;
        tempfireparticle = Instantiate(flamingMuzzleShot, startBulletHere.transform.position, startBulletHere.transform.rotation) as GameObject;
        tempfireparticle.transform.parent = gameObject.transform;
        Destroy(tempfireparticle, 1f);

        shotCount++;
        if (shotCount != timesToShoot)
        {
            Invoke("yeehawder", shotSpacing);
        }
        else
        {
            soundhotter.Stop();
            if (activelyFlamingObject != null)
            {
                PS = activelyFlamingObject.GetComponent<ParticleSystem>();
                PS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                Destroy(activelyFlamingObject, 2f);
            }
            rapidlyShooting = false;

        }
    }

    public enum handEnum
    {
        Left,
        Right
    }
}
