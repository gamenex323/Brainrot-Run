using UnityEngine;

public class StartingBlockController : MonoBehaviour
{
	public Rigidbody rb;

	public GameObject rightPedal;

	public GameObject leftPedal;

	public ParticleSystem sparks;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void adjustPedals(PlayerAnimationV2 anim)
	{
		Transform leftFoot = anim.leftFoot;
		Transform rightFoot = anim.rightFoot;
		Vector3 position = new Vector3(leftFoot.transform.position.x, leftPedal.transform.position.y, leftFoot.transform.position.z);
		Vector3 position2 = new Vector3(rightFoot.transform.position.x, rightPedal.transform.position.y, rightFoot.transform.position.z);
		leftPedal.transform.position = position;
		rightPedal.transform.position = position2;
	}

	public void addLaunchForce()
	{
		Vector3 vector = Random.insideUnitCircle.normalized;
		rb.AddForce((base.transform.forward * -120f + Vector3.up * 250f + vector * 100f) * (1f / 90f), ForceMode.Impulse);
		base.transform.Rotate(vector * 300f * (1f / 90f));
		sparks.Play();
	}
}
