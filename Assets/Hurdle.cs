using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurdle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.gameObject.tag == "Bot")
            {
                Debug.Log("Collide With: " + other.gameObject.name + GetComponent<DOTweenAnimation>());
                GetComponent<Rigidbody>().AddForce(other.transform.position, ForceMode.Impulse);
                GetComponent<BoxCollider>().enabled = false;
            }
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("Collide With Player: " + other.gameObject.name + GetComponent<DOTweenAnimation>());
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    GetComponent<BoxCollider>().enabled = false;
                });
            }
        }
    }
}
