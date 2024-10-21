using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class rbescript : MonoBehaviour
{
    [Header("Technical Stuff")]
    public SteamVR_Action_Boolean fireAction;
    public handEnum whichHand;
    public GameObject redBulletPrefab;
    public GameObject blueBulletPrefab;
    public GameObject greenBulletPrefab;
    public GameObject yellowBulletPrefab;
    public GameObject pinkBulletPrefab;
    public float bulletLifespan;
    public float bulletForce;
    public float bulletForceMultiplier;
    public GameObject startBulletHere;
    private Interactable interactable;
    public float strength;
    public float strengthChange;
    public float gradualChange;

    [Header("Particles")]
    public GameObject normalMuzzleShot;
    public GameObject flamingMuzzleShot;

    private ParticleSystem PS;

    [Header("Audio")]
    public AudioSource soundfire;

    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    void Update()
    {
        if (fireAction.stateDown)
        {
            yeehaw();
            Debug.Log("NORMAL SHOT TRIGGERED");
            strength -= strengthChange;
        }
    }

    void FixedUpdate()
    {
        /*
        1< = RED
        1-2 = BLUE
        2-3 = GREEN
        3-4 = YELLOW
        4-5 = PINK
        */

        if (strength != 2.5)
        {
            if (strength > 2.45 && strength <= 2.55)
                strength = 2.5f;
            else if (strength < 2.5)
                strength += gradualChange;
            else if (strength > 2.5)
                strength -= gradualChange;

            if (strength < 0)
                strength = 0;
            if (strength > 6)
                strength = 6;
        }
    }

    void yeehaw()
    {
        soundfire.Play();

        GameObject bulletPrefab;
        float actualBulletForce;
        if (strength < 1)
        {
            bulletPrefab = redBulletPrefab;
            actualBulletForce = (bulletForce / (bulletForceMultiplier * 2));
        }
        else if (strength < 2)
        {
            bulletPrefab = blueBulletPrefab;
            actualBulletForce = (bulletForce / bulletForceMultiplier);
        }
        else if (strength < 3)
        {
            bulletPrefab = greenBulletPrefab;
            actualBulletForce = bulletForce;
        }
        else if (strength < 4)
        {
            bulletPrefab = yellowBulletPrefab;
            actualBulletForce = (bulletForce * bulletForceMultiplier);
        }
        else
        {
            bulletPrefab = pinkBulletPrefab;
            actualBulletForce = (bulletForce * (bulletForceMultiplier * 2));
        }

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

        Temporary_RigidBody.AddForce(Temporary_Bullet_Handler.transform.forward * actualBulletForce);

        Destroy(Temporary_Bullet_Handler, bulletLifespan);

        /*
        GameObject tempfireparticle;
        tempfireparticle = Instantiate(normalMuzzleShot, startBulletHere.transform.position, startBulletHere.transform.rotation) as GameObject;
        tempfireparticle.transform.parent = gameObject.transform;
        Destroy(tempfireparticle, 1f);
        */
    }

    public enum handEnum
    {
        Left,
        Right
    }
}
