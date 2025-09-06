using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RaceModeManager : MonoBehaviour
{
    public static RaceModeManager Instance;
    public GameObject hurdle;
    private GameObject hurdleExist;
    public Transform hurdleSpawnPosition;
    public Modes activeMode;
    public Camera cameraForHurdleCanvas;

    private void Start()
    {
        Instance = this;
    }
    public void EnableMode()
    {
        if (activeMode == Modes.Hurdles)
        {
            if(hurdleExist)
                Destroy(hurdleExist);
            hurdleExist = Instantiate(hurdle);
            hurdleExist.transform.position = hurdleSpawnPosition.position;
            DOVirtual.DelayedCall(0.5f, () =>
            {
                hurdleExist.SetActive(true);
            });
        }
        else
        {
            if (hurdleExist)
                Destroy(hurdleExist);
        }
    }
}
