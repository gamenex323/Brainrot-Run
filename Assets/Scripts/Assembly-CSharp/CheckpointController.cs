using UnityEngine;

public class CheckpointController : MonoBehaviour
{
	public FinishLineController flc;

	public GameObject player;

	private void OnTriggerEnter(Collider col)
	{
		GameObject gameObject = col.gameObject;
		if (gameObject.tag == "Chest" && gameObject.transform.parent.parent.parent.parent.parent.parent.parent.gameObject == player)
		{
			flc.isActive = true;
		}
	}
}
