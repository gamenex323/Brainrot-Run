using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HurdleDetector : MonoBehaviour
{
    public Transform rayOrigin;            // The origin point of the ray
    public float rayDistance = 10f;        // Distance the ray should travel
    public LayerMask hurdleLayerMask;            // Optional: filter by layer
    public LayerMask relayLayerMask;
    public Vector3 rayDirection = Vector3.forward;
    public TextMeshPro metersText;
    public GameObject hurdleIcon;
    public GameObject relayIcon;

    void Update()
    {
        if (rayOrigin == null)
        {
            Debug.LogWarning("Ray origin not assigned.");
            return;
        }

        // Cast a ray from the origin in the given direction
        Ray ray = new Ray(rayOrigin.position, rayOrigin.TransformDirection(rayDirection));
        RaycastHit hit;
        if(RaceModeManager.Instance.activeMode == Modes.Hurdles)
        {
            if (Physics.Raycast(ray, out hit, rayDistance, hurdleLayerMask, QueryTriggerInteraction.Collide))
            {
                if (GetComponentInParent<PlayerAnimationV2>().isPlayer)
                {
                    Debug.Log(hit.distance.ToString("F2") + " M");
                    if (hit.distance < 10f && hit.distance > 0)
                    {
                        hurdleIcon.gameObject.SetActive(true);
                        metersText.gameObject.SetActive(true);
                        metersText.text =hit.distance.ToString("F2") + " M";
                    }
                    else
                    {
                        metersText.gameObject.SetActive(false);

                    }
                    Debug.DrawLine(ray.origin, hit.point, Color.green);

                }
                // Optional: Draw green line to hit point
            }
            else
            {
                metersText.gameObject.SetActive(false);
            }
        }


        if (RaceModeManager.Instance.activeMode == Modes.Relays)
        {
            if (Physics.Raycast(ray, out hit, rayDistance, relayLayerMask, QueryTriggerInteraction.Collide))
            { 
                if (GetComponentInParent<PlayerAnimationV2>().isPlayer)
                {
                    if (hit.distance < 15f && hit.distance > 0)
                    {
                        relayIcon.gameObject.SetActive(true);
                        metersText.gameObject.SetActive(true);
                        metersText.text = hit.distance.ToString("F2") + " M";
                    }
                    else
                    {
                        metersText.gameObject.SetActive(false);

                    }
                    Debug.DrawLine(ray.origin, hit.point, Color.green);
                }
                // Optional: Draw green line to hit point
            }
            else
            {
                metersText.gameObject.SetActive(false);
            }
        }

        if (RaceModeManager.Instance.activeMode == Modes.Sprint)
        {
            metersText.gameObject.SetActive(false);
        }
    }


    private void OnDisable()
    {
        metersText.gameObject.SetActive(false);
    }
}
