﻿using System.Collections;
using UnityEngine;


public class PlayerCollisionO2 : MonoBehaviour
{
    // Local reference to the central AvatarAccounting object (held by main camera)
    private AvatarAccounting avatarAccounting;

    // Set in inspector
    public AudioSource wallCollisionAudio1;
    public AudioSource wallCollisionAudio2;

    private int collisions = 0;

    private bool inRefuelRange;


    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the AvatarAccounting component of Main Camera
        this.avatarAccounting = Camera.main.GetComponent<AvatarAccounting>();

        inRefuelRange = false;
        // TODO: get values of variables from save data once implemented
        //wallCollision = GetComponent<AudioSource>();
    }


    private void Update()
    {
        if (inRefuelRange)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                //TODO: actually fill tanks
                Debug.Log("pressed f");
            }
        }
    }

    private void OnCollisionEnter(Collision c)
    {
        GameObject collided = c.gameObject;

        switch (collided.tag)
        {
            case "Cave":
                avatarAccounting.AddInjury(InjuryType.WallStrikeDirect);

                if (collisions % 2 == 0)
                {
                    wallCollisionAudio1.Play();
                }
                else
                {
                    wallCollisionAudio2.Play();
                }
                collisions++;
                //TODO: add impulse to player depending on speed
                break;
            case "AirTank":
                avatarAccounting.AddOxygen(OxygenTankRefillAmount.FillBothTanks);
                Destroy(collided);
                break;
            case "Cave_GlancingBlow":
                avatarAccounting.AddInjury(InjuryType.WallStrikeGlancingBlow);
                break;
            case "SharpObject":
                avatarAccounting.AddInjury(InjuryType.SharpObject);
                break;
            case "SharpObject_NearMiss":
                avatarAccounting.AddInjury(InjuryType.SharpObjectNearMiss);
                break;
            case "Monster":
                avatarAccounting.AddInjury(InjuryType.EnemyAttack);
                break;
            case "Monster_NearMiss":
                avatarAccounting.AddInjury(InjuryType.EnemyAttackNearMiss);
                break;
            case "AirTank_Single":
                avatarAccounting.AddOxygen(OxygenTankRefillAmount.FullSingleTank);
                Destroy(collided);
                break;
            case "AirTank_Double":
                avatarAccounting.AddOxygen(OxygenTankRefillAmount.FillBothTanks);
                Destroy(collided);
                break;
            case "AirTank_PonyBottle":
                avatarAccounting.AddOxygenExtraTank();
                Destroy(collided);
                break;
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        GameObject collided = c.gameObject;

        if (collided.tag == "AirTank")
        {
            avatarAccounting.AddOxygen(OxygenTankRefillAmount.FillBothTanks);
            Destroy(collided);
        }
        if (collided.tag == "RefuelStation")
        {
            inRefuelRange = true;
        }
    }

    private void OnTriggerExit(Collider c)
    {
        GameObject collided = c.gameObject;
        if (collided.tag == "RefuelStation")
        {
            inRefuelRange = false;
        }
    }



}