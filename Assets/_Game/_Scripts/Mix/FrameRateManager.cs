using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    [SerializeField] private int targetFPS = 60;

    void Awake()
    {
        QualitySettings.vSyncCount = 0; // disable VSync
        Application.targetFrameRate = targetFPS;
    }
}
