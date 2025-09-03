using UnityEngine;

public class SplitsLineController : MonoBehaviour
{
	[SerializeField]
	private SplitsController sc;

	private void OnTriggerEnter(Collider col)
	{
		GameObject gameObject = col.gameObject;
		if (gameObject.tag == "Chest")
		{
			GameObject racer = gameObject.transform.parent.parent.parent.parent.parent.parent.parent.gameObject;
			sc.register(racer);
		}
	}
}
