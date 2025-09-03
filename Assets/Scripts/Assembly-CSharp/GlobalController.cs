using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class GlobalController : MonoBehaviour
{
    public static bool hasInitialSetup;

    public static bool hasFirstRace;

    public static bool hasFirstRace_400m;

    public static bool hasFirstLeaderboard;

    public static string PLAYABLE_RACER_MEMORY = "PLAYABLE RACER IDS";

    public static string SAVED_RACER_MEMORY = "SAVED RACER IDS";

    public GameObject canvas;

    public GameObject canvas_camera;

    public GameObject RaceUI;

    private bool RaceUIActive;

    public static float keyPressAgo;

    [SerializeField]
    private float cooldown_keyPress;

    public GameObject StartScreen;

    [SerializeField]
    private Animator startScreenAnimator;

    private bool StartScreenActive;

    public GameObject SetupScreen;

    private bool SetupScreenActive;

    public GameObject LeaderboardScreen;

    private bool LeaderboardScreenActive;

    public GameObject CharacterCreatorScreen;

    public GameObject previewRacer_creation;

    public GameObject previewRacer_setup;

    public GameObject newRacerButton;

    private bool CharacterCreatorScreenActive;

    public GameObject nameInputField;

    public GameObject CountdownScreen;

    public bool CountdownScreenActive;

    public GameObject RaceScreen;

    public Image screenFlash;

    public bool RaceScreenActive;

    public GameObject FinishScreen;

    private bool FinishScreenActive;

    public GameObject EmptyScreen;

    private bool EmptyScreenActive;

    public GameObject TransitionScreen;

    private bool TransitionScreenActive;

    public ButtonHandler buttonHandler;

    public SelectionListScript ghostSelectButtonList;

    public SelectionListScript playerSelectButtonList;

    public List<string> playableRacerIDs;

    public List<string> savedRacerIDs;

    public GameObject tooltip;

    public TaskManager taskManager;

    public CountdownController countdownController;

    public CameraController cameraController;

    public AudioController audioController;

    public MusicManager musicManager;

    public RaceManager raceManager;

    public WelcomeManager welcomeManager;

    public SettingsManager settingsManager;

    public TipsManager tipsManager;

    public SetupManager setupManager;

    public UnlockManager unlockManager;

    public NotificationBlipController notificationBlipController;

    public EnvironmentController environmentController;

    public LeaderboardManager leaderboardManager;

    public SteamController steamController;

    public GameObject racerPrefab;

    public GameObject player_backEnd;

    public int playerIndex;

    public List<GameObject> racers_backEnd;

    public List<GameObject> racers_backEnd_replay;

    public List<GameObject> racers;

    public int selectedRaceEvent;

    public GameObject overheadCamera;

    public Material hudIndicatorMat_player;

    public Material hudIndicatorMat_nonPlayer;

    public Camera previewCamera_setup;

    public Camera previewCamera_creation;

    [SerializeField]
    private Transform cameraStartTransform;

    [SerializeField]
    private Text usernameInputText;

    public string username;

    private bool lbUpdate;

    public int downloadedGhosts;

    public static bool allowInput;

    public InitialCharacterSetup initialCharacterSetupHandler;

    private void Start()
    {
        TransitionScreen.SetActive(value: true);
        TransitionScreen.GetComponent<CanvasGroup>().alpha = 1f;
        cameraController.setCameraFocus("100m Start", CameraController.STATIONARY_SLOW);
        startScreenAnimator.SetBool("startScreenAccessed", value: false);
        if (PlayerPrefs.GetInt("hasInitialSetup") == 1)
        {
            hasInitialSetup = true;
            if (PlayerPrefs.GetInt("hasFirstRace") == 1)
            {
                hasFirstRace = true;
            }
            else
            {
                hasFirstRace = false;
            }
            if (PlayerPrefs.GetInt("hasFirstRace_400m") == 1)
            {
                hasFirstRace_400m = true;
            }
            else
            {
                hasFirstRace_400m = false;
            }
            if (PlayerPrefs.GetInt("hasFirstLeaderboard") == 1)
            {
                hasFirstLeaderboard = true;
            }
            else
            {
                hasFirstLeaderboard = false;
            }
        }
        else
        {
            hasInitialSetup = false;
            runFirstTimeOperations();


            //buttonHandler.createCharacter();
        }
        StartCoroutine(handleLogin(successMsg: false, failMsg: true));
        float @float = PlayerPrefs.GetFloat("Audio Volume");
        downloadedGhosts = PlayerPrefs.GetInt("Downloaded Ghosts");
        int @int = PlayerPrefs.GetInt("Character Slots");
        string @string = PlayerPrefs.GetString("Display Mode");
        float float2 = PlayerPrefs.GetFloat("Camera Distance");
        int int2 = PlayerPrefs.GetInt("Camera Gameplay Mode");
        int int3 = PlayerPrefs.GetInt("Camera Replay Mode");
        KeyCode int4 = (KeyCode)PlayerPrefs.GetInt("Controls Left");
        KeyCode int5 = (KeyCode)PlayerPrefs.GetInt("Controls Right");
        cameraController.init(float2, int2, int3);
        raceManager.init();
        musicManager.init(@float, !hasInitialSetup);
        environmentController.init(PlayerPrefs.GetInt("Theme"));
        setupManager.init(setCamera: false);
        settingsManager.init(int4, int5, float2, @string);
        unlockManager.init(@int);
        racers_backEnd = new List<GameObject>();
        racers_backEnd_replay = new List<GameObject>();
        racers = new List<GameObject>();
        playerSelectButtonList.init(RaceManager.RACE_EVENT_100M, PLAYABLE_RACER_MEMORY, 1, 0, _replaceLastSelection: true, _showIfNoTime: true);
        playableRacerIDs = PlayerPrefs.GetString("PLAYABLE RACER IDS").Split(':').ToList();
        ghostSelectButtonList.init(RaceManager.RACE_EVENT_100M, SAVED_RACER_MEMORY, 7, 0, _replaceLastSelection: false, _showIfNoTime: false);
        savedRacerIDs = PlayerPrefs.GetString("SAVED RACER IDS").Split(':').ToList();

        if (hasInitialSetup)
        {
            goStartScreen();
        }
        else
        {
            initialCharacterSetupHandler.RandomizeCharacter();

            //goStartScreen();

        }

        //if (hasInitialSetup)
        //{
        //	goStartScreen();
        //	return;
        //}
        //goEmptyScreen();
        //welcomeManager.show();
        //CharacterCreatorScreen.transform.Find("Back Button").gameObject.SetActive(value: false);
    }

    private void Update()
    {
        if (CountdownScreenActive)
        {
            if (countdownController.state == CountdownController.SET)
            {
                raceManager.raceStatus = RaceManager.STATUS_SET;
            }
            if (countdownController.finished)
            {
                goRaceScreen();
            }
        }
        if (RaceUIActive && allowInput && keyPressAgo >= cooldown_keyPress)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                restartRace();
                keyPressAgo = 0f;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                raceManager.quitRace();
                keyPressAgo = 0f;
            }
        }
        keyPressAgo += Time.deltaTime;
    }

    public void goStartScreen()
    {
        StartCoroutine(screenTransition("Start Screen", immediate: false));
        if (countdownController.isRunning)
        {
            countdownController.cancelCountdown();
        }
        raceManager.raceStatus = RaceManager.STATUS_INACTIVE;
    }

    public void goSetupScreen()
    {
        StartCoroutine(screenTransition("Setup Screen", immediate: false));
        raceManager.raceStatus = RaceManager.STATUS_INACTIVE;
    }

    public void goLeaderboardScreen()
    {
        StartCoroutine(screenTransition("Leaderboard Screen", immediate: false));
        if (!hasFirstLeaderboard)
        {
            tipsManager.showTips_leaderboard();
        }
    }

    public void goCharacterCreatorScreen()
    {
        PlayerAttributes component = previewRacer_creation.GetComponent<PlayerAttributes>();
        component.setBodyProportions(PlayerAttributes.DEFAULT);
        component.setClothing(PlayerAttributes.DEFAULT);
        StartCoroutine(screenTransition("Character Creator Screen", immediate: false));
    }

    public void goCountDownScreen()
    {
        StartCoroutine(screenTransition("Countdown Screen", immediate: false));
        if (countdownController.isRunning)
        {
            countdownController.cancelCountdown();
        }
        countdownController.startCountdown();
    }

    public void goRaceScreen()
    {
        StartCoroutine(screenTransition("Race Screen", immediate: true));
        StartCoroutine("flash");
        raceManager.raceStatus = RaceManager.STATUS_GO;
        raceManager.setOffRacers();
    }

    public void goFinishScreen()
    {
        StartCoroutine(screenTransition("Finish Screen", immediate: true));
        raceManager.raceStatus = RaceManager.STATUS_FINISHED;
    }

    public void goEmptyScreen()
    {
        StartCoroutine(screenTransition("Empty Screen", immediate: false));
        raceManager.raceStatus = RaceManager.STATUS_INACTIVE;
    }

    public IEnumerator screenTransition(string nextScreen, bool immediate)
    {
        allowInput = false;
        TransitionScreen.SetActive(value: true);
        GameObject[] deactivatedButtons0 = deactivateAllButtons();
        CanvasGroup cg = TransitionScreen.GetComponent<CanvasGroup>();
        if (!immediate)
        {
            while (cg.alpha < 1f)
            {
                cg.alpha += 0.1f;
                yield return null;
            }
            doTasks();
        }
        canvas_camera.SetActive(value: false);
        RaceUI.SetActive(value: false);
        RaceUIActive = false;
        StartScreen.SetActive(value: false);
        StartScreenActive = false;
        SetupScreen.SetActive(value: false);
        SetupScreenActive = false;
        LeaderboardScreen.SetActive(value: false);
        LeaderboardScreenActive = false;
        CharacterCreatorScreen.SetActive(value: false);
        CharacterCreatorScreenActive = false;
        CountdownScreen.SetActive(value: false);
        CountdownScreenActive = false;
        countdownController.hideFalseStartText();
        RaceScreen.SetActive(value: false);
        RaceScreenActive = false;
        FinishScreen.SetActive(value: false);
        FinishScreenActive = false;
        overheadCamera.SetActive(value: false);
        previewCamera_creation.gameObject.SetActive(value: false);
        previewCamera_setup.gameObject.SetActive(value: false);
        switch (nextScreen)
        {
            case "Start Screen":
                StartScreen.SetActive(value: true);
                setupManager.setSelectedRaceEvent(RaceManager.RACE_EVENT_100M, setCamera: false);
                StartScreenActive = true;
                break;
            case "Setup Screen":
                SetupScreen.SetActive(value: true);
                SetupScreenActive = true;
                previewCamera_setup.gameObject.SetActive(value: true);
                if (playerSelectButtonList.numSelected < 1)
                {
                    playerSelectButtonList.getFirst().GetComponent<SelectionButtonScript>().toggle();
                }
                break;
            case "Leaderboard Screen":
                LeaderboardScreen.SetActive(value: true);
                LeaderboardScreenActive = true;
                leaderboardManager.setLeaderboardMode(LeaderboardManager.GLOBAL);
                leaderboardManager.setEmpty();
                break;
            case "Character Creator Screen":
                CharacterCreatorScreen.SetActive(value: true);
                CharacterCreatorScreenActive = true;
                canvas_camera.SetActive(value: true);
                previewCamera_creation.gameObject.SetActive(value: true);
                break;
            case "Countdown Screen":
                CountdownScreen.SetActive(value: true);
                RaceUI.SetActive(value: true);
                CountdownScreenActive = true;
                RaceUIActive = true;
                overheadCamera.SetActive(value: true);
                break;
            case "Race Screen":
                RaceScreen.SetActive(value: true);
                RaceUI.SetActive(value: true);
                RaceScreenActive = true;
                RaceUIActive = true;
                overheadCamera.SetActive(value: true);
                break;
            case "Finish Screen":
                FinishScreen.SetActive(value: true);
                FinishScreenActive = true;
                break;
            case "Empty Screen":
                EmptyScreen.SetActive(value: true);
                EmptyScreenActive = true;
                break;
        }
        GameObject[] deactivatedButtons1 = deactivateAllButtons();
        if (!immediate)
        {
            while (cg.alpha > 0f)
            {
                cg.alpha -= 0.1f;
                yield return null;
            }
            TransitionScreen.SetActive(value: false);
        }
        allowInput = true;
        activateButtons(deactivatedButtons0);
        activateButtons(deactivatedButtons1);
    }

    public void goScreenAfterCharacterCreation()
    {
        if (hasInitialSetup)
        {
            goSetupScreen();
            return;
        }
        clearRacersFromScene();
        raceManager.resetCounts();
        buttonHandler.loadSelectedPlayer();
        loadBots();
        startNewRace();
    }

    public void startNewRace()
    {
        startRaceAsLive(newLanes: true);
    }

    public void restartRace()
    {
        PlayerPrefs.SetInt("Camera Gameplay Mode", cameraController.cameraGameplayMode);
        startRaceAsLive(newLanes: false);
    }

    public void startRaceAsLive(bool newLanes)
    {
        Time.timeScale = 1f;
        startRace(selectedRaceEvent, RaceManager.VIEW_MODE_LIVE, newLanes);
    }

    public void startRaceAsReplay()
    {
        if (raceManager.viewMode != RaceManager.VIEW_MODE_REPLAY)
        {
            taskManager.addTask(TaskManager.SETUP_REPLAY);
        }
        startRace(selectedRaceEvent, RaceManager.VIEW_MODE_REPLAY, newLanes: false);
    }

    public void startRace(int raceEvent, int viewMode, bool newLanes)
    {
        if (viewMode == RaceManager.VIEW_MODE_LIVE)
        {
            StopCoroutine(raceManager.falseStart());
            countdownController.startCountdown();
            audioController.playSound(AudioController.VOICE_READY, 0f);
        }
        else if (viewMode == RaceManager.VIEW_MODE_REPLAY)
        {
            countdownController.cancelCountdown();
        }
        goCountDownScreen();
        StartCoroutine(setupRaceWhenReady(raceEvent, viewMode, newLanes));
    }

    private IEnumerator setupRaceWhenReady(int raceEvent, int viewMode, bool newLanes)
    {
        yield return new WaitUntil(() => taskManager.tasks.Count == 0);
        clearListAndObjects(racers);
        List<GameObject> backEndRacers = new List<GameObject>();
        int cameraMode = 0;
        if (viewMode == RaceManager.VIEW_MODE_LIVE)
        {
            backEndRacers = racers_backEnd;
            cameraMode = cameraController.cameraGameplayMode;
        }
        else if (viewMode == RaceManager.VIEW_MODE_REPLAY)
        {
            backEndRacers = racers_backEnd_replay;
            cameraMode = cameraController.cameraReplayMode;
        }
        racers = raceManager.setupRace(backEndRacers, raceEvent, viewMode, newLanes);
        cameraController.setCameraFocus(raceManager.player, cameraMode);
    }

    public void showResultsScreen()
    {
        StartCoroutine(endRace());
        goFinishScreen();
    }

    public IEnumerator endRace()
    {
        lbUpdate = false;
        if (raceManager.racerPB && !raceManager.cancelSaveGhost)
        {
            if (raceManager.userPB)
            {
                lbUpdate = true;
                taskManager.addTask(TaskManager.SAVE_USER_PB);
                taskManager.addTask(TaskManager.SET_USER_RECORD);
            }
            taskManager.addTask(TaskManager.SAVE_PLAYER);
        }
        taskManager.addTask(TaskManager.SAVE_DEFAULT_CAMERA_MODES);
        yield return null;
    }

    public void clearListAndObjects(List<GameObject> list)
    {
        list.Clear();
        Transform transform = null;
        if (list == racers_backEnd)
        {
            transform = raceManager.RacersBackEndParent.transform;
        }
        else if (list == racers)
        {
            transform = raceManager.RacersFieldParent.transform;
        }
        if (!(transform != null))
        {
            return;
        }
        foreach (Transform item in transform)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }
    }

    public void forgetRacer(string id, string[] sourceMemoryLocations, bool forReplay)
    {
        if (forReplay)
        {
            id += "_REPLAY";
            PlayerPrefs.DeleteKey(id);
        }
        string text = "";
        for (int i = 0; i < sourceMemoryLocations.Length; i++)
        {
            text = sourceMemoryLocations[i];
            if (text == PLAYABLE_RACER_MEMORY)
            {
                playableRacerIDs.Remove(id);
                string value = PlayerPrefs.GetString(PLAYABLE_RACER_MEMORY).Replace(id + ":", "");
                PlayerPrefs.SetString(PLAYABLE_RACER_MEMORY, value);
                if (!PlayerPrefs.GetString(SAVED_RACER_MEMORY).Contains(id))
                {
                    PlayerPrefs.DeleteKey(id);
                }
            }
            else if (text == SAVED_RACER_MEMORY)
            {
                savedRacerIDs.Remove(id);
                string value2 = PlayerPrefs.GetString(SAVED_RACER_MEMORY).Replace(id + ":", "");
                PlayerPrefs.SetString(SAVED_RACER_MEMORY, value2);
                if (!PlayerPrefs.GetString(PLAYABLE_RACER_MEMORY).Contains(id))
                {
                    PlayerPrefs.DeleteKey(id);
                }
            }
        }
    }

    public void saveRacer(GameObject racer, int raceEvent, string[] targetMemoryLocations, bool sendToLeaderboard, bool forReplay)
    {
        PlayerAttributes component = racer.GetComponent<PlayerAttributes>();
        string text = component.id;
        if (forReplay)
        {
            text += "_REPLAY";
        }
        else
        {
            string text2 = "";
            for (int i = 0; i < targetMemoryLocations.Length; i++)
            {
                text2 = targetMemoryLocations[i];
                if (text2 == PLAYABLE_RACER_MEMORY)
                {
                    if ((float)Array.IndexOf(playableRacerIDs.ToArray(), text) == -1f)
                    {
                        PlayerPrefs.SetString(PLAYABLE_RACER_MEMORY, PlayerPrefs.GetString(PLAYABLE_RACER_MEMORY) + text + ":");
                        playableRacerIDs.Add(text);
                    }
                }
                else if (text2 == SAVED_RACER_MEMORY && (float)Array.IndexOf(savedRacerIDs.ToArray(), text) == -1f)
                {
                    PlayerPrefs.SetString(SAVED_RACER_MEMORY, PlayerPrefs.GetString(SAVED_RACER_MEMORY) + text + ":");
                    savedRacerIDs.Add(text);
                }
            }
        }
        string text7;
        string text6;
        string text5;
        string text4;
        string text3;
        string text8 = (text7 = (text6 = (text5 = (text4 = (text3 = "")))));
        int pathLength = component.pathLength;
        int leanLockTick = component.leanLockTick;
        for (int j = 0; j < pathLength; j++)
        {
            text8 = text8 + component.velMagPath[j] + ",";
            text7 = text7 + component.velPathY[j] + ",";
            text6 = text6 + component.posPathY[j] + ",";
            text5 = text5 + component.posPathZ[j] + ",";
            text4 = text4 + component.rightInputPath[j] + ",";
            text3 = text3 + component.leftInputPath[j] + ",";
        }
        string text14;
        string text13;
        string text12;
        string text11;
        string text10;
        string text9;
        string text15 = (text14 = (text13 = (text12 = (text11 = (text10 = (text9 = ""))))));
        for (int k = 0; k < 3; k++)
        {
            text15 = text15 + component.dummyRGB[k] + ",";
            text14 = text14 + component.topRGB[k] + ",";
            text13 = text13 + component.bottomsRGB[k] + ",";
            text12 = text12 + component.shoesRGB[k] + ",";
            text11 = text11 + component.socksRGB[k] + ",";
            text10 = text10 + component.headbandRGB[k] + ",";
            text9 = text9 + component.sleeveRGB[k] + ",";
        }
        int[] array = new int[0];
        array = ((raceEvent != -1) ? new int[1] { raceEvent } : new int[4] { 0, 1, 2, 3 });
        string text16 = "";
        string text17 = "";
        foreach (int num in array)
        {
            string text18 = pathLength + ":" + leanLockTick + ":" + text8 + ":" + text7 + ":" + text6 + ":" + text5 + ":" + text4 + ":" + text3;
            text16 += text18;
            PlayerPrefs.SetString(text + "_pathsForEvent " + num, text16);
        }
        text17 = text + ":" + component.racerName + ":" + component.finishTime + ":" + component.personalBests[0] + ":" + component.personalBests[1] + ":" + component.personalBests[2] + ":" + component.personalBests[3] + ":" + component.resultString + ":" + component.POWER + ":" + component.TRANSITION_PIVOT_SPEED + ":" + component.QUICKNESS + ":" + component.KNEE_DOMINANCE + ":" + component.TURNOVER + ":" + component.FITNESS + ":" + component.LAUNCH_POWER + ":" + component.CURVE_POWER + ":" + component.CRUISE + ":" + component.dummyMeshNumber + ":" + component.topMeshNumber + ":" + component.bottomsMeshNumber + ":" + component.shoesMeshNumber + ":" + component.socksMeshNumber + ":" + component.headbandMeshNumber + ":" + component.sleeveMeshNumber + ":" + component.dummyMaterialNumber + ":" + component.topMaterialNumber + ":" + component.bottomsMaterialNumber + ":" + component.shoesMaterialNumber + ":" + component.socksMaterialNumber + ":" + component.headbandMaterialNumber + ":" + component.sleeveMaterialNumber + ":" + text15 + ":" + text14 + ":" + text13 + ":" + text12 + ":" + text11 + ":" + text10 + ":" + text9 + ":" + component.headX + ":" + component.headY + ":" + component.headZ + ":" + component.neckX + ":" + component.neckY + ":" + component.neckZ + ":" + component.torsoX + ":" + component.torsoY + ":" + component.torsoZ + ":" + component.armX + ":" + component.armY + ":" + component.armZ + ":" + component.legX + ":" + component.legY + ":" + component.legZ + ":" + component.feetX + ":" + component.feetY + ":" + component.feetZ + ":" + component.height + ":" + component.weight + ":" + component.animatorNum + ":" + component.leadLeg + ":" + component.armSpeedFlexL + ":" + component.armSpeedExtendL + ":" + component.armSpeedFlexR + ":" + component.armSpeedExtendR;
        PlayerPrefs.SetString(text, text17);
        if (sendToLeaderboard)
        {
            int num2 = ScoreCalculator.calculateScore_user();
            if (num2 <= PlayerPrefs.GetInt("Total Score1", 0))
            {
                num2 = -1;
            }
            else
            {
                PlayerPrefs.SetInt("Total Score1", num2);
            }
            int num3 = ScoreCalculator.calculateScore(component.personalBests);
            if (num3 <= PlayerPrefs.GetInt("Total Score (Best Racer)1", 0))
            {
                num3 = -1;
            }
            else
            {
                PlayerPrefs.SetInt("Total Score (Best Racer)1", num3);
            }
            string text19 = text17 + "&" + text16;
            string package = string.Join("#", raceEvent.ToString(), component.personalBests[raceEvent].ToString(), component.racerName, text19, num2.ToString(), num3.ToString());
            StartCoroutine(handleLeaderboardSend(package, raceEvent));
        }
    }

    private IEnumerator handleLeaderboardSend(string package, int raceEvent)
    {
        if (!PlayFabManager.loggedIn)
        {
            PlayFabManager.loginError = false;
            StartCoroutine(handleLogin(successMsg: true, failMsg: false));
            yield return new WaitUntil(() => PlayFabManager.loggedIn || PlayFabManager.loginError);
        }
        if (!PlayFabManager.loggedIn)
        {
            PlayerPrefs.SetString("UnsentScore " + raceEvent, package);
            yield break;
        }
        PlayFabManager.leaderboardSent = false;
        PlayFabManager.leaderboardSendFailure = false;
        PlayFabManager.sendLeaderboard(package);
        yield return new WaitUntil(() => PlayFabManager.leaderboardSent || PlayFabManager.leaderboardSendFailure);
        if (PlayFabManager.leaderboardSent)
        {
            PlayerPrefs.DeleteKey("UnsentScore " + raceEvent);
        }
        else if (PlayFabManager.leaderboardSendFailure)
        {
            PlayerPrefs.SetString("UnsentScore " + raceEvent, package);
        }
    }

    public GameObject loadRacer(string id, int raceEvent, string asTag, bool fromLeaderboard, bool forReplay)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(racerPrefab);
        gameObject.tag = asTag;
        PlayerAttributes component = gameObject.GetComponent<PlayerAttributes>();
        if (forReplay)
        {
            id += "_REPLAY";
        }
        component.personalBests = new float[4];
        string[] array;
        string[] array2;
        if (!fromLeaderboard)
        {
            array = PlayerPrefs.GetString(id).Split(':');
            array2 = PlayerPrefs.GetString(id + "_pathsForEvent " + raceEvent).Split(':');
        }
        else
        {
            string[] array3 = PlayFabManager.userRacerData.Split('&');
            array = array3[0].Split(':');
            array[0] = id;
            array2 = array3[1].Split(':');
        }
        int num = 0;
        try
        {
            component.pathLength = int.Parse(array2[num]);
            num++;
        }
        catch (Exception ex)
        {
            Debug.Log("error loading racer: " + id);
            Debug.Log(ex.ToString());
        }
        component.leanLockTick = int.Parse(array2[num]);
        num++;
        string[] array4 = array2[num].Split(',');
        num++;
        string[] array5 = array2[num].Split(',');
        num++;
        string[] array6 = array2[num].Split(',');
        num++;
        string[] array7 = array2[num].Split(',');
        num++;
        string[] array8 = array2[num].Split(',');
        num++;
        string[] array9 = array2[num].Split(',');
        num++;
        num = 0;
        component.id = array[num];
        num++;
        component.racerName = array[num];
        num++;
        component.finishTime = float.Parse(array[num]);
        num++;
        component.personalBests[0] = float.Parse(array[num]);
        num++;
        component.personalBests[1] = float.Parse(array[num]);
        num++;
        component.personalBests[2] = float.Parse(array[num]);
        num++;
        component.personalBests[3] = float.Parse(array[num]);
        num++;
        component.resultString = array[num];
        num++;
        component.POWER = float.Parse(array[num]);
        num++;
        component.TRANSITION_PIVOT_SPEED = float.Parse(array[num]);
        num++;
        component.QUICKNESS = float.Parse(array[num]);
        num++;
        component.KNEE_DOMINANCE = float.Parse(array[num]);
        num++;
        component.TURNOVER = float.Parse(array[num]);
        num++;
        component.FITNESS = float.Parse(array[num]);
        num++;
        component.LAUNCH_POWER = float.Parse(array[num]);
        num++;
        component.CURVE_POWER = float.Parse(array[num]);
        num++;
        component.CRUISE = float.Parse(array[num]);
        num++;
        component.dummyMeshNumber = int.Parse(array[num]);
        num++;
        component.topMeshNumber = int.Parse(array[num]);
        num++;
        component.bottomsMeshNumber = int.Parse(array[num]);
        num++;
        component.shoesMeshNumber = int.Parse(array[num]);
        num++;
        component.socksMeshNumber = int.Parse(array[num]);
        num++;
        component.headbandMeshNumber = int.Parse(array[num]);
        num++;
        component.sleeveMeshNumber = int.Parse(array[num]);
        num++;
        component.dummyMaterialNumber = int.Parse(array[num]);
        num++;
        component.topMaterialNumber = int.Parse(array[num]);
        num++;
        component.bottomsMaterialNumber = int.Parse(array[num]);
        num++;
        component.shoesMaterialNumber = int.Parse(array[num]);
        num++;
        component.socksMaterialNumber = int.Parse(array[num]);
        num++;
        component.headbandMaterialNumber = int.Parse(array[num]);
        num++;
        component.sleeveMaterialNumber = int.Parse(array[num]);
        num++;
        string[] array10 = array[num].Split(',');
        num++;
        string[] array11 = array[num].Split(',');
        num++;
        string[] array12 = array[num].Split(',');
        num++;
        string[] array13 = array[num].Split(',');
        num++;
        string[] array14 = array[num].Split(',');
        num++;
        string[] array15 = array[num].Split(',');
        num++;
        string[] array16 = array[num].Split(',');
        num++;
        component.headX = float.Parse(array[num]);
        num++;
        component.headY = float.Parse(array[num]);
        num++;
        component.headZ = float.Parse(array[num]);
        num++;
        component.neckX = float.Parse(array[num]);
        num++;
        component.neckY = float.Parse(array[num]);
        num++;
        component.neckZ = float.Parse(array[num]);
        num++;
        component.torsoX = float.Parse(array[num]);
        num++;
        component.torsoY = float.Parse(array[num]);
        num++;
        component.torsoZ = float.Parse(array[num]);
        num++;
        component.armX = float.Parse(array[num]);
        num++;
        component.armY = float.Parse(array[num]);
        num++;
        component.armZ = float.Parse(array[num]);
        num++;
        component.legX = float.Parse(array[num]);
        num++;
        component.legY = float.Parse(array[num]);
        num++;
        component.legZ = float.Parse(array[num]);
        num++;
        component.feetX = float.Parse(array[num]);
        num++;
        component.feetY = float.Parse(array[num]);
        num++;
        component.feetZ = float.Parse(array[num]);
        num++;
        component.height = float.Parse(array[num]);
        num++;
        component.weight = float.Parse(array[num]);
        num++;
        component.animatorNum = int.Parse(array[num]);
        num++;
        component.leadLeg = int.Parse(array[num]);
        num++;
        component.armSpeedFlexL = float.Parse(array[num]);
        num++;
        component.armSpeedExtendL = float.Parse(array[num]);
        num++;
        component.armSpeedFlexR = float.Parse(array[num]);
        num++;
        component.armSpeedExtendR = float.Parse(array[num]);
        num++;
        component.setPaths(component.pathLength + 500);
        for (int i = 0; i < component.pathLength; i++)
        {
            component.velMagPath[i] = float.Parse(array4[i]);
            component.velPathY[i] = float.Parse(array5[i]);
            component.posPathY[i] = float.Parse(array6[i]);
            component.posPathZ[i] = float.Parse(array7[i]);
            component.rightInputPath[i] = int.Parse(array8[i]);
            component.leftInputPath[i] = int.Parse(array9[i]);
        }
        float[] array17 = new float[3];
        float[] array18 = new float[3];
        float[] array19 = new float[3];
        float[] array20 = new float[3];
        float[] array21 = new float[3];
        float[] array22 = new float[3];
        float[] array23 = new float[3];
        for (int j = 0; j < 3; j++)
        {
            array17[j] = float.Parse(array10[j]);
            array18[j] = float.Parse(array11[j]);
            array19[j] = float.Parse(array12[j]);
            array20[j] = float.Parse(array13[j]);
            array21[j] = float.Parse(array14[j]);
            array22[j] = float.Parse(array15[j]);
            array23[j] = float.Parse(array16[j]);
        }
        component.dummyRGB = array17;
        component.topRGB = array18;
        component.bottomsRGB = array19;
        component.shoesRGB = array20;
        component.socksRGB = array21;
        component.headbandRGB = array22;
        component.sleeveRGB = array23;
        component.setClothing(PlayerAttributes.FROM_THIS);
        component.setBodyProportions(PlayerAttributes.FROM_THIS);
        component.setStats(PlayerAttributes.FROM_THIS);
        component.setAnimations(PlayerAttributes.FROM_THIS);
        return gameObject;
    }

    public void loadBots()
    {
        int num = setupManager.botCount - raceManager.botCount;
        for (int i = 0; i < num; i++)
        {
            racers_backEnd.Add(getRandomBot());
        }
    }

    public GameObject getRandomBot()
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(racerPrefab);
        gameObject.tag = "Bot (Back End)";
        gameObject.transform.SetParent(raceManager.RacersBackEndParent.transform);
        gameObject.SetActive(value: false);
        PlayerAttributes component = gameObject.GetComponent<PlayerAttributes>();
        component.setInfo(PlayerAttributes.RANDOM);
        component.finishTime = -1f;
        component.personalBests = new float[4] { -1f, -1f, -1f, -1f };
        component.setPaths(PlayerAttributes.DEFAULT_PATH_LENGTH);
        component.pathLength = PlayerAttributes.DEFAULT_PATH_LENGTH;
        component.setClothing(PlayerAttributes.RANDOM);
        component.setBodyProportions(PlayerAttributes.RANDOM);
        component.setStats(PlayerAttributes.RANDOM);
        component.setAnimations(PlayerAttributes.RANDOM);
        return gameObject;
    }

    private void doTasks()
    {
        for (int i = 0; i < taskManager.tasks.Count; i++)
        {
            int num = taskManager.tasks[i];
            if (num == TaskManager.SAVE_PLAYER)
            {
                savePlayer(lbUpdate);
            }
            else if (num == TaskManager.LOAD_SELECTED_PLAYER)
            {
                loadSelectedPlayer();
            }
            else if (num == TaskManager.LOAD_SELECTED_RACERS)
            {
                loadSelectedRacers();
            }
            else if (num == TaskManager.SETUP_REPLAY)
            {
                setupReplay();
            }
            else if (num == TaskManager.CREATE_RACER)
            {
                createRacer();
                if (!hasInitialSetup)
                {
                    CharacterCreatorScreen.transform.Find("Back Button").gameObject.SetActive(value: true);
                }
            }
            else if (num == TaskManager.CLEAR_RACERS_FROM_SCENE)
            {
                clearRacersFromScene();
            }
            else if (num == TaskManager.SAVE_USER_PB)
            {
                saveUserPB(raceManager.raceEvent, raceManager.userPB_time);
            }
            else if (num == TaskManager.SET_USER_RECORD)
            {
                setUserRecord(raceManager.raceEvent, raceManager.userPB_time, raceManager.player.GetComponent<PlayerAttributes>().racerName);
            }
            else if (num == TaskManager.SAVE_DEFAULT_CAMERA_MODES)
            {
                PlayerPrefs.SetInt("Camera Gameplay Mode", cameraController.cameraGameplayMode);
                PlayerPrefs.SetInt("Camera Replay Mode", cameraController.cameraReplayMode);
            }
        }
        taskManager.tasks.Clear();
    }
    private void createRacerManually()
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(racerPrefab);
        gameObject.tag = "Player";
        gameObject.SetActive(false);

        PlayerAttributes component = gameObject.GetComponent<PlayerAttributes>();

        // --- Generate random name instead of using input field ---
        string racerName = TextReader.getRandomName(); // <-- assume you already have this utility
        component.racerName = racerName;

        component.id = PlayerAttributes.generateID(component.racerName, PlayFabManager.thisUserDisplayName);

        // Copy preview attributes (same as normal racer creation)
        component.copyAttributesFromOther(previewRacer_creation, "clothing");
        component.copyAttributesFromOther(previewRacer_creation, "body proportions");

        // Random stats/animations
        component.setStats(PlayerAttributes.RANDOM);
        component.setAnimations(PlayerAttributes.RANDOM);

        // Default setup
        component.pathLength = 0;
        component.setPaths(PlayerAttributes.DEFAULT_PATH_LENGTH);
        component.finishTime = -1f;
        component.personalBests = new float[4] { -1f, -1f, -1f, -1f };

        // Save racer
        saveRacer(gameObject, -1, new string[1] { PLAYABLE_RACER_MEMORY }, sendToLeaderboard: false, forReplay: false);

        // Destroy prefab instance (button keeps data)
        UnityEngine.Object.Destroy(gameObject);

        // Add to selection list
        GameObject obj = playerSelectButtonList.GetComponent<SelectionListScript>()
            .addButton(component.id, selectedRaceEvent, SelectionButtonScript.playerButtonColor);

        playerSelectButtonList.numSelected = 1;
        playerSelectButtonList.toggleAllOff();
        obj.GetComponent<SelectionButtonScript>().toggle();

        unlockManager.fillCharacterSlot();

        if (!hasInitialSetup)
        {
            PlayerPrefs.SetInt("hasInitialSetup", 1);
        }
    }

    private void createRacer()
    {
        GameObject gameObject = UnityEngine.Object.Instantiate(racerPrefab);
        gameObject.tag = "Player";
        gameObject.SetActive(value: false);
        PlayerAttributes component = gameObject.GetComponent<PlayerAttributes>();
        string racerName = legalizeEnteredName(nameInputField.gameObject.transform.Find("Text").GetComponent<Text>().text);
        component.racerName = racerName;
        component.id = PlayerAttributes.generateID(component.racerName, PlayFabManager.thisUserDisplayName);
        component.copyAttributesFromOther(previewRacer_creation, "clothing");
        component.copyAttributesFromOther(previewRacer_creation, "body proportions");
        component.setStats(PlayerAttributes.RANDOM);
        component.setAnimations(PlayerAttributes.RANDOM);
        component.pathLength = 0;
        component.setPaths(PlayerAttributes.DEFAULT_PATH_LENGTH);
        component.finishTime = -1f;
        component.personalBests = new float[4] { -1f, -1f, -1f, -1f };
        saveRacer(gameObject, -1, new string[1] { PLAYABLE_RACER_MEMORY }, sendToLeaderboard: false, forReplay: false);
        UnityEngine.Object.Destroy(gameObject);
        GameObject obj = playerSelectButtonList.GetComponent<SelectionListScript>().addButton(component.id, selectedRaceEvent, SelectionButtonScript.playerButtonColor);
        playerSelectButtonList.numSelected = 1;
        playerSelectButtonList.toggleAllOff();
        obj.GetComponent<SelectionButtonScript>().toggle();
        unlockManager.fillCharacterSlot();
        if (!hasInitialSetup)
        {
            PlayerPrefs.SetInt("hasInitialSetup", 1);
        }

        //createRacerManually();
    }

    private void savePlayer(bool sendToLeaderboard)
    {
        string id = raceManager.player.GetComponent<PlayerAttributes>().id;
        playerSelectButtonList.removeButton(id);
        saveRacer(raceManager.player, selectedRaceEvent, new string[2] { PLAYABLE_RACER_MEMORY, SAVED_RACER_MEMORY }, sendToLeaderboard, forReplay: false);
        GameObject gameObject = playerSelectButtonList.addButton(id, selectedRaceEvent, SelectionButtonScript.playerButtonColor);
        gameObject.GetComponent<SelectionButtonScript>().toggle();
        bool flag = false;
        if (ghostSelectButtonList.getButton(id) != null)
        {
            flag = ghostSelectButtonList.getButton(id).GetComponent<SelectionButtonScript>().selected;
        }
        ghostSelectButtonList.removeButton(id);
        gameObject = ghostSelectButtonList.addButton(id, selectedRaceEvent, SelectionButtonScript.ghostButtonColor);
        if (flag)
        {
            gameObject.GetComponent<SelectionButtonScript>().toggle(setting: true);
        }
        lbUpdate = false;
    }

    private void loadSelectedPlayer()
    {
        GameObject grid = playerSelectButtonList.grid;
        string text = "";
        foreach (Transform item in grid.transform)
        {
            SelectionButtonScript component = item.gameObject.GetComponent<SelectionButtonScript>();
            if (component.selected)
            {
                text = component.id;
                break;
            }
        }
        if (text == "")
        {
            SelectionButtonScript component = playerSelectButtonList.getFirst().GetComponent<SelectionButtonScript>();
            text = component.id;
        }
        GameObject gameObject = loadRacer(text, selectedRaceEvent, "Player (Back End)", fromLeaderboard: false, forReplay: false);
        gameObject.GetComponent<PlayerAttributes>().setPaths(PlayerAttributes.DEFAULT_PATH_LENGTH);
        gameObject.SetActive(value: false);
        gameObject.transform.SetParent(raceManager.RacersBackEndParent.transform);
        racers_backEnd.Add(gameObject);
    }

    private void loadSelectedRacers()
    {
        foreach (Transform item in ghostSelectButtonList.grid.transform)
        {
            SelectionButtonScript component = item.gameObject.GetComponent<SelectionButtonScript>();
            if (component.selected)
            {
                GameObject gameObject = loadRacer(component.id, selectedRaceEvent, "Ghost (Back End)", fromLeaderboard: false, forReplay: false);
                gameObject.transform.SetParent(raceManager.RacersBackEndParent.transform);
                racers_backEnd.Add(gameObject);
            }
        }
    }

    private void setupReplay()
    {
        int count = racers.Count;
        clearListAndObjects(racers_backEnd_replay);
        for (int i = 0; i < count; i++)
        {
            GameObject gameObject = racers[i];
            if (gameObject.GetComponent<PlayerAttributes>().finishTime != -1f)
            {
                string id = gameObject.GetComponent<PlayerAttributes>().id;
                saveRacer(gameObject, selectedRaceEvent, new string[1] { PLAYABLE_RACER_MEMORY }, sendToLeaderboard: false, forReplay: true);
                GameObject gameObject2 = loadRacer(id, selectedRaceEvent, "Ghost (Back End)", fromLeaderboard: false, forReplay: true);
                gameObject2.SetActive(value: false);
                gameObject2.transform.SetParent(raceManager.RacersBackEndParent.transform);
                gameObject2.GetComponent<PlayerAttributes>().lane = racers[i].GetComponent<PlayerAttributes>().lane;
                racers_backEnd_replay.Add(gameObject2);
                forgetRacer(id, new string[1] { PLAYABLE_RACER_MEMORY }, forReplay: true);
                if (gameObject.tag == "Player")
                {
                    playerIndex = racers_backEnd_replay.Count - 1;
                }
            }
        }
    }

    public void clearRacersFromScene()
    {
        clearListAndObjects(racers);
        clearListAndObjects(racers_backEnd);
        clearListAndObjects(racers_backEnd_replay);
    }

    private static void saveUserPB(int raceEvent, float time)
    {
        if (raceEvent == RaceManager.RACE_EVENT_100M)
        {
            PlayerPrefs.SetFloat("user_PB_100m", time);
        }
        else if (raceEvent == RaceManager.RACE_EVENT_200M)
        {
            PlayerPrefs.SetFloat("user_PB_200m", time);
        }
        else if (raceEvent == RaceManager.RACE_EVENT_400M)
        {
            PlayerPrefs.SetFloat("user_PB_400m", time);
        }
        else if (raceEvent == RaceManager.RACE_EVENT_60M)
        {
            PlayerPrefs.SetFloat("user_PB_60m", time);
        }
    }

    public static float getUserPB(int raceEvent)
    {
        if (raceEvent == RaceManager.RACE_EVENT_100M)
        {
            return PlayerPrefs.GetFloat("user_PB_100m");
        }
        if (raceEvent == RaceManager.RACE_EVENT_200M)
        {
            return PlayerPrefs.GetFloat("user_PB_200m");
        }
        if (raceEvent == RaceManager.RACE_EVENT_400M)
        {
            return PlayerPrefs.GetFloat("user_PB_400m");
        }
        if (raceEvent == RaceManager.RACE_EVENT_60M)
        {
            return PlayerPrefs.GetFloat("user_PB_60m");
        }
        return 0f;
    }

    public static void setUserRecord(int raceEvent, float time, string racerName)
    {
        if (raceEvent == RaceManager.RACE_EVENT_100M)
        {
            PlayerPrefs.SetString("wr_local_100m", time + ":" + racerName);
        }
        else if (raceEvent == RaceManager.RACE_EVENT_200M)
        {
            PlayerPrefs.SetString("wr_local_200m", time + ":" + racerName);
        }
        else if (raceEvent == RaceManager.RACE_EVENT_400M)
        {
            PlayerPrefs.SetString("wr_local_400m", time + ":" + racerName);
        }
        else if (raceEvent == RaceManager.RACE_EVENT_60M)
        {
            PlayerPrefs.SetString("wr_local_60m", time + ":" + racerName);
        }
    }


    public static float getUserRecordTime(int raceEvent)
    {
        string key = "";

        if (raceEvent == RaceManager.RACE_EVENT_100M)
            key = "wr_local_100m";
        else if (raceEvent == RaceManager.RACE_EVENT_200M)
            key = "wr_local_200m";
        else if (raceEvent == RaceManager.RACE_EVENT_400M)
            key = "wr_local_400m";
        else if (raceEvent == RaceManager.RACE_EVENT_60M)
            key = "wr_local_60m";
        else
            return 0f; // unknown race type

        // If no saved record yet return 0
        if (!PlayerPrefs.HasKey(key))
            return 0f;

        string raw = PlayerPrefs.GetString(key);

        if (string.IsNullOrEmpty(raw))
            return 0f;

        string[] parts = raw.Split(':');

        if (parts.Length == 0 || string.IsNullOrEmpty(parts[0]))
            return 0f;

        if (float.TryParse(parts[0], out float result))
            return result;

        return 0f; // fallback
    }

    //public static float getUserRecordTime(int raceEvent)
    //{
    //    if (raceEvent == RaceManager.RACE_EVENT_100M)
    //    {
    //        return float.Parse(PlayerPrefs.GetString("wr_local_100m").Split(':')[0]);
    //    }
    //    if (raceEvent == RaceManager.RACE_EVENT_200M)
    //    {
    //        return float.Parse(PlayerPrefs.GetString("wr_local_200m").Split(':')[0]);
    //    }
    //    if (raceEvent == RaceManager.RACE_EVENT_400M)
    //    {
    //        return float.Parse(PlayerPrefs.GetString("wr_local_400m").Split(':')[0]);
    //    }
    //    if (raceEvent == RaceManager.RACE_EVENT_60M)
    //    {
    //        return float.Parse(PlayerPrefs.GetString("wr_local_60m").Split(':')[0]);
    //    }
    //    return 0f;
    //}

    public static string getUserRecordName(int raceEvent)
    {
        string key = "";

        if (raceEvent == RaceManager.RACE_EVENT_100M)
            key = "wr_local_100m";
        else if (raceEvent == RaceManager.RACE_EVENT_200M)
            key = "wr_local_200m";
        else if (raceEvent == RaceManager.RACE_EVENT_400M)
            key = "wr_local_400m";
        else if (raceEvent == RaceManager.RACE_EVENT_60M)
            key = "wr_local_60m";
        else
            return "";

        // If no saved record yet
        if (!PlayerPrefs.HasKey(key))
            return "";

        string raw = PlayerPrefs.GetString(key);

        if (string.IsNullOrEmpty(raw))
            return "";

        string[] parts = raw.Split(':');

        if (parts.Length < 2 || string.IsNullOrEmpty(parts[1]))
            return "";

        return parts[1]; // return racer name
    }

    //public static string getUserRecordName(int raceEvent)
    //{
    //    if (raceEvent == RaceManager.RACE_EVENT_100M)
    //    {
    //        return PlayerPrefs.GetString("wr_local_100m").Split(':')[1];
    //    }
    //    if (raceEvent == RaceManager.RACE_EVENT_200M)
    //    {
    //        return PlayerPrefs.GetString("wr_local_200m").Split(':')[1];
    //    }
    //    if (raceEvent == RaceManager.RACE_EVENT_400M)
    //    {
    //        return PlayerPrefs.GetString("wr_local_400m").Split(':')[1];
    //    }
    //    if (raceEvent == RaceManager.RACE_EVENT_60M)
    //    {
    //        return PlayerPrefs.GetString("wr_local_60m").Split(':')[1];
    //    }
    //    return "";
    //}

    public static void getWorldRecordInfo(int raceEvent)
    {
        PlayFabManager.userLeaderboardInfoRetrieved = false;
        PlayFabManager.getWorldRecordInfo(raceEvent);
    }

    public GameObject[] deactivateAllButtons()
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag("Button");
        GameObject[] array2 = array;
        for (int i = 0; i < array2.Length; i++)
        {
            array2[i].GetComponent<Button>().interactable = false;
        }
        return array;
    }

    public void activateButtons(GameObject[] objs)
    {
        foreach (GameObject gameObject in objs)
        {
            if (gameObject != null)
            {
                gameObject.GetComponent<Button>().interactable = true;
            }
        }
    }

    public static string legalizeEnteredName(string str)
    {
        str = Regex.Replace(str, "[^a-zA-Z0-9 ]", "");
        return str;
    }

    public void exitGame()
    {
        if (PlayFabManager.loggedIn)
        {
            PlayFabManager.logout();
        }
        Application.Quit();
    }

    public IEnumerator handleLogin(bool successMsg, bool failMsg)
    {
        if (!SteamManager.Initialized)
        {
            yield break;
        }
        PlayFabManager.loggedIn = false;
        PlayFabManager.login();
        yield return new WaitUntil(() => PlayFabManager.loggedIn || PlayFabManager.loginError);
        if (PlayFabManager.loginError)
        {
            if (failMsg)
            {
                string message = "Could not log in with Steam. Scores will be saved locally.";
                StartCoroutine(showNotificationBlip(message, 4f));
            }
            yield break;
        }
        if (successMsg)
        {
            string message2 = "Logged in to Steam!";
            StartCoroutine(showNotificationBlip(message2, 4f));
        }
        PlayFabManager.setUserDisplayName(steamController.getThisUserSteamName());
        sendUnsentScores();
    }

    private void sendUnsentScores()
    {
        for (int i = 0; i < 4; i++)
        {
            string @string = PlayerPrefs.GetString("UnsentScore " + i, "NA");
            if (@string != "NA")
            {
                StartCoroutine(handleLeaderboardSend(@string, i));
            }
        }
    }

    public IEnumerator showNotificationBlip(string message, float time)
    {
        notificationBlipController.setText(message);
        notificationBlipController.show();
        yield return new WaitForSeconds(time);
        notificationBlipController.hide();
    }

    private void runFirstTimeOperations()
    {
        saveUserPB(RaceManager.RACE_EVENT_100M, float.MaxValue);
        saveUserPB(RaceManager.RACE_EVENT_200M, float.MaxValue);
        saveUserPB(RaceManager.RACE_EVENT_400M, float.MaxValue);
        saveUserPB(RaceManager.RACE_EVENT_60M, float.MaxValue);
        setUserRecord(RaceManager.RACE_EVENT_100M, float.MaxValue, "None");
        setUserRecord(RaceManager.RACE_EVENT_200M, float.MaxValue, "None");
        setUserRecord(RaceManager.RACE_EVENT_400M, float.MaxValue, "None");
        setUserRecord(RaceManager.RACE_EVENT_60M, float.MaxValue, "None");
        PlayerPrefs.SetInt("Total Score1", 0);
        PlayerPrefs.SetInt("Total Score (Best Racer)1", 0);
        PlayerPrefs.SetFloat("Audio Volume", 0.04f);
        PlayerPrefs.SetInt("Camera Gameplay Mode", CameraController.SIDE_RIGHT);
        PlayerPrefs.SetInt("Camera Replay Mode", CameraController.TV);
        PlayerPrefs.SetFloat("Camera Distance", 0.5f);
        PlayerPrefs.SetInt("Controls Left", 276);
        PlayerPrefs.SetInt("Controls Right", 275);
        PlayerPrefs.SetString("Display Mode", "Full screen");
        PlayerPrefs.SetInt("Character Slots", 1);
        PlayerPrefs.SetInt("Downloaded Ghosts", 0);
        PlayerPrefs.SetInt("Theme", EnvironmentController.SUNNY);
    }

    private IEnumerator flash()
    {
        Color c = new Color32(byte.MaxValue, 217, 0, 0);
        screenFlash.color = c;
        while (c.a < 0.2f)
        {
            c.a += 3f * Time.deltaTime / Time.timeScale;
            screenFlash.color = c;
            yield return null;
        }
        while (c.a > 0f)
        {
            c.a -= 0.35f * Time.deltaTime / Time.timeScale;
            screenFlash.color = c;
            yield return null;
        }
        c.a = 0f;
        screenFlash.color = c;
    }

    private IEnumerator wait(float t)
    {
        yield return new WaitForSeconds(t);
    }
}
