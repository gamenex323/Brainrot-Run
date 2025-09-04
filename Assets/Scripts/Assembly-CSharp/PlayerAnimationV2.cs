using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerAnimationV2 : MonoBehaviour
{
    public GameObject gyro;

    public Transform hudIndicatorT;

    public MeshRenderer hudIndicatorRenderer;

    public GameObject marker;

    public Transform root;

    public Rigidbody rb;

    public BoxCollider chestCollider;

    public Animator animator;

    public Transform rootTransform;

    public Transform headT;

    public Transform rightFoot;

    public Transform leftFoot;

    public Transform rightUpperArm;

    public Transform leftUpperArm;

    public Transform pushLeg;

    public Quaternion pushRotation;

    public ParticleSystem sweatParticles;

    public ParticleSystem popParticles;

    public ParticleSystem dustParticles;

    private bool showDust;

    private Vector3 rightFootPos_lastFrame;

    private Vector3 leftFootPos_lastFrame;

    private float rightFootVel_lastFrame;

    private float leftFootVel_lastFrame;

    public static int Set = 1;

    public static int Run = 2;

    public int mode;

    private int tick;

    public float transitionSpeed;

    public float minWeight;

    public float jumpForce;


    public GlobalController globalController;

    public RaceManager raceManager;

    public PlayerAttributes attributes;

    public OrientationController oc;

    public TimerController timer;

    public EnergyMeterController emc;

    public RacerFootV2[] feet;

    public RacerFootV2 rightFootScript;

    public RacerFootV2 leftFootScript;

    public bool rightInput;

    public bool leftInput;

    public bool isPlayer;

    public bool finished;

    public bool onCurve;

    public bool launchFlag;

    private Vector3 friction;

    private bool canFalseStart;

    public float frictionMagnitude;

    public float torsoAngle;

    public float torsoAngle_upright;

    public float torsoAngle_max;

    public float power;

    public float powerMod;

    private float knee_dominance;

    public float driveModifier;

    public float launch_power;

    private float curve_power;

    private float cruise;

    public float maxPower;

    public float maxUpSpeed;

    private float topSpeed;

    private Vector3 velocityLastFrame;

    private Vector3 velocityLastFrame_relative;

    public float speedHoriz;

    public float speedHoriz_lastFrame;

    public float energy;

    public float fitness;

    private float energyCost_base;

    private float energyBurnoutMod;

    private float energyBurnoutThreshold;

    public int pathLength;

    public float[] velMagPath;

    public float[] velPathY;

    public float[] posPathY;

    public float[] posPathZ;

    public bool leans;

    public float zTilt;

    public bool leanLock;

    public int leanLockTick;

    private float leanThreshold;

    public float quickness;

    public float quickness_base;

    public float quicknessMod;

    public float turnover;

    public float armFlexL;

    public float armExtendL;

    public float armFlexR;

    public float armExtendR;

    public int leadLeg;

    public bool upInSet;

    public float setPositionWeight;

    public float leanWeight;

    public float runWeight;

    public float driveWeight;

    public float cruiseWeight;

    private float dTime;


    private bool uiLeftPressed;
    private bool uiRightPressed;



    #region HurdleRace
    private Vector2 touchStartPosition;
    private float minSwipeDistance = 50f; // Minimum swipe distance in pixels
    private bool swipeDetectedThisFrame;

    private void Update()
    {
        DetectSwipes();
    }

    private void DetectSwipes()
    {
        swipeDetectedThisFrame = false;

        // Mouse input for testing in editor
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 swipeDelta = (Vector2)Input.mousePosition - touchStartPosition;
            ProcessSwipe(swipeDelta);
        }
#endif

        // Touch input for mobile devices
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    Vector2 swipeDelta = touch.position - touchStartPosition;
                    ProcessSwipe(swipeDelta);
                    break;
            }
        }
    }

    private void ProcessSwipe(Vector2 swipeDelta)
    {
        // Check if it's a valid swipe (minimum distance)
        if (swipeDelta.magnitude < minSwipeDistance)
            return;

        // Normalize the swipe direction
        swipeDelta.Normalize();

        // Check for upward swipes with horizontal bias
        if (swipeDelta.y > 0.5f) // Upward component is strong
        {
            if (swipeDelta.x > 0.3f) // Right swipe up
            {
                DebugSwipe("Right Swipe Up Detected!");
                OnRightSwipeUp();
            }
            else if (swipeDelta.x < -0.3f) // Left swipe up
            {
                DebugSwipe("Left Swipe Up Detected!");
                OnLeftSwipeUp();
            }
            else // Straight up swipe
            {
                DebugSwipe("Straight Up Swipe Detected!");
                OnStraightSwipeUp();
            }

            swipeDetectedThisFrame = true;
        }
    }

    private void DebugSwipe(string message)
    {
        Debug.Log($"[SWIPE DEBUG] {message} | Time: {Time.time} | Speed: {speedHoriz} | Energy: {energy}");

        // Optional: Add visual feedback in the console with more details
        Debug.Log($"[SWIPE DETAILS] Position: {transform.position} | Velocity: {rb.velocity} | Mode: {(mode == Set ? "Set" : "Run")}");
    }

    private void OnRightSwipeUp()
    {
        // Add your right swipe up functionality here
        Debug.Log("Right swipe up action triggered!");

        // Example: Boost right side power temporarily
        // StartCoroutine(RightSideBoost());

        // Or trigger a special right-side animation
        // animator.SetTrigger("RightSwipeSpecial");
    }

    private void OnLeftSwipeUp()
    {
        // Add your left swipe up functionality here
        Debug.Log("Left swipe up action triggered!");

        // Example: Boost left side power temporarily
        // StartCoroutine(LeftSideBoost());

        // Or trigger a special left-side animation
        // animator.SetTrigger("LeftSwipeSpecial");
    }

    private void OnStraightSwipeUp()
    {
        // Add straight up swipe functionality
        Debug.Log("Straight up swipe action triggered!");

        // Example: Jump or special move
        if (mode == Run && (rightFootScript.groundContact || leftFootScript.groundContact))
        {
            // Trigger jump or hurdle
            hurdleWeight = 1f;
            if (rb != null && Mathf.Abs(rb.velocity.y) < 0.01f)
            {
                rb.AddForce(Vector3.up * jumpForce * 1.2f, ForceMode.VelocityChange);
            }
        }
    }

    // Example coroutine for swipe effects
    private IEnumerator RightSideBoost()
    {
        float originalPowerMod = powerMod;
        powerMod *= 1.3f; // 30% power boost
        Debug.Log("Right side boost activated!");

        yield return new WaitForSeconds(2f);

        powerMod = originalPowerMod;
        Debug.Log("Right side boost ended.");
    }

    private IEnumerator LeftSideBoost()
    {
        float originalQuicknessMod = quicknessMod;
        quicknessMod *= 1.25f; // 25% speed boost
        Debug.Log("Left side quickness boost activated!");

        yield return new WaitForSeconds(2f);

        quicknessMod = originalQuicknessMod;
        Debug.Log("Left side quickness boost ended.");
    }

    // Optional: Add swipe visualization in FixedUpdate for debugging

    #endregion



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

    private int baseLayer;
    private int CruiseLayer;
    private int SetPositionLayer;
    private int TosoUprightLayer;
    private int TorsoDriveLayer;
    private int RightLegURLayer;
    private int LeftLegURLayer;
    private int RightLegDRLayer;
    private int LeftLegDRLayer;
    private int RightArmURLayer;
    private int LeftArmURLayer;
    private int RightArmDRLayer;
    private int LeftArmDRLayer;
    private int TorsoHurdleLayer;
    private int RightLegHurdleLayer;
    private int LeftLegHurdleLayer;
    private int RightArmHurdleLayer;
    private int LeftArmHurdleLayer;

    private enum RacerType { Player, Ghost, Bot }
    private RacerType racerType;



    private void Start()
    {
        baseLayer = animator.GetLayerIndex("Base Layer");
        CruiseLayer = animator.GetLayerIndex("Cruise");
        SetPositionLayer = animator.GetLayerIndex("Set Position");
        TosoUprightLayer = animator.GetLayerIndex("Torso (upright)");
        TorsoDriveLayer = animator.GetLayerIndex("Torso (drive)");
        RightLegURLayer = animator.GetLayerIndex("Right Leg (upright)");
        LeftLegURLayer = animator.GetLayerIndex("Left Leg (upright)");
        RightLegDRLayer = animator.GetLayerIndex("Right Leg (drive)");
        LeftLegDRLayer = animator.GetLayerIndex("Left Leg (drive)");
        RightArmURLayer = animator.GetLayerIndex("Right Arm (upright)");
        LeftArmURLayer = animator.GetLayerIndex("Left Arm (upright)");
        RightArmDRLayer = animator.GetLayerIndex("Right Arm (drive)");
        LeftArmDRLayer = animator.GetLayerIndex("Left Arm (drive)");
        TorsoHurdleLayer = animator.GetLayerIndex("Torso(hurdle)");
        RightLegHurdleLayer = animator.GetLayerIndex("Right Leg (hurdle)");
        LeftLegHurdleLayer = animator.GetLayerIndex("Left Leg (hurdle)");
        RightArmHurdleLayer = animator.GetLayerIndex("Right Arm (hurdle)");
        LeftArmHurdleLayer = animator.GetLayerIndex("Left Arm (hurdle)");

        feet = new RacerFootV2[2] { rightFootScript, leftFootScript };

        DebugLayerWeights();



    }

    public void init(int raceEvent)
    {
        raceManager = globalController.raceManager;
        pathLength = attributes.pathLength;
        velMagPath = attributes.velMagPath;
        velPathY = attributes.velPathY;
        posPathY = attributes.posPathY;
        posPathZ = attributes.posPathZ;
        maxPower = attributes.POWER;
        turnover = attributes.TURNOVER;
        quickness_base = attributes.QUICKNESS;
        fitness = attributes.FITNESS;
        knee_dominance = attributes.KNEE_DOMINANCE;
        launch_power = attributes.LAUNCH_POWER;
        curve_power = attributes.CURVE_POWER;
        cruise = attributes.CRUISE;
        leadLeg = attributes.leadLeg;
        animator.SetInteger(AnimHashes.LeadLeg, leadLeg);
        armFlexL = attributes.armSpeedFlexL;
        armExtendL = attributes.armSpeedExtendL;
        armFlexR = attributes.armSpeedFlexL;
        armExtendR = attributes.armSpeedExtendL;
        driveModifier = 1f * Mathf.Pow(knee_dominance, 1.2f) * Mathf.Pow(2f - (cruise + 0.5f), 0.25f);
        leanThreshold = 0.825f * knee_dominance;
        torsoAngle_max = 355f;
        torsoAngle_upright = 320f * Mathf.Pow(knee_dominance, 0.01f);
        energyCost_base = 48f;
        energyBurnoutMod = 1f;
        energyBurnoutThreshold = 20f;
        showDust = false;
        if (base.gameObject.tag.StartsWith("Player"))
        {
            isPlayer = true;
            hudIndicatorRenderer.material = globalController.hudIndicatorMat_player;
            if (raceManager.viewMode == RaceManager.VIEW_MODE_LIVE)
            {
                canFalseStart = true;
            }
        }
        else
        {
            isPlayer = false;
            hudIndicatorRenderer.material = globalController.hudIndicatorMat_nonPlayer;
            canFalseStart = false;
        }
        leftFootScript.raceManager = raceManager;
        rightFootScript.raceManager = raceManager;
        quicknessMod = 1f;
        powerMod = 1f;
        leanLock = false;

        if (CompareTag("Player"))
            racerType = RacerType.Player;
        else if (CompareTag("Ghost"))
            racerType = RacerType.Ghost;
        else if (CompareTag("Bot"))
            racerType = RacerType.Bot;
    }

    public void FixedUpdate()
    {







        animator.SetBool(AnimHashes.groundContact, rightFootScript.groundContact || leftFootScript.groundContact);

        dTime = 1f / 90f;

        speedHoriz = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;

        if (mode == Set)
        {
            setPositionMode();
        }
        else if (mode == Run)
        {
            runMode();
        }

        adjustRotation();
        updateLayerWeights();
        applyFriction();
        applyPowerModifiers();
        applySpeedModifiers();
        velocityLastFrame = rb.velocity;
        velocityLastFrame_relative = gyro.transform.InverseTransformDirection(rb.velocity);
        speedHoriz_lastFrame = speedHoriz;
        rightFootPos_lastFrame = rightFoot.position;
        leftFootPos_lastFrame = leftFoot.position;






        // Your existing FixedUpdate code...
        animator.SetBool(AnimHashes.groundContact, rightFootScript.groundContact || leftFootScript.groundContact);

        dTime = 1f / 90f;
        speedHoriz = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;

        if (mode == Set)
        {
            setPositionMode();
        }
        else if (mode == Run)
        {
            runMode();
        }

        adjustRotation();
        updateLayerWeights();
        applyFriction();
        applyPowerModifiers();
        applySpeedModifiers();
        velocityLastFrame = rb.velocity;
        velocityLastFrame_relative = gyro.transform.InverseTransformDirection(rb.velocity);
        speedHoriz_lastFrame = speedHoriz;
        rightFootPos_lastFrame = rightFoot.position;
        leftFootPos_lastFrame = leftFoot.position;

        // Reset swipe flag for next frame
        if (swipeDetectedThisFrame)
        {
            swipeDetectedThisFrame = false;
        }
    }

    private void runMode()
    {
        if (raceManager.raceEvent >= 2)
        {
            if (!finished)
            {
                oc.updateOrientation(enforcePosition: true);
            }
            if (oc.trackSegment == 1 || oc.trackSegment == 3)
            {
                if (!onCurve)
                {
                    animator.SetBool(AnimHashes.onCurve, value: true);
                }
                onCurve = true;
            }
            else
            {
                if (onCurve)
                {
                    animator.SetBool(AnimHashes.onCurve, value: false);
                }
                onCurve = false;
            }
        }


        torsoAngle = root.rotation.eulerAngles.x;

        quickness = quickness_base * speedHoriz * 0.25f;
        if (quickness > quickness_base)
        {
            if (quickness > quickness_base * 1.35f)
            {
                quickness = quickness_base * 1.35f;
            }
        }
        else
        {
            quickness = quickness_base;
        }
        if (energy < 70f)
        {
            quickness *= Mathf.Pow(energy / 70f, 0.075f);
        }
        if (quickness < 0.95f)
        {
            quickness = 0.95f;
        }
        float num = quickness * quicknessMod;
        animator.SetFloat(AnimHashes.LimbSpeed, num);
        animator.SetFloat(AnimHashes.ArmFlexL, armFlexL * num);
        animator.SetFloat(AnimHashes.ArmExtendL, armExtendL * num);
        animator.SetFloat(AnimHashes.ArmFlexR, armFlexR * num);
        animator.SetFloat(AnimHashes.ArmExtendR, armExtendR * num);
        if (speedHoriz >= 5f)
        {
            if (!showDust)
            {
                showDust = true;
                dustParticles.Play();
            }
        }
        else if (showDust)
        {
            showDust = false;
            dustParticles.Stop();
        }
        if (setPositionWeight >= 0f)
        {
            setPositionWeight -= transitionSpeed * quickness_base * dTime;
        }
        else
        {
            setPositionWeight = 0f;
        }

        //runWeight += transitionSpeed * quickness_base * dTime;

        if (leftInput || rightInput) // running condition
        {
            runWeight += transitionSpeed * quickness_base * dTime; // fade in
            if (runWeight > 1f) runWeight = 1f;
        }
        else // idle condition
        {
            runWeight -= transitionSpeed * quickness_base * dTime; // fade out
            if (runWeight < minWeight) runWeight = minWeight;
        }

    }

    public void setPositionMode()
    {
        animator.SetBool(AnimHashes.UpInSet, upInSet);
        setPositionWeight = 1f;
        runWeight = 0f;
    }

    float hurdleWeight = 0;

    public void readInput(int tick)
    {
        if (gameObject == null)
            return;

        if (racerType == RacerType.Player)
        {
#if UNITY_EDITOR
            print("Right Arrow");
            rightInput = Input.GetKey(KeyCode.RightArrow);
            leftInput = Input.GetKey(KeyCode.LeftArrow);
#else
            rightInput = uiRightPressed;
            leftInput = uiLeftPressed;
#endif

            // Smooth fade hurdle layer in/out
            if (Input.GetKey(KeyCode.UpArrow)) // or Input.GetButton("Jump")
            {
                hurdleWeight = Mathf.MoveTowards(
                    animator.GetLayerWeight(TorsoHurdleLayer), 1f, dTime * 5f);
            }
            else
            {
                hurdleWeight = Mathf.MoveTowards(
                    animator.GetLayerWeight(TorsoHurdleLayer), 0f, dTime * 5f);
            }
        }
        else if (racerType == RacerType.Ghost || racerType == RacerType.Bot)
        {
            if (tick < pathLength)
            {
                rightInput = attributes.rightInputPath[tick] == 1;
                leftInput = attributes.leftInputPath[tick] == 1;
            }
        }
    }

    //    public void readInput(int tick)
    //    {
    //        if (base.gameObject == null)
    //            return;

    //        if (base.tag == "Player")
    //        {
    //#if UNITY_EDITOR
    //            rightInput = Input.GetKey(SettingsManager.controlsRight);
    //            leftInput = Input.GetKey(SettingsManager.controlsLeft);
    //#else
    //            rightInput = uiRightPressed;
    //            leftInput = uiLeftPressed;
    //#endif

    //            if (Input.GetKey(KeyCode.UpArrow)) // or Input.GetButton("Jump")
    //            {
    //                hurdleWeight = Mathf.MoveTowards(
    //                    animator.GetLayerWeight(TorsoHurdleLayer), 1f, dTime * 5f); // smooth fade in
    //            }
    //            else
    //            {
    //                hurdleWeight = Mathf.MoveTowards(
    //                    animator.GetLayerWeight(TorsoHurdleLayer), 0f, dTime * 5f); // smooth fade out
    //            }
    //        }
    //        else if ((base.tag == "Ghost" || base.tag == "Bot") && tick < pathLength)
    //        {
    //            rightInput = attributes.rightInputPath[tick] == 1;
    //            leftInput = attributes.leftInputPath[tick] == 1;
    //        }
    //    }

    public void applyInput(int tick)
    {
        if (mode == Set)
        {
            if (rightInput || leftInput)
            {
                mode = Run;
                if (canFalseStart && tick <= 1)
                {
                    StartCoroutine(raceManager.falseStart());
                }
            }
        }
        else
        {
            if (mode != Run)
            {
                return;
            }

            // Any input at all
            if (rightInput || leftInput)
            {
                animator.SetBool(AnimHashes.Input, true);
            }
            else
            {
                animator.SetBool(AnimHashes.Input, false);
            }


            // --- Normal right foot logic ---
            if (rightInput)
            {
                rightFootScript.input = true;
                animator.SetBool(AnimHashes.Right, true);

                if (!launchFlag)
                {
                    StartCoroutine(launch(leadLeg == 1));
                    launchFlag = true;
                }
            }
            else
            {
                rightFootScript.input = false;
                animator.SetBool(AnimHashes.Right, false);
            }

            // --- Normal left foot logic ---
            if (leftInput)
            {
                leftFootScript.input = true;
                animator.SetBool(AnimHashes.Left, true);

                if (!launchFlag)
                {
                    StartCoroutine(launch(leadLeg == 0));
                    launchFlag = true;
                }
            }
            else
            {
                leftFootScript.input = false;
                animator.SetBool(AnimHashes.Left, false);
            }
        }

    }


    //public void applyInput(int tick)
    //{
    //    if (mode == Set)
    //    {
    //        if (rightInput || leftInput)
    //        {
    //            mode = Run;
    //            if (canFalseStart && tick <= 1)
    //            {
    //                StartCoroutine(raceManager.falseStart());
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (mode != Run)
    //        {
    //            return;
    //        }
    //        if (rightInput || leftInput)
    //        {
    //            animator.SetBool("input", value: true);
    //        }
    //        else
    //        {
    //            animator.SetBool("input", value: false);
    //        }
    //        if (rightInput)
    //        {
    //            rightFootScript.input = true;
    //            animator.SetBool("right", value: true);
    //            if (!launchFlag)
    //            {
    //                StartCoroutine(launch(leadLeg == 1));
    //                launchFlag = true;
    //            }
    //        }
    //        else
    //        {
    //            rightFootScript.input = false;
    //            animator.SetBool("right", value: false);
    //        }
    //        if (leftInput)
    //        {
    //            leftFootScript.input = true;
    //            animator.SetBool("left", value: true);
    //            if (!launchFlag)
    //            {
    //                StartCoroutine(launch(leadLeg == 0));
    //                launchFlag = true;
    //            }
    //        }
    //        else
    //        {
    //            leftFootScript.input = false;
    //            animator.SetBool("left", value: false);
    //        }
    //    }
    //}

    private void adjustRotation()
    {
        if (!leans)
        {
            return;
        }
        if (mode == Set)
        {
            leanWeight += 0.5f * dTime;
        }
        else if (mode == Run && !leanLock)
        {
            bool flag = false;
            bool[] array = new bool[2] { rightInput, leftInput };
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i])
                {
                    flag = true;
                    leanWeight += 0.009666667f;
                    root.Rotate(Vector3.right * 23f * (1f / 60f), Space.Self);
                    zTilt += 0.38333336f;
                }
            }
            if (!flag)
            {
                leanWeight -= 0.020000001f;
                root.Rotate(Vector3.left * 47f * (1f / 60f), Space.Self);
                zTilt -= 47f / 60f;
            }
        }
        if (leanWeight > 0f)
        {
            if (leanWeight > 1f)
            {
                leanWeight = 1f;
            }
        }
        else
        {
            leanWeight = 0f;
        }
        if (zTilt > 45f)
        {
            root.Rotate(Vector3.left * (zTilt - 45f), Space.Self);
            zTilt = 45f;
        }
        else if (zTilt < -45f)
        {
            root.Rotate(Vector3.left * (zTilt + 45f), Space.Self);
            zTilt = -45f;
        }
    }

    public void turnTowardsY(float y)
    {
        Vector3 eulerAngles = base.transform.rotation.eulerAngles;
        eulerAngles.y = y;
        base.transform.rotation = Quaternion.Euler(eulerAngles);
        eulerAngles = gyro.transform.rotation.eulerAngles;
        eulerAngles.y = y;
        gyro.transform.rotation = Quaternion.Euler(eulerAngles);
    }

    public void setPositionAndVelocity(int tick)
    {
        if (tick > attributes.leanLockTick && attributes.leanLockTick > 0)
        {
            leanLock = true;
        }
        else
        {
            leanLock = false;
        }
        if (tick >= pathLength)
        {
            return;
        }
        float maxLength = velMagPath[tick];
        float y = velPathY[tick];
        float y2 = posPathY[tick];
        float z = posPathZ[tick];
        Vector3 velocity = rb.velocity;
        velocity.y = 0f;
        velocity *= 100f;
        velocity = Vector3.ClampMagnitude(velocity, maxLength);
        velocity.y = y;
        rb.velocity = velocity;
        if (oc.trackSegment == 4)
        {
            Vector3 vector = new Vector3(base.transform.position.x, y2, z);
            if (Vector3.Distance(base.transform.position, globalController.raceManager.finishLine.transform.position) > 2f)
            {
                base.transform.position = Vector3.Lerp(base.transform.position, vector, 1f * dTime);
            }
            else
            {
                base.transform.position = vector;
            }
        }
    }


    public void updateEnergy(float speed, float swingTimeBonus)
    {
        float num = energyCost_base * dTime;
        num *= Mathf.Pow(speed / 27f, 1.5f);
        num *= 1f + (1f - swingTimeBonus / 2.0736f);
        num *= 1.5f - fitness;
        num *= 1f - cruiseWeight / 3f;

        num *= 0.25f;

        if (num < 0.05f)
        {
            num = 0.05f;
        }

        energy -= num;

        if (energy < 0f)
        {
            energy = 0f;
        }

        emc.adjustForEnergyLevel(energy);
    }


    //public void updateEnergy(float speed, float swingTimeBonus)
    //{
    //    float num = energyCost_base * dTime;
    //    num *= Mathf.Pow(speed / 27f, 2.5f);
    //    num *= 1f + (1f - swingTimeBonus / 2.0736f);
    //    num *= 2f - fitness;
    //    num *= 1f - cruiseWeight / 3f;
    //    if (num < 0.1f)
    //    {
    //        num = 0.1f;
    //    }
    //    energy -= num;
    //    if (energy < 0f)
    //    {
    //        energy = 0f;
    //    }
    //    emc.adjustForEnergyLevel(energy);
    //}

    private void applyFriction()
    {
        friction = gyro.transform.forward * -1f * speedHoriz * frictionMagnitude;
        if (rightFootScript.groundContact || leftFootScript.groundContact)
        {
            rb.AddForce(friction, ForceMode.Force);
        }
    }

    private void applyPowerModifiers()
    {
        float num = 1f;
        float num2 = maxPower;
        if (rightFootScript.groundContact && leftFootScript.groundContact)
        {
            num2 *= 0.2f;
        }
        float leanMagnitude = rightFootScript.leanMagnitude;
        if (leanMagnitude > leanThreshold)
        {
            num = leanThreshold / leanMagnitude;
            num *= num * num * num * num * num * num * num;
            num2 *= num;
        }
        if (torsoAngle < torsoAngle_upright)
        {
            num = torsoAngle / torsoAngle_upright;
            num *= num;
            num2 *= num;
        }
        if (onCurve)
        {
            num = 0.925f * curve_power;
            num2 *= num;
        }
        if (energy < 80f)
        {
            if (energy >= energyBurnoutThreshold)
            {
                num *= energy / 80f;
            }
            else
            {
                energyBurnoutMod = Mathf.Lerp(energyBurnoutMod, 0.5f, 0.25f * (speedHoriz / 27f) * dTime);
                num *= 50f * energyBurnoutMod / 80f;
            }
            num = Mathf.Pow(num, 0.95f);
            num2 *= num;
        }
        power = num2;
        power *= powerMod;
    }

    private void applySpeedModifiers()
    {
        Vector3 vector = rb.velocity;
        float y = vector.y;
        vector.y = 0f;
        if (gyro.transform.forward.z > 0f)
        {
            if (vector.z < 0f)
            {
                vector.z = 0f;
            }
        }
        else if (vector.z > 0f)
        {
            vector.z = 0f;
        }
        float num = 0.5f * driveModifier + speedHoriz_lastFrame;
        if (speedHoriz > num)
        {
            vector = Vector3.ClampMagnitude(vector, num);
        }
        if (y > maxUpSpeed)
        {
            y = maxUpSpeed;
        }
        Vector3 velocity = vector;
        velocity.y = y;
        rb.velocity = velocity;
    }


    private void updateLayerWeights()
    {
        driveWeight = leanWeight * (1f - speedHoriz / (attributes.TRANSITION_PIVOT_SPEED * 0.1425f));
        if (driveWeight > 0.8f)
        {
            driveWeight = 0.8f;
        }
        if (speedHoriz > speedHoriz_lastFrame)
        {
            cruiseWeight -= 0.7f * dTime;
            if (cruiseWeight < 0f)
            {
                cruiseWeight = 0f;
            }
        }
        else if (speedHoriz > 15f)
        {
            cruiseWeight += cruise * dTime;
        }
        if (speedHoriz < 10f)
        {
            cruiseWeight *= speedHoriz / 10f;
        }
        cruiseWeight *= 313f / torsoAngle;
        if (cruiseWeight > 1f)
        {
            cruiseWeight = 1f;
        }

        //    //Debug.Log("speedHor: " + speedHoriz + " leftInput: " + leftInput + " rightInput: " + rightInput);
        //Debug.Log($"(1f - driveWeight)*runWeight) --  driveWeight: {driveWeight}, runWeight: {runWeight}");

        animator.SetLayerWeight(CruiseLayer, cruiseWeight * runWeight);
        animator.SetLayerWeight(SetPositionLayer, setPositionWeight);


        animator.SetLayerWeight(TorsoHurdleLayer, hurdleWeight);
        animator.SetLayerWeight(RightLegHurdleLayer, hurdleWeight);
        animator.SetLayerWeight(LeftLegHurdleLayer, hurdleWeight);
        animator.SetLayerWeight(RightArmHurdleLayer, hurdleWeight);
        animator.SetLayerWeight(LeftArmHurdleLayer, hurdleWeight);

        if (Mathf.Approximately(hurdleWeight, 1f))
        {


            if (rb != null && Mathf.Abs(rb.velocity.y) < 0.01f) // only jump if not already moving vertically
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            }

            animator.SetLayerWeight(RightLegURLayer, 0f);
            animator.SetLayerWeight(LeftLegURLayer, 0f);
            animator.SetLayerWeight(RightLegDRLayer, 0f);
            animator.SetLayerWeight(LeftLegDRLayer, 0f);
            animator.SetLayerWeight(RightArmURLayer, 0f);
            animator.SetLayerWeight(LeftArmURLayer, 0f);
            animator.SetLayerWeight(RightArmDRLayer, 0f);
            animator.SetLayerWeight(LeftArmDRLayer, 0f);



            animator.SetLayerWeight(TosoUprightLayer, 0f); //torso drive
            animator.SetLayerWeight(TorsoDriveLayer, 1f); //torso drive

            //torsoAngle = 20;
        }
        else
        {

            animator.SetLayerWeight(TosoUprightLayer, (1f - leanWeight) * runWeight);

            animator.SetLayerWeight(TorsoDriveLayer, leanWeight * Mathf.Max(runWeight));
            animator.SetLayerWeight(RightLegURLayer, (1f - driveWeight) * runWeight);
            animator.SetLayerWeight(LeftLegURLayer, (1f - driveWeight) * runWeight);
            animator.SetLayerWeight(RightLegDRLayer, driveWeight * runWeight);
            animator.SetLayerWeight(LeftLegDRLayer, driveWeight * runWeight);
            animator.SetLayerWeight(RightArmURLayer, (1f - driveWeight) * runWeight);
            animator.SetLayerWeight(LeftArmURLayer, (1f - driveWeight) * runWeight);
            animator.SetLayerWeight(RightArmDRLayer, driveWeight * runWeight);
            animator.SetLayerWeight(LeftArmDRLayer, driveWeight * runWeight);

        }

        animator.SetFloat(AnimHashes.HorizSpeed, speedHoriz / 28f);
    }

    //private void updateLayerWeights()
    //{
    //    driveWeight = leanWeight * (1f - speedHoriz / (attributes.TRANSITION_PIVOT_SPEED * 0.1425f));
    //    if (driveWeight > 0.8f)
    //    {
    //        driveWeight = 0.8f;
    //    }
    //    if (speedHoriz > speedHoriz_lastFrame)
    //    {
    //        cruiseWeight -= 0.7f * dTime;
    //        if (cruiseWeight < 0f)
    //        {
    //            cruiseWeight = 0f;
    //        }
    //    }
    //    else if (speedHoriz > 15f)
    //    {
    //        cruiseWeight += cruise * dTime;
    //    }
    //    if (speedHoriz < 10f)
    //    {
    //        cruiseWeight *= speedHoriz / 10f;
    //    }
    //    cruiseWeight *= 313f / torsoAngle;
    //    if (cruiseWeight > 1f)
    //    {
    //        cruiseWeight = 1f;
    //    }

    //    //Debug.Log("speedHor: " + speedHoriz + " leftInput: " + leftInput + " rightInput: " + rightInput);
    //    Debug.Log($"(1f - driveWeight)*runWeight) --  driveWeight: {driveWeight}, runWeight: {runWeight}");

    //    animator.SetLayerWeight(1, cruiseWeight * runWeight);
    //    animator.SetLayerWeight(2, setPositionWeight);
    //    animator.SetLayerWeight(3, (1f - leanWeight) * runWeight);
    //    animator.SetLayerWeight(4, leanWeight * runWeight);

    //    if (leftInput || rightInput)
    //    {


    //        animator.SetLayerWeight(5, (1f - driveWeight) * runWeight);
    //        animator.SetLayerWeight(6, (1f - driveWeight) * runWeight);
    //        animator.SetLayerWeight(7, driveWeight * runWeight);
    //        animator.SetLayerWeight(8, driveWeight * runWeight);
    //        animator.SetLayerWeight(9, (1f - driveWeight) * runWeight);
    //        animator.SetLayerWeight(10, (1f - driveWeight) * runWeight);
    //        animator.SetLayerWeight(11, driveWeight * runWeight);
    //        animator.SetLayerWeight(12, driveWeight * runWeight);
    //    }
    //    else
    //    {
    //        for (int i = 5; i <= 12; i++)
    //        {
    //            animator.SetLayerWeight(i, minWeight);

    //        }
    //        //animator.SetLayerWeight(5, minWeight);
    //        //animator.SetLayerWeight(6, minWeight);
    //        //animator.SetLayerWeight(7, minWeight);
    //        //animator.SetLayerWeight(8, minWeight);
    //        //animator.SetLayerWeight(9, minWeight);
    //        //animator.SetLayerWeight(10, minWeight);
    //        //animator.SetLayerWeight(11, minWeight);
    //        //animator.SetLayerWeight(12, minWeight);
    //    }


    //    animator.SetFloat("horizSpeed", speedHoriz / 28f);

    //}



    private void DebugLayerWeights()
    {
        //int layerCount = animator.layerCount;
        //for (int i = 0; i < layerCount; i++)
        //{
        //    string layerName = animator.GetLayerName(i);
        //    float weight = animator.GetLayerWeight(i);
        //    //Debug.Log($"Layer {i} ({layerName}) = {weight}");
        //}
    }


    public IEnumerator launch(bool leadLegLaunch)
    {
        raceManager.startingBlocks_current[attributes.lane - 1].GetComponent<StartingBlockController>().addLaunchForce();
        if (isPlayer && raceManager.viewMode == RaceManager.VIEW_MODE_LIVE)
        {
            globalController.audioController.playSound(AudioController.BLOCK_EXIT, 0f);
        }
        animator.SetBool(AnimHashes.Launch, value: true);
        cruiseWeight = 0f;
        float launchPower = 27.489f * launch_power;
        if (!leadLegLaunch)
        {
            launchPower *= 0.2f;
        }
        Vector3 launchVecVert = Vector3.up * launch_power * 0.35f;
        for (int j = 0; j < 12; j++)
        {
            rb.AddForce((gyro.transform.forward + launchVecVert) * launchPower, ForceMode.Force);
            yield return new WaitForSeconds(1f / 90f);
        }
        for (int j = 0; j < 10; j++)
        {
            yield return new WaitForSeconds(1f / 90f);
        }
        animator.SetBool(AnimHashes.Launch, value: false);
    }

    public IEnumerator pop(float delay)
    {
        yield return new WaitForSeconds(delay);
        popParticles.Play();
        sweatParticles.gameObject.SetActive(value: false);
        dustParticles.gameObject.SetActive(value: false);
        hide();
    }

    public void hide()
    {
        attributes.smr_dummy.enabled = false;
        attributes.smr_top.enabled = false;
        attributes.smr_bottoms.enabled = false;
        attributes.smr_shoes.enabled = false;
        attributes.smr_socks.enabled = false;
        attributes.smr_headband.enabled = false;
        attributes.smr_sleeve.enabled = false;
    }
}
