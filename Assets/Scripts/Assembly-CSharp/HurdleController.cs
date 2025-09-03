using System.Collections;
using UnityEngine;

public class HurdleController : MonoBehaviour
{
	public Collider barCollider;

	public Collider stickCollider_right;

	public Collider stickCollider_left;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag != "Ground")
		{
			StartCoroutine(wait(0.25f));
			Collider collider = collision.collider;
			Physics.IgnoreCollision(barCollider, collider);
			Physics.IgnoreCollision(stickCollider_right, collider);
			Physics.IgnoreCollision(stickCollider_left, collider);
		}
	}

	private IEnumerator wait(float seconds)
	{
		yield return new WaitForSeconds(seconds);
	}
}
