using System.Collections;
using UnityEngine;

public class RacerFootV2 : MonoBehaviour
{
	public RaceManager raceManager;

	public Animator animator;

	public BoxCollider bc;

	public Rigidbody rb;

	private PlayerAttributes attributes;

	public PlayerAnimationV2 animation;

	private float swingTimeBonus;

	private float swingTimeBonusModifier;

	private float zTiltMinAbs;

	private float zTiltMaxAbs;

	public float leanMagnitude;

	private float energyModifier;

	public string side;

	public bool input;

	private bool touchDown;

	public bool groundContact;

	public float swingFrames;

	private float power;

	private float knee_dominance;

	private float knee_dominance_powerModifier;

	private float zSpeed;

	private float ySpeed;

	private float transitionPivotSpeed;

	private float zSpeedOverTransitionPivotSpeed;

	private float turnoverFactor;

	private float torsoAngle_max;

	private float torsoAngle;

	private Vector3 forceDirHoriz;

	private Vector3 forceDirVert;

	private float forceHoriz;

	private float forcePatch;

	private void Start()
	{
		attributes = GetComponent<PlayerAttributes>();
		knee_dominance = attributes.KNEE_DOMINANCE;
		transitionPivotSpeed = attributes.TRANSITION_PIVOT_SPEED;
		knee_dominance_powerModifier = Mathf.Pow(2f - knee_dominance, 1.5f);
		knee_dominance_powerModifier = 1f;
		swingFrames = 0f;
		zTiltMinAbs = Mathf.Abs(-45f);
		zTiltMaxAbs = Mathf.Abs(45f);
		turnoverFactor = 150f * (2f - animation.turnover);
		torsoAngle_max = animation.torsoAngle_max;
		forcePatch = 0.85f;
	}

	private void FixedUpdate()
	{
		if (animation.mode == 1)
		{
			swingTimeBonus = 1f;
		}
		else
		{
			if (animation.mode != 2)
			{
				return;
			}
			power = animation.power;
			leanMagnitude = (animation.zTilt + zTiltMinAbs) / (zTiltMaxAbs + zTiltMinAbs);
			torsoAngle = animation.torsoAngle;
			zSpeed = animation.speedHoriz;
			ySpeed = rb.velocity.y;
			zSpeedOverTransitionPivotSpeed = zSpeed / transitionPivotSpeed;
			if (zSpeedOverTransitionPivotSpeed > 1f)
			{
				zSpeedOverTransitionPivotSpeed = 1f;
			}
			if (leanMagnitude < 0.375f)
			{
				leanMagnitude = 0.375f;
			}
			if (touchDown)
			{
				return;
			}
			swingFrames += 1f;
			swingTimeBonus = swingFrames / turnoverFactor;
			if (swingTimeBonus > 0.065f)
			{
				if (swingTimeBonus > 0.12f)
				{
					swingTimeBonus = 0.12f;
				}
			}
			else
			{
				swingTimeBonus = 0.065f;
			}
			swingTimeBonus *= swingTimeBonus * swingTimeBonus * swingTimeBonus * 1000000f * (1f / 90f);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		GameObject gameObject = collision.gameObject;
		if (animation.mode != 2 || !gameObject.tag.StartsWith("Ground"))
		{
			return;
		}
		touchDown = true;
		float num = torsoAngle / torsoAngle_max;
		float num2 = num * (num * num) * (1f - zSpeed / 40f) * 12f;
		if (num2 > 1f)
		{
			if (num2 > 12f)
			{
				num2 = 12f;
			}
		}
		else
		{
			num2 = 1f;
		}
		forceHoriz = 1f;
		forceHoriz *= power * (1f - zSpeedOverTransitionPivotSpeed) * knee_dominance_powerModifier;
		forceHoriz *= num2;
		forceHoriz *= swingTimeBonus;
		StartCoroutine(applyForce(forceHoriz));
		animation.updateEnergy(zSpeed, swingTimeBonus);
		swingTimeBonus = 0f;
		swingFrames = 0f;
	}

	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.tag.StartsWith("Ground"))
		{
			groundContact = true;
			if (animation.mode == 2)
			{
				touchDown = false;
			}
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag.StartsWith("Ground"))
		{
			groundContact = false;
		}
	}

	public IEnumerator applyForce(float forceMagnitude)
	{
		for (int i = 0; i < 5; i++)
		{
			forceDirHoriz = animation.gyro.transform.forward;
			rb.AddForce(forceDirHoriz * forcePatch * forceMagnitude, ForceMode.Force);
			yield return new WaitForSeconds(1f / 90f);
		}
	}
}
