using UnityEngine;

public class Bot_AI : MonoBehaviour
{
	public RaceManager raceManager;

	public PlayerAnimationV2 anim;

	public PlayerAttributes att;

	private int[] leftInputPath;

	private int[] rightInputPath;

	private bool reaction;

	private float reactionTime;

	private float freeTicks;

	private float downTicks;

	private float ticksPassedLeft;

	private float ticksPassedRight;

	private float frequencyLeft;

	private float frequencyRight;

	private int inputLeft;

	private int inputRight;

	private float time;

	private float tickRate;

	private float pace;

	private bool maintenanceFlag;

	private float difficulty;

	private float cadenceModifier;

	private float quicknessModifier;

	private float powerModifier;

	private float torsoAngle_upright;

	private float torsoAngle_forward;

	private float torsoAngle_back;

	private bool topEndFlag;

	private bool forward;

	private float zSpeed;

	private float transitionPivotSpeed;

	private float zSpeedOverTransitionPivotSpeed;

	private float modifierCapSpeed;

	private float curveModifier;

	private float energyThreshold;

	private float dTime;

	public void runAI(int tick)
	{
		dTime = 1f / 90f;
		if (!reaction)
		{
			reaction = raceManager.raceTime > reactionTime;
			return;
		}
		adjustStride();
		if (ticksPassedLeft >= frequencyLeft)
		{
			if (inputLeft == 0)
			{
				inputLeft = 1;
				frequencyLeft = downTicks;
				if (maintenanceFlag)
				{
					ticksPassedRight = freeTicks * 0.25f;
				}
			}
			else
			{
				inputLeft = 0;
				frequencyLeft = freeTicks;
			}
			ticksPassedLeft = 0f;
		}
		att.leftInputPath[tick] = inputLeft;
		ticksPassedLeft += tickRate;
		if (ticksPassedRight >= frequencyRight)
		{
			if (inputRight == 0)
			{
				inputRight = 1;
				frequencyRight = downTicks;
			}
			else
			{
				inputRight = 0;
				frequencyRight = freeTicks;
			}
			ticksPassedRight = 0f;
		}
		att.rightInputPath[tick] = inputRight;
		ticksPassedRight += tickRate;
	}

	public void adjustStride()
	{
		float torsoAngle = anim.torsoAngle;
		zSpeed = anim.speedHoriz;
		if (anim.onCurve)
		{
			curveModifier = 1.5f;
		}
		else
		{
			curveModifier = 1f;
		}
		zSpeedOverTransitionPivotSpeed = zSpeed / (modifierCapSpeed * curveModifier * 1.1f);
		if (zSpeedOverTransitionPivotSpeed > 1f)
		{
			if (zSpeedOverTransitionPivotSpeed > 1.2f)
			{
				zSpeedOverTransitionPivotSpeed = 1.2f;
			}
		}
		else
		{
			zSpeedOverTransitionPivotSpeed = 1f;
		}
		if (zSpeed < 3f)
		{
			zSpeedOverTransitionPivotSpeed = 0.5f;
		}
		tickRate = (int)(100f * zSpeedOverTransitionPivotSpeed);
		if (!topEndFlag)
		{
			if (torsoAngle < torsoAngle_upright)
			{
				downTicks += 115f;
				topEndFlag = true;
			}
		}
		else
		{
			if (!topEndFlag)
			{
				return;
			}
			if (forward)
			{
				downTicks -= 6f;
				forward = false;
			}
			if (raceManager.raceTick % 10 == 0)
			{
				if (torsoAngle > torsoAngle_upright)
				{
					forward = true;
				}
				else
				{
					if (!maintenanceFlag)
					{
						setTicks(2400f);
						maintenanceFlag = true;
					}
					if (!anim.leanLock && torsoAngle < torsoAngle_back)
					{
						anim.leanLock = true;
						att.leanLockTick = raceManager.raceTick;
					}
				}
			}
			float energy = anim.energy;
			if (pace < 1f && energy < energyThreshold)
			{
				if (energy > 30f)
				{
					pace = Mathf.Lerp(pace, 1f * (1f - energy / 1000f), dTime);
				}
				else
				{
					pace = 1f;
				}
				setTicks(2400f);
			}
		}
	}

	private void setTicks(float _freeTicks)
	{
		freeTicks = _freeTicks * (2f - cadenceModifier) * (2f - pace);
		downTicks = (3575f - _freeTicks) * (2f - cadenceModifier) * (2f - pace);
	}

	public void init(float _difficulty)
	{
		difficulty = _difficulty;
		cadenceModifier = 1f;
		cadenceModifier *= Mathf.Pow(att.TURNOVER, 0.01f);
		quicknessModifier = Mathf.Pow(difficulty, 0.3f);
		anim.quicknessMod = quicknessModifier;
		powerModifier = Mathf.Pow(difficulty, 2f);
		att.POWER *= powerModifier;
		anim.powerMod = powerModifier;
		leftInputPath = att.leftInputPath;
		rightInputPath = att.rightInputPath;
		reaction = false;
		reactionTime = Random.Range(0.01f, 0.03f);
		reactionTime = Random.Range(0.01f, 0.01f + (2f - Mathf.Pow(difficulty, 10f)) / 10f) * Mathf.Pow(2f - difficulty, 4f);
		topEndFlag = false;
		maintenanceFlag = false;
		forward = false;
		transitionPivotSpeed = att.TRANSITION_PIVOT_SPEED;
		modifierCapSpeed = 23f * difficulty;
		freeTicks = 2700f * (2f - cadenceModifier);
		downTicks = 1000f * (2f - cadenceModifier);
		tickRate = 100f;
		if (att.leadLeg == 0)
		{
			frequencyLeft = downTicks;
			frequencyRight = freeTicks;
			inputLeft = 1;
			inputRight = 0;
			ticksPassedLeft = 0f;
			ticksPassedRight = downTicks / 2f + 310f;
		}
		else
		{
			frequencyLeft = freeTicks;
			frequencyRight = downTicks;
			inputLeft = 0;
			inputRight = 1;
			ticksPassedLeft = downTicks / 2f + 310f;
			ticksPassedRight = 0f;
		}
		setSpecialAttributes();
	}

	private void setSpecialAttributes()
	{
		float num = -2.5f;
		if (raceManager.raceEvent == RaceManager.RACE_EVENT_100M)
		{
			torsoAngle_upright = anim.torsoAngle_upright - 4f + num;
			pace = 0.95f;
			energyThreshold = 0f;
			att.POWER *= 1.1f;
			att.CRUISE = 0.55f;
			att.KNEE_DOMINANCE *= 0.9f;
		}
		else if (raceManager.raceEvent == RaceManager.RACE_EVENT_200M)
		{
			torsoAngle_upright = anim.torsoAngle_upright - 3f + num;
			pace = (Random.Range(0.97f, 1f) + Random.Range(0.97f, 1f) + Random.Range(0.97f, 1f) + Random.Range(0.97f, 1f)) / 4f * cadenceModifier;
			pace = 0.95f;
			energyThreshold = 75f;
			att.POWER *= 1.08f;
			att.CRUISE = 0.55f;
			att.KNEE_DOMINANCE *= 0.9f;
		}
		else if (raceManager.raceEvent == RaceManager.RACE_EVENT_400M)
		{
			torsoAngle_upright = anim.torsoAngle_upright - 3f + num;
			energyThreshold = 75f;
			pace = 0.8f;
			att.POWER *= 1.06f;
			att.CRUISE = 0.8f;
			att.KNEE_DOMINANCE *= 0.95f;
			att.FITNESS *= 1.05f;
		}
		else if (raceManager.raceEvent == RaceManager.RACE_EVENT_60M)
		{
			torsoAngle_upright = anim.torsoAngle_upright - 4f + num;
			pace = 0.95f;
			energyThreshold = 0f;
			att.POWER *= 1.1f;
			att.CRUISE = 0.55f;
			att.KNEE_DOMINANCE *= 0.9f;
		}
		torsoAngle_back = torsoAngle_upright;
	}
}
