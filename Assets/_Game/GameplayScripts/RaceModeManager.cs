using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RaceModeManager : MonoBehaviour
{
    public static RaceModeManager Instance;

    [Header("Hurdle Mode")]
    public GameObject hurdle;
    private GameObject hurdleExist;
    public Transform hurdleSpawnPosition;
    [Space]
    [Space]
    [Header("Relay Mode")]
    public List<GameObject> relayCharacters;
    public GameObject relayHeading;
    public Text legText;

    [Space]
    [Space]
    [Header("Global Variables")]
    [Space]
    [Space]
    public Modes activeMode;
    public Camera cameraForHurdleCanvas;

    private void Start()
    {
        Instance = this;
    }

    void DestroyAllCharacter()
    {
        for (int i = 0; i < relayCharacters.Count; i++)
        {
            Destroy(relayCharacters[i]);

        }
        relayCharacters.Clear();
    }

    public void ResetGameplay()
    {
        DestroyAllCharacter();
        if(hurdleExist)
            Destroy(hurdleExist);
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
