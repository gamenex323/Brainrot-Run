using UnityEngine;

public class GravityModifier : MonoBehaviour
{
	public Rigidbody rb;

	public float magnitude;

	private void Start()
	{
	}

	private void FixedUpdate()
	{
		rb.AddForce(Vector3.down * 10f * magnitude);
	}
}
