using UnityEngine;

public class TimerController : MonoBehaviour
{
	public PlayerAttributes attributes;

	public PlayerAnimationV2 animation;

	public OrientationController oc;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void FixedUpdate()
	{
	}

	public void recordInput(int tick)
	{
		if (animation.rightInput)
		{
			attributes.rightInputPath[tick] = 1;
		}
		else
		{
			attributes.rightInputPath[tick] = 0;
		}
		if (animation.leftInput)
		{
			attributes.leftInputPath[tick] = 1;
		}
		else
		{
			attributes.leftInputPath[tick] = 0;
		}
		Vector3 velocity = animation.rb.velocity;
		float y = velocity.y;
		velocity.y = 0f;
		float magnitude = velocity.magnitude;
		Vector3 position = base.transform.position;
		attributes.velMagPath[tick] = magnitude;
		attributes.velPathY[tick] = y;
		attributes.posPathY[tick] = position.y;
		attributes.posPathZ[tick] = position.z;
	}
}
