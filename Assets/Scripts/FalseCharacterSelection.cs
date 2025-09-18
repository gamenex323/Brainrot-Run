using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseCharacterSelection : MonoBehaviour
{
    public GameObject panelToFalse;
    void Update()
    {
        panelToFalse.SetActive(false);
    }
}
