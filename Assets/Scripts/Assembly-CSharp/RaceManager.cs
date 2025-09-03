using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour
{
	public static int RACE_EVENT_60M = 0;

	public static int RACE_EVENT_100M = 1;

	public static int RACE_EVENT_200M = 2;

	public static int RACE_EVENT_400M = 3;

	public static int VIEW_MODE_LIVE = 0;

	public static int VIEW_MODE_REPLAY = 1;

	public GlobalController gc;

	public CameraController cameraController;

	public FinishLineController finishLineController;

	public SplitsController splitsController;

	public CheckpointController checkpointController;

	public Text resultsText;

	public GameObject gameCamera;

	public GameObject cameraButton;

	public int raceEvent;

	public int viewMode;

	public int raceStatus;

	public static int STATUS_MARKS = 1;

	public static int STATUS_SET = 2;

	public static int STATUS_GO = 3;

	public static int STATUS_FINISHED = 4;

	public static int STATUS_INACTIVE = 5;

	public bool fStart;

	public bool raceStarted;

	public float raceTime;

	public int raceTick;

	public bool cancelSaveGhost;

	public bool playerFinished;

	public bool allRacersFinished;

	public bool racerPB;

	public float racerPB_time;

	public bool userPB;

	public float userPB_time;

	public bool playerWR;

	public float userRecord_time;

	public string userRecord_name;

	public float wr_time;

	public string wr_name;

	public string raceEvent_string;

	public int focusIndex;

	public GameObject player_backEnd;

	public GameObject player;

	private int playerLane;

	public List<GameObject> racers_backEnd;

	public List<GameObject> racers;

	public List<GameObject> racers_laneOrder;

	public List<GameObject> bots;

	public List<GameObject> ghosts;

	public GameObject RacersBackEndParent;

	public GameObject RacersFieldParent;

	public GameObject track;

	public GameObject[] startingBlocks_100m;

	public GameObject[] startingBlocks_200m;

	public GameObject[] startingBlocks_400m;

	public GameObject[] startingBlocks_60m;

	public GameObject[] startingBlocks_current;

	public GameObject[] startingLines_100m;

	public GameObject[] startingLines_200m;

	public GameObject[] startingLines_400m;

	public GameObject[] startingLines_60m;

	public GameObject[] startingLines_current;

	public GameObject finishLine;

	public GameObject finishTape;

	public GameObject hurdlesSetPrefab;

	public GameObject hurdlesSet;

	public GameObject curve1_surface;

	public GameObject curve2_surface;

	public GameObject curve1_sphere;

	public GameObject curve2_sphere;

	public TextMeshProUGUI speedText;

	public TextMeshProUGUI clockText;

	private TimeSpan timeSpan;

	private int minutes;

	private int seconds;

	private int milliseconds;

	public Image energyUIImage;
	public TextMeshProUGUI energyText;

	public int racerCount;

	public int playerCount;

	public int botCount;

	public int ghostCount;

	public int racersFinished;

	private float botDifficulty;

	public float transparency_ghost_base;

	public float transparency_ghost_min;

	public float transparency_bot_base;

	public float transparency_bot_min;

	private PlayerAttributes att;

	private PlayerAnimationV2 anim;

	private Rigidbody rb;

	private TimerController tc;

	private OrientationController oc;

	private EnergyMeterController emc;

	private PlayerAttributes playerAtt;

	private PlayerAnimationV2 playerAnim;

	private Rigidbody playerRb;

	private TimerController playerTc;

	private OrientationController playerOc;

	private EnergyMeterController playerEmc;

	private PlayerAttributes[] botAtts;

	private PlayerAnimationV2[] botAnims;

	private Rigidbody[] botRbs;

	private TimerController[] botTcs;

	private OrientationController[] botOcs;

	private EnergyMeterController[] botEmcs;

	private Bot_AI[] botAIs;

	private PlayerAttributes[] ghostAtts;

	private PlayerAnimationV2[] ghostAnims;

	private Rigidbody[] ghostRbs;

	private TimerController[] ghostTcs;

	private OrientationController[] ghostOcs;

	private EnergyMeterController[] ghostEmcs;

	public GameObject finishPlaceHolder;

    private bool uiLeftPressed;
    private bool uiRightPressed;

    private void OnEnable()
    {
        TapButton.btnClickedEvent += HandleUIButton;
    }

    private void OnDisable()
    {
        TapButton.btnClickedEvent -= HandleUIButton;
    }

    private void HandleUIButton(bool isLeft, bool isPressed)
    {
        if (isLeft)
            uiLeftPressed = isPressed;
        else
            uiRightPressed = isPressed;
    }

    private void Start()
	{
	}

	private void Update()
	{
		if (raceStatus >= STATUS_FINISHED)
		{
			return;
		}
		if (raceStatus >= STATUS_GO)
		{
			raceTime += Time.deltaTime;
		}
		updateSpeedometer();
		if (GlobalController.allowInput)
		{
			if (Input.GetKeyDown(KeyCode.C))
			{
				cameraController.cycleCameraMode(viewMode);
			}
			if (viewMode == VIEW_MODE_REPLAY)
			{
				if (uiRightPressed)
				{
					focusIndex++;
					if (focusIndex >= racers_laneOrder.Count)
					{
						focusIndex = 0;
					}
					cameraController.referenceObject = racers_laneOrder[focusIndex];
				}
				if (uiLeftPressed)
				{
					focusIndex--;
					if (focusIndex < 0)
					{
						focusIndex = racers_laneOrder.Count - 1;
					}
					cameraController.referenceObject = racers_laneOrder[focusIndex];
				}
			}
		}
		if (raceTime >= 90f && !cancelSaveGhost)
		{
			cancelSaveGhost = true;
			StartCoroutine(gc.showNotificationBlip("Your ghost will not be saved (too much data).", 5f));
		}
		int mode = cameraController.mode;
		bool flag = false;
		if (viewMode != VIEW_MODE_LIVE)
		{
			return;
		}
		for (int i = 0; i < racers_laneOrder.Count; i++)
		{
			GameObject gameObject = racers_laneOrder[i];
			if (!(gameObject != player))
			{
				continue;
			}
			float num = 1f;
			if (gameObject.tag == "Bot")
			{
				num = transparency_bot_base;
				if ((mode == CameraController.SIDE_RIGHT && i >= playerLane) || (mode == CameraController.SIDE_LEFT && i < playerLane))
				{
					float num2 = Mathf.Abs(gameCamera.transform.InverseTransformDirection(gameObject.transform.position - player.transform.position).x);
					if (num2 < 3f)
					{
						flag = true;
						num *= num2 / 3f;
						if (num < transparency_bot_min)
						{
							num = transparency_bot_min;
						}
					}
				}
			}
			else if (gameObject.tag == "Ghost")
			{
				num = transparency_ghost_base;
				if ((mode == CameraController.SIDE_RIGHT && i >= playerLane) || (mode == CameraController.SIDE_LEFT && i < playerLane))
				{
					float num3 = Mathf.Min(Vector3.Distance(gameObject.transform.position, player.transform.position), Vector3.Distance(gameObject.transform.position, cameraController.camera.transform.position));
					if (num3 < 3f)
					{
						flag = true;
						num *= num3 / 3f;
						if (num < transparency_ghost_min)
						{
							num = transparency_ghost_min;
						}
					}
				}
			}
			if (flag)
			{
				setTransparency(gameObject, num);
			}
		}
	}

	private void FixedUpdate()
	{
		if (raceStatus >= STATUS_INACTIVE)
		{
			return;
		}
		if (!allRacersFinished)
		{
			setRacerModes(raceStatus);
			if (raceStatus > STATUS_MARKS)
			{
				manageRace();
			}
		}
		if (raceStatus >= STATUS_GO)
		{
			raceTick++;
		}
	}

	public void init()
	{
		raceStatus = STATUS_INACTIVE;
	}

	public void setRaceEvent(int e)
	{
		raceEvent = e;
	}

	public List<GameObject> setupRace(List<GameObject> backEndRacers, int raceEvent, int viewMode, bool newLanes)
	{
		racers_backEnd = backEndRacers;
		racers = new List<GameObject>();
		bots = new List<GameObject>();
		ghosts = new List<GameObject>();
		this.raceEvent = raceEvent;
		this.viewMode = viewMode;
		fStart = false;
		raceStarted = false;
		raceTime = 0f;
		raceTick = 0;
		cancelSaveGhost = false;
		playerFinished = false;
		allRacersFinished = false;
		racerPB = false;
		userPB = false;
		userPB_time = GlobalController.getUserPB(raceEvent);
		playerWR = false;
		racerCount = 0;
		playerCount = 0;
		botCount = 0;
		ghostCount = 0;
		racersFinished = 0;
		botAtts = new PlayerAttributes[8];
		botAnims = new PlayerAnimationV2[8];
		botRbs = new Rigidbody[8];
		botTcs = new TimerController[8];
		botOcs = new OrientationController[8];
		botEmcs = new EnergyMeterController[8];
		botAIs = new Bot_AI[8];
		ghostAtts = new PlayerAttributes[8];
		ghostAnims = new PlayerAnimationV2[8];
		ghostRbs = new Rigidbody[8];
		ghostTcs = new TimerController[8];
		ghostOcs = new OrientationController[8];
		ghostEmcs = new EnergyMeterController[8];
		if (viewMode == VIEW_MODE_LIVE)
		{
			StartCoroutine(setResultSheetHeader());
			for (int i = 0; i < racers_backEnd.Count; i++)
			{
				GameObject gameObject = racers_backEnd[i];
				if (gameObject.tag == "Player (Back End)")
				{
					player_backEnd = gameObject;
					botDifficulty = calculateDifficulty(raceEvent, GlobalController.getUserPB(raceEvent));
					break;
				}
			}
			cameraButton.SetActive(value: false);
		}
		else
		{
			cameraButton.SetActive(value: true);
		}
		for (int j = 0; j < racers_backEnd.Count; j++)
		{
			racers_backEnd[j].SetActive(value: false);
			GameObject gameObject = UnityEngine.Object.Instantiate(racers_backEnd[j]);
			gameObject.tag = racers_backEnd[j].tag.Split(' ')[0];
			gameObject.transform.SetParent(RacersFieldParent.transform);
			gameObject.SetActive(value: true);
			rb = gameObject.GetComponent<Rigidbody>();
			rb.velocity = Vector3.zero;
			att = gameObject.GetComponent<PlayerAttributes>();
			att.setPaths(PlayerAttributes.DEFAULT_PATH_LENGTH);
			att.copyAttributesFromOther(racers_backEnd[j], "all");
			att.isRacing = false;
			anim = gameObject.GetComponent<PlayerAnimationV2>();
			anim.globalController = gc;
			anim.init(raceEvent);
			anim.mode = PlayerAnimationV2.Set;
			anim.finished = false;
			anim.energy = 100f;
			anim.launchFlag = false;
			tc = gameObject.GetComponent<TimerController>();
			tc.attributes = att;
			tc.animation = anim;
			tc.oc = anim.GetComponent<OrientationController>();
			emc = gameObject.GetComponent<EnergyMeterController>();
			racerCount++;
			if (gameObject.tag == "Player")
			{
				setPlayer(gameObject);
				playerAtt = att;
				playerAnim = anim;
				playerRb = rb;
				playerTc = tc;
				playerOc = oc;
				playerEmc = emc;
				playerCount++;
				playerAnim.marker.SetActive(value: true);
				playerAnim.hudIndicatorT.position = playerAnim.hudIndicatorT.position + Vector3.up;
				emc.setUIImage(energyUIImage, energyText);
				emc.adjustForEnergyLevel(100f);
			}
			else if (gameObject.tag == "Bot")
			{
				Bot_AI component = gameObject.GetComponent<Bot_AI>();
				component.raceManager = this;
				component.init(botDifficulty);
				anim.init(raceEvent);
				bots.Add(gameObject);
				botAtts[botCount] = att;
				botAnims[botCount] = anim;
				botRbs[botCount] = rb;
				botTcs[botCount] = tc;
				botOcs[botCount] = oc;
				botEmcs[botCount] = emc;
				botAIs[botCount] = component;
				botCount++;
				if (viewMode == VIEW_MODE_LIVE)
				{
					setTransparency(gameObject, transparency_bot_base);
				}
			}
			else if (gameObject.tag == "Ghost")
			{
				ghosts.Add(gameObject);
				ghostAtts[ghostCount] = att;
				ghostAnims[ghostCount] = anim;
				ghostRbs[ghostCount] = rb;
				ghostTcs[ghostCount] = tc;
				ghostOcs[ghostCount] = oc;
				ghostEmcs[ghostCount] = emc;
				ghostCount++;
				if (viewMode == VIEW_MODE_LIVE)
				{
					setTransparency(gameObject, transparency_ghost_base);
				}
			}
			racers.Add(gameObject);
		}
		if (raceEvent == RACE_EVENT_100M)
		{
			startingLines_current = startingLines_100m;
			startingBlocks_current = startingBlocks_100m;
			raceEvent_string = "100m";
		}
		else if (raceEvent == RACE_EVENT_200M)
		{
			startingLines_current = startingLines_200m;
			startingBlocks_current = startingBlocks_200m;
			raceEvent_string = "200m";
		}
		else if (raceEvent == RACE_EVENT_400M)
		{
			startingLines_current = startingLines_400m;
			startingBlocks_current = startingBlocks_400m;
			raceEvent_string = "400m";
		}
		else if (raceEvent == RACE_EVENT_60M)
		{
			startingLines_current = startingLines_60m;
			startingBlocks_current = startingBlocks_60m;
			raceEvent_string = "60m";
		}
		if (viewMode == VIEW_MODE_REPLAY)
		{
			player = racers[gc.playerIndex];
			EnergyMeterController component2 = player.GetComponent<EnergyMeterController>();
			PlayerAnimationV2 component3 = player.GetComponent<PlayerAnimationV2>();
			component3.marker.SetActive(value: true);
			component3.hudIndicatorRenderer.material = gc.hudIndicatorMat_player;
			component3.hudIndicatorT.position = component3.hudIndicatorT.position + Vector3.up;
			component2.setUIImage(energyUIImage, energyText);
			component2.adjustForEnergyLevel(100f);
			Time.timeScale = 4f;
		}
		hideStartingBlocks();
		assignLanes(racers, newLanes);
		playerLane = playerAtt.lane;
		racers_laneOrder = new List<GameObject>(racers);
		racers_laneOrder.Sort((GameObject x, GameObject y) => x.GetComponent<PlayerAttributes>().lane.CompareTo(y.GetComponent<PlayerAttributes>().lane));
		finishLineController.init();
		splitsController.init(raceEvent, player);
		checkpointController.player = player;
		resetClock();
		raceStatus = STATUS_MARKS;
		checkTipsConditions();
		focusIndex = racers_laneOrder.IndexOf(player);
		return racers;
	}

	public void resetCounts()
	{
		racerCount = 0;
		playerCount = 0;
		botCount = 0;
		ghostCount = 0;
		racersFinished = 0;
	}

	private float calculateDifficulty(int raceEvent, float time)
	{
		float num = 0f;
		float num2 = 0f;
		if (raceEvent == RACE_EVENT_100M)
		{
			num = 9.8f;
			num2 = 0.65f;
		}
		else if (raceEvent == RACE_EVENT_200M)
		{
			num = 19.8f;
			num2 = 0.7f;
		}
		else if (raceEvent == RACE_EVENT_400M)
		{
			num = 44f;
			num2 = 0.825f;
		}
		else if (raceEvent == RACE_EVENT_60M)
		{
			num = 6.45f;
			num2 = 0.85f;
		}
		float num3 = num / time;
		if (num3 > 0f)
		{
			if (num3 > num2)
			{
				if (num3 > 1f)
				{
					num3 = 1f;
				}
			}
			else
			{
				num3 = num2;
			}
		}
		else
		{
			num3 = num2;
		}
		return num3;
	}

	private void assignLanes(List<GameObject> racers, bool newLanes)
	{
		List<int> list = new List<int> { 5, 4, 6, 3, 7, 2, 8, 1 };
		int count = racers.Count;
		list.RemoveRange(count, list.Count - count);
		if (viewMode == VIEW_MODE_LIVE && viewMode == VIEW_MODE_LIVE && newLanes)
		{
			for (int i = 0; i < racers.Count; i++)
			{
				PlayerAttributes component = racers[i].GetComponent<PlayerAttributes>();
				int index = UnityEngine.Random.Range(0, list.Count);
				component.lane = list[index];
				racers_backEnd[i].GetComponent<PlayerAttributes>().lane = list[index];
				list.RemoveAt(index);
			}
		}
		int num = 0;
		for (int j = 0; j < racers.Count; j++)
		{
			GameObject obj = racers[j];
			PlayerAttributes component2 = obj.GetComponent<PlayerAttributes>();
			PlayerAnimationV2 component3 = obj.GetComponent<PlayerAnimationV2>();
			OrientationController component4 = obj.GetComponent<OrientationController>();
			num = component2.lane;
			GameObject gameObject = startingLines_current[num - 1];
			GameObject gameObject2 = startingBlocks_current[num - 1];
			obj.transform.position = gameObject.transform.position + gameObject.transform.forward * -0.13f + Vector3.up * Mathf.Pow(num, 0.3f);


			obj.GetComponent<Rigidbody>().isKinematic = true;

            StartCoroutine(DelayKinematicOff(obj.GetComponent<Rigidbody>(), 0.2f));

            gameObject2.SetActive(value: true);
			Physics.IgnoreCollision(component3.rightFootScript.bc, gameObject2.GetComponent<BoxCollider>());
			Physics.IgnoreCollision(component3.leftFootScript.bc, gameObject2.GetComponent<BoxCollider>());
			gameObject2.transform.position = gameObject.transform.position + gameObject.transform.forward * -1f + Vector3.up * 0.22f;
			gameObject2.transform.rotation = gameObject.transform.rotation;
			gameObject2.GetComponent<Rigidbody>().velocity = Vector3.zero;
			StartCoroutine(setBlockPedals(gameObject2, racers[j]));
			component4.sphere1 = curve1_sphere;
			component4.sphere2 = curve2_sphere;
			component4.sphere = component4.sphere1;
			if (raceEvent != RACE_EVENT_100M && raceEvent != RACE_EVENT_60M)
			{
				component4.updateOrientation(enforcePosition: false);
			}
			component4.initRotations();


			//Debug.Break();
		}
	}

    IEnumerator DelayKinematicOff(Rigidbody rb, float t)
    {
        yield return new WaitForSeconds(t);
        rb.isKinematic = false;
    }

    private void hideStartingBlocks()
	{
		for (int i = 0; i < startingBlocks_current.Length; i++)
		{
			startingBlocks_100m[i].SetActive(value: false);
			startingBlocks_200m[i].SetActive(value: false);
			startingBlocks_400m[i].SetActive(value: false);
			startingBlocks_60m[i].SetActive(value: false);
		}
	}

	private void setRenderQueues(List<GameObject> racers)
	{
		_ = cameraController.mode;
		for (int i = 0; i < racers.Count; i++)
		{
			PlayerAttributes component = racers[i].GetComponent<PlayerAttributes>();
			int lane = component.lane;
			SkinnedMeshRenderer[] array = new SkinnedMeshRenderer[7] { component.smr_dummy, component.smr_top, component.smr_bottoms, component.smr_shoes, component.smr_socks, component.smr_headband, component.smr_sleeve };
			for (int j = 0; j < array.Length; j++)
			{
				SkinnedMeshRenderer obj = array[j];
				Material sharedMaterial = obj.sharedMaterial;
				sharedMaterial.renderQueue += lane * 100 + j;
				obj.sharedMaterial = sharedMaterial;
			}
		}
	}

	public void setOffRacers()
	{
		if (!fStart)
		{
			raceStarted = true;
			if (viewMode == VIEW_MODE_LIVE)
			{
				gc.audioController.playSound(AudioController.GUNSHOT, 0f);
				gc.audioController.playSound(AudioController.CHEERING, 0f);
			}
		}
	}

	public void manageRace()
	{
		bool num = racersFinished >= racerCount;
		if (playerFinished)
		{
			playerFinished = false;
			gc.showResultsScreen();
		}
		if (num)
		{
			raceStatus = STATUS_FINISHED;
		}
		if (viewMode == VIEW_MODE_LIVE)
		{
			playerAnim.readInput(raceTick);
			playerAnim.applyInput(raceTick);
			playerTc.recordInput(raceTick);
		}
		for (int i = 0; i < bots.Count; i++)
		{
			botAIs[i].runAI(raceTick);
			botAnims[i].readInput(raceTick);
			botAnims[i].applyInput(raceTick);
			botTcs[i].recordInput(raceTick);
		}
		for (int j = 0; j < ghosts.Count; j++)
		{
			ghostAnims[j].readInput(raceTick);
			ghostAnims[j].applyInput(raceTick);
			if (raceTick > 0)
			{
				ghostAnims[j].setPositionAndVelocity(raceTick);
			}
		}
	}

	public void addFinisher(GameObject racer)
	{
		racersFinished++;
		PlayerAttributes component = racer.GetComponent<PlayerAttributes>();
		PlayerAnimationV2 component2 = racer.GetComponent<PlayerAnimationV2>();
		float num;
		if (!(racer == player))
		{
			num = ((!(racer.tag == "Ghost")) ? ((float)raceTick / 100f) : component.personalBests[raceEvent]);
		}
		else
		{
			num = float.Parse(raceTime.ToString("F3"));
			playerFinished = true;
			if (viewMode == VIEW_MODE_LIVE)
			{
				finishPlaceHolder.transform.position = racer.transform.position;
				cameraController.setCameraFocus(finishPlaceHolder, cameraController.mode);
				if (num < component.personalBests[raceEvent] || component.personalBests[raceEvent] == -1f)
				{
					playerAtt.personalBests[raceEvent] = num;
					racerPB = true;
					racerPB_time = num;
					if (num < userPB_time)
					{
						userPB = true;
						userPB_time = num;
						if (num <= wr_time)
						{
							playerWR = true;
							component.resultTag = "<color=magenta>WR</color>";
						}
						else
						{
							component.resultTag = "<color=yellow>PB</color>";
						}
						gc.unlockManager.updateUnlocks(raceEvent, num);
						finishLineController.yayParticles.Play();
					}
				}
				else
				{
					component.resultTag = "";
				}
			}
			component2.finished = true;
			component2.marker.SetActive(value: false);
		}
		if (viewMode == VIEW_MODE_LIVE)
		{
			component.finishTime = num;
			component.pathLength = raceTick;
			string text = num.ToString("F2").PadLeft(24 - component.racerName.Length);
			component.resultString = "<color=" + component.resultColor + ">" + component.racerName + "</color>" + text + "  " + component.resultTag;
			if (racer == player)
			{
				Text text2 = resultsText;
				text2.text = text2.text + "\n <b><color=yellow>" + racersFinished + "</color>  " + component.resultString + "</b>";
			}
			else
			{
				Text text2 = resultsText;
				text2.text = text2.text + "\n " + racersFinished + "  " + component.resultString;
			}
		}
		StartCoroutine(component2.pop(0.5f));
	}

	public IEnumerator falseStart()
	{
		fStart = true;
		gc.goRaceScreen();
		gc.countdownController.showFalseStartText();
		yield return new WaitForSeconds(0.2f);
		Time.timeScale = 0.15f;
		yield return new WaitForSeconds(2f * Time.timeScale);
		Time.timeScale = 1f;
		gc.restartRace();
	}

	public void falseStartBotsReaction()
	{
		for (int i = 0; i < bots.Count; i++)
		{
			botAtts[i].isRacing = true;
			botAIs[i].runAI(raceTick + 1);
		}
		raceTick++;
	}

	public void setRacerModes(int mode)
	{
		if (mode == STATUS_MARKS)
		{
			playerAnim.upInSet = false;
		}
		else if (mode == STATUS_SET)
		{
			if (viewMode == VIEW_MODE_REPLAY)
			{
				Time.timeScale = 2f;
			}
			if (!playerAnim.upInSet && viewMode == VIEW_MODE_LIVE)
			{
				gc.audioController.playSound(AudioController.BLOCK_RATTLE, 0f);
				gc.audioController.playSound(AudioController.VOICE_SET, 0f);
			}
			playerAnim.upInSet = true;
		}
		else if (mode == STATUS_GO)
		{
			if (viewMode == VIEW_MODE_REPLAY)
			{
				Time.timeScale = 1f;
			}
			playerAtt.isRacing = true;
		}
		for (int i = 0; i < ghostCount; i++)
		{
			if (mode == STATUS_MARKS)
			{
				ghostAnims[i].upInSet = false;
			}
			else if (mode == STATUS_SET)
			{
				ghostAnims[i].upInSet = true;
			}
			else if (mode == STATUS_GO)
			{
				ghostAtts[i].isRacing = true;
			}
		}
		for (int j = 0; j < botCount; j++)
		{
			if (mode == STATUS_MARKS)
			{
				botAnims[j].upInSet = false;
			}
			else if (mode == STATUS_SET)
			{
				botAnims[j].upInSet = true;
			}
			else if (mode == STATUS_GO)
			{
				botAtts[j].isRacing = true;
			}
		}
	}

	public void quitRace()
	{
		for (int i = 0; i < racers.Count; i++)
		{
			racers[i].GetComponent<PlayerAnimationV2>().pop(0f);
		}
		gc.taskManager.addTask(TaskManager.SAVE_DEFAULT_CAMERA_MODES);
		gc.clearListAndObjects(racers);
		cameraController.setCameraFocusOnStart();

		gc.goStartScreen();
		//gc.goSetupScreen();
	}

	private void setPlayer(GameObject racer)
	{
		player = racer;
		playerAtt = player.GetComponent<PlayerAttributes>();
		playerAnim = player.GetComponent<PlayerAnimationV2>();
		playerTc = player.GetComponent<TimerController>();
		playerOc = player.GetComponent<OrientationController>();
		playerEmc = player.GetComponent<EnergyMeterController>();
		playerRb = player.GetComponent<Rigidbody>();
	}

	private void updateSpeedometer()
	{
		if (player == null)
			return;

		timeSpan = TimeSpan.FromSeconds(raceTime);
		minutes = timeSpan.Minutes;
		seconds = timeSpan.Seconds;
		milliseconds = timeSpan.Milliseconds / 10;
		if ((float)minutes > 0f)
		{
			clockText.text = minutes + ":" + seconds + "." + milliseconds;
		}
		else
		{
			clockText.text = seconds + "." + milliseconds + " s";
		}
		float num = player.GetComponent<PlayerAnimationV2>().speedHoriz / 2f;
		num *= 2.236936f;
		num = Mathf.Round(num * 10f) / 10f;
		speedText.text = num + " mph";
		speedText.color = Color.Lerp(Color.white, Color.red, num / 35f);
	}

	private void resetClock()
	{
		minutes = 0;
		seconds = 0;
		milliseconds = 0;
	}

	private void checkTipsConditions()
	{
		if (!GlobalController.hasFirstRace)
		{
			gc.tipsManager.showTips_firstRace();
		}
		else if (raceEvent == RACE_EVENT_400M && !GlobalController.hasFirstRace_400m)
		{
			gc.tipsManager.showTips_400m();
		}
	}

	private void setTransparency(GameObject racer, float alpha)
	{
		PlayerAttributes component = racer.GetComponent<PlayerAttributes>();
		SkinnedMeshRenderer[] array = new SkinnedMeshRenderer[7] { component.smr_shoes, component.smr_socks, component.smr_top, component.smr_bottoms, component.smr_sleeve, component.smr_headband, component.smr_dummy };
		for (int i = 0; i < array.Length; i++)
		{
			Material obj = array[i].materials[0];
			Color color = obj.color;
			color.a = alpha;
			obj.color = color;
			color = obj.GetColor("_OutlineColor");
			color.a = alpha;
			obj.SetColor("_OutlineColor", color);
		}
	}

	private IEnumerator setResultSheetHeader()
	{
		userRecord_time = GlobalController.getUserRecordTime(raceEvent);
		userRecord_name = GlobalController.getUserRecordName(raceEvent);
		if (!PlayFabManager.loggedIn)
		{
			PlayFabManager.loginError = false;
			StartCoroutine(gc.handleLogin(successMsg: true, failMsg: false));
			yield return new WaitUntil(() => PlayFabManager.loggedIn || PlayFabManager.loginError);
		}
		if (PlayFabManager.loggedIn)
		{
			GlobalController.getWorldRecordInfo(raceEvent);
			yield return new WaitUntil(() => PlayFabManager.userLeaderboardInfoRetrieved || PlayFabManager.leaderboardGetError);
		}
		string text;
		if (!PlayFabManager.loggedIn || PlayFabManager.leaderboardGetError)
		{
			wr_time = -1f;
			wr_name = "NA";
			text = raceEvent_string + " Dash Finals\n==========================\n<color=magenta>WR:</color> <No connection!>";
		}
		else
		{
			wr_time = (float)PlayFabManager.userScore / -10000f;
			wr_name = PlayFabManager.userRacerName + " (" + PlayFabManager.userDisplayName + ")";
			text = raceEvent_string + " Dash Finals\n==========================\n<color=magenta>WR: " + wr_time.ToString("F3") + "</color> " + wr_name;
		}
		resultsText.text = text;
		if (userRecord_name != "None")
		{
			text = text + "\n<color=yellow>PB: " + userRecord_time.ToString("F3") + "</color> " + userRecord_name;
		}
		text += "\n\n==========================\n\nFinals";
		resultsText.text = text;
	}

	private IEnumerator setBlockPedals(GameObject block, GameObject racer)
	{
		while (raceStatus != STATUS_SET)
		{
			block.GetComponent<StartingBlockController>().adjustPedals(racer.GetComponent<PlayerAnimationV2>());
			yield return null;
		}
	}

	private IEnumerator wait(float t)
	{
		yield return new WaitForSeconds(t);
	}
}
