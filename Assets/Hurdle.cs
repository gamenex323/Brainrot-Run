using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurdle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            Debug.Log("Collide With: " + collision.gameObject.name);
            GetComponent<DOTweenAnimation>().DOPlay();
            GetComponent<BoxCollider>().enabled = false;

        }
    }
}
