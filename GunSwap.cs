using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GunSwap : MonoBehaviour
{
    [Header("General")]
    public SteamVR_Action_Boolean selectAction;
    public GameObject spawnPosition;
    public GameObject switchyPrefab;
    private GameObject tempOptions;
    public LayerMask layermask;
    private bool switching = false;

    [Header("The Guns")]
    public GameObject MechaBlaster;
    public GameObject FarmersJustice;
    public GameObject RBE;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (selectAction.stateDown)
        {
            switching = true;
            tempOptions = Instantiate(switchyPrefab, transform.position, transform.rotation) as GameObject;
        }
        else if (!selectAction.state && switching)
        {
            Collider[] hitColliders = Physics.OverlapSphere(spawnPosition.transform.position, 0.01f, layermask);
            if (hitColliders.Length == 0)
            {
                MechaBlaster.SetActive(false);
                FarmersJustice.SetActive(false);
                RBE.SetActive(false);
            }
            else
            {
                foreach (var hitCollider in hitColliders)
                {
                    GunSwapTrigger temp = hitCollider.gameObject.GetComponent<GunSwapTrigger>();
                    if (temp.whichGun == "RYE")
                    {
                        MechaBlaster.SetActive(false);
                        FarmersJustice.SetActive(true);
                        RBE.SetActive(false);
                    }
                    else if (temp.whichGun == "RBE")
                    {
                        MechaBlaster.SetActive(false);
                        FarmersJustice.SetActive(false);
                        RBE.SetActive(true);
                    }
                    else if (temp.whichGun == "MECHA")
                    {
                        MechaBlaster.SetActive(true);
                        FarmersJustice.SetActive(false);
                        RBE.SetActive(false);
                    }
                    else
                    {
                        MechaBlaster.SetActive(false);
                        FarmersJustice.SetActive(false);
                        RBE.SetActive(false);
                    }
                }
            }
            Destroy(tempOptions);
            switching = false;
        }
    }
}
