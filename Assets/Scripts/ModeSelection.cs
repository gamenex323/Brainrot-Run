using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSelection : MonoBehaviour
{
    public Modes currentMode;

    public void SetCurrentMode(int mode)
    {
        currentMode = (Modes)mode;
        RaceModeManager.Instance.activeMode = currentMode;
    }
}

[System.Serializable]

public enum Modes
{
    Sprint,
    Hurdles,
    Relays
}
