using UnityEngine;
using UnityEngine.UI;

public class HurdleDetector : MonoBehaviour
{
    public Transform rayOrigin;            // The origin point of the ray
    public float rayDistance = 10f;        // Distance the ray should travel
    public LayerMask layerMask;            // Optional: filter by layer
    public Vector3 rayDirection = Vector3.forward;
    public Text metersText;
    public Canvas canvas;

    private void Start()
    {
        canvas.worldCamera = RaceModeManager.Instance.cameraForHurdleCanvas;
    }
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

        if (Physics.Raycast(ray, out hit, rayDistance, layerMask, QueryTriggerInteraction.Collide))
        {

            
            if (GetComponentInParent<PlayerAnimationV2>().isPlayer)
            {
                Debug.Log("Distance to hurdle: " + hit.distance.ToString("F2") + " meters");
                if (hit.distance < 10f && hit.distance > 0)
                {
                    canvas.gameObject.SetActive(true);
                    metersText.text = "Hurdle " + hit.distance.ToString("F2");
                }
                else
                {
                    canvas.gameObject.SetActive(false);

                }
                Debug.DrawLine(ray.origin, hit.point, Color.green);

            }

            // Optional: Draw green line to hit point
           
        }
        else
        {
            canvas.gameObject.SetActive(false);
        }
    }
}
