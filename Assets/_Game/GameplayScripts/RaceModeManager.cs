using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RaceModeManager : MonoBehaviour
{
    public static RaceModeManager Instance;
    public GameObject hurdle;
    public Modes activeMode;

    private void Start()
    {
        Instance = this;
    }
    public void EnableMode()
    {
        if (activeMode == Modes.Hurdles)
        {
            hurdle.SetActive(true);
        }
        else {
            hurdle.SetActive(false);
        }
    }
}
