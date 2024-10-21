using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetScript : MonoBehaviour
{
    public TargetManager TM;
    private Renderer rendr;
    public TMP_Text damageText;
    public bool damagable;
    public float health;
    public AudioSource audio;
    public GameObject breakParticle;
    public GameObject particleOrigin;
    public Renderer theRenderer;

    public TMP_Text targetText;

    void Start()
    {
        GameObject temp = GameObject.Find("TargetManager");
        TM = temp.GetComponent<TargetManager>();
        temp = GameObject.Find("Counter");
        targetText = temp.GetComponent<TMP_Text>();

        rendr = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        rendr.material.SetColor("_Color", Color.red);
        Invoke("ColorReset", 0.1f);
        if (other.tag == "bullet")
        {
            BulletScript BS = other.GetComponent<BulletScript>();
            damageText.text = BS.damage.ToString();

            TM.shotsLanded++;
            if(BS.SpecialProperty == "RYE" && BS.FiredFrom == "Left")
            {
                TM.Invoke("RyeGunUpdateLeft", 0);
            }
            else if(BS.SpecialProperty == "RYE" && BS.FiredFrom == "Right")
            {
                TM.Invoke("RyeGunUpdateRight", 0);
            }
            else if(BS.SpecialProperty == "RUBBER" && BS.FiredFrom == "Left")
            {
                TM.Invoke("RBEGunUpdateLeft", 0);
            }
            else if(BS.SpecialProperty == "RUBBER" && BS.FiredFrom == "Right")
            {
                TM.Invoke("RBEGunUpdateRight", 0);
            }

            if (damagable)
            {
                health -= BS.damage;
            }
            if ((health > 0 && damagable) || !damagable)
            {
                BS.Invoke("DestroyBullet", 0);
            }
            if (health <= 0)
            {
                TM.targetsBroken++;
                CountCounter();
                audio.Play();
                GameObject temp = Instantiate(breakParticle, particleOrigin.transform.position, particleOrigin.transform.rotation) as GameObject;
                Destroy(temp, 4f);
                Destroy(theRenderer);
                Destroy(this.gameObject, 2f);
            }
        }
    }

    private void ColorReset()
    {
        rendr.material.SetColor("_Color", Color.white);
    }

    public void CountCounter()
    {
        targetText.text = "Targets Hit: " + TM.targetsBroken.ToString();

        if (TM.targetsBroken >= 125)
        {
            targetText.color = new Color32(215, 190, 105, 255);
            targetText.text = "You've broken all the targets. Congragulations! \nTargets Hit: " + TM.targetsBroken.ToString();
        }
    }
}