using UnityEngine;

public class BigScreenCameraController : MonoBehaviour
{
	public RaceManager raceManager;

	public static float tightness = 16f;

	private void Start()
	{
	}

	private void Update()
	{
		if (raceManager.raceStatus <= RaceManager.STATUS_GO && raceManager.player != null)
		{
			Vector3 position = raceManager.player.transform.position;
			Quaternion rotation = base.transform.rotation;
			Quaternion b = Quaternion.LookRotation(position - base.transform.position, Vector3.up);
			base.transform.rotation = Quaternion.Slerp(rotation, b, tightness * Time.deltaTime);
		}
	}
}
