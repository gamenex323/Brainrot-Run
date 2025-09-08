using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RelayCharacter : MonoBehaviour
{
    public int legNumber;
    public bool isPlayer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerAnimationV2>())
        {
            if (other.GetComponentInParent<PlayerAnimationV2>().gameObject.name == gameObject.name)
            {
                other.GetComponentInParent<PlayerAnimationV2>().gameObject.
                    GetComponent<Rigidbody>().AddForce(Vector3.forward * 2f, ForceMode.Impulse);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                GetComponent<Rigidbody>().AddForce(Vector3.right * 50f, ForceMode.Impulse);
                if (isPlayer)
                {
                    RaceModeManager.Instance.legText.text = LegNumber();
                    RaceModeManager.Instance.relayHeading.SetActive(true);
                }
            }
        }

    }

    string LegNumber()
    {
        switch (legNumber)
        {
            case 1:
                return "2nd Leg";
            case 2:
                return "3rd Leg";
            case 3:
                return "4th Leg";
            default:
                return "Unknown Leg";
        }
    }
}
