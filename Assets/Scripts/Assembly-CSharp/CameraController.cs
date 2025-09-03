using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public int mode;

	public static int STATIONARY = 0;

	public static int STATIONARY_SLOW = 1;

	public static int SIDE_RIGHT = 2;

	public static int SIDE_LEFT = 3;

	public static int ISOMETRIC = 4;

	public static int TV = 5;

	public static int FIRSTPERSON = 6;

	public static int THIRDPERSON = 7;

	public static int TOPDOWN = 8;

	[SerializeField]
	private List<int> cameraModes_live;

	[SerializeField]
	private List<int> cameraModes_replay;

	public static float posX_base = 24f;

	public bool cameraShake;

	public int fov;

	public GlobalController gc;

	public Camera camera;

	public Animator animator;

	public GameObject referenceObject;

	public PlayerAnimationV2 referenceAnim;

	public bool referenceHasAnim;

	public float referenceSpeed;

	public float cameraDistance;

	public int cameraGameplayMode;

	public int cameraReplayMode;

	public GameObject trackReferenceObject_start_60m;

	public GameObject trackReferenceObject_start_100m;

	public GameObject trackReferenceObject_start_200m;

	public GameObject trackReferenceObject_start_400m;

	public Transform t;

	private Vector3 currentPos;

	private Vector3 referencePos;

	private Vector3 targetPos;

	private Quaternion currentRot;

	private Quaternion referenceRot;

	private Quaternion targetRot;

	private Vector3 posOffset;

	private Vector3 rotTargetOffset;

	private float posSpeed;

	private float rotSpeed;

	private float dTime;

	public void init(float _cameraDistance, int _cameraGameplayMode, int _cameraReplayMode)
	{
		setCameraDistance(_cameraDistance);
		cameraGameplayMode = _cameraGameplayMode;
		cameraReplayMode = _cameraReplayMode;
		camera.ResetProjectionMatrix();
		Matrix4x4 projectionMatrix = camera.projectionMatrix;
		float num = 1.5f;
		projectionMatrix.m00 *= num;
		camera.projectionMatrix = projectionMatrix;
		initViewSettings(PlayerPrefs.GetString("Display Mode"));
	}

	public void initViewSettings(string dm)
	{
		Application.targetFrameRate = -1;
		QualitySettings.vSyncCount = 0;
		if (dm == "Windowed")
		{
			FullScreenMode fullscreenMode = FullScreenMode.Windowed;
			Screen.SetResolution(1280, 720, fullscreenMode, 60);
		}
		else
		{
			FullScreenMode fullscreenMode = FullScreenMode.ExclusiveFullScreen;
			Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, fullscreenMode, 60);
		}
	}

	public void setCameraDistance(float d)
	{
		cameraDistance = d;
		cameraDistance = Mathf.Clamp(cameraDistance, 0.1f, 1f);
		setCameraFocus(referenceObject, mode);
	}

	private void Update()
	{
		dTime = Time.deltaTime;
		currentPos = t.position;
		referencePos = referenceObject.transform.position;
		referenceRot = referenceObject.transform.rotation;
		currentRot = t.rotation;
		if (referenceHasAnim)
		{
			referenceSpeed = Mathf.Pow(referenceAnim.speedHoriz, 0.4f);
			if (referenceSpeed < 1f)
			{
				referenceSpeed = 1f;
			}
		}
		else
		{
			referenceSpeed = 1f;
		}
		if (mode == STATIONARY)
		{
			targetPos = referencePos + referenceObject.transform.TransformDirection(posOffset);
			targetRot = referenceRot;
		}
		else if (mode == STATIONARY_SLOW)
		{
			targetPos = referencePos + referenceObject.transform.TransformDirection(posOffset);
			targetRot = referenceRot;
		}
		else if (mode == SIDE_RIGHT)
		{
			targetPos = referencePos + referenceObject.transform.TransformDirection(posOffset) + referenceObject.transform.right * referenceSpeed;
			targetRot = Quaternion.LookRotation(referenceObject.transform.right * -1f, Vector3.up);
		}
		else if (mode == SIDE_LEFT)
		{
			referencePos.y = 1f;
			targetPos = referencePos + referenceObject.transform.TransformDirection(posOffset) + referenceObject.transform.right * -1f;
			targetRot = Quaternion.LookRotation(referencePos - t.position);
		}
		else if (mode == ISOMETRIC)
		{
			referencePos.y = 0f;
			targetPos = referencePos + posOffset;
			targetRot = Quaternion.LookRotation(referencePos - t.position);
		}
		else if (mode == TV)
		{
			referencePos.y = 1f;
			targetPos = referencePos + posOffset;
			targetRot = Quaternion.LookRotation(referencePos - t.position);
		}
		else if (mode == FIRSTPERSON)
		{
			cameraShake = false;
			targetPos = referenceAnim.headT.position;
			targetRot = referenceAnim.headT.rotation;
		}
		else if (mode == THIRDPERSON)
		{
			cameraShake = false;
			referencePos.y = 0f;
			targetPos = referencePos + referenceObject.transform.TransformDirection(posOffset);
			targetRot = Quaternion.LookRotation(referencePos - t.position);
		}
		else if (mode == TOPDOWN)
		{
			cameraShake = false;
			referencePos.y = 0f;
			targetPos = referencePos + posOffset + referenceObject.transform.forward * -0.5f;
			targetRot = Quaternion.LookRotation(referencePos - t.position);
		}
		t.position = Vector3.Lerp(currentPos, targetPos, posSpeed * dTime);
		t.rotation = Quaternion.Slerp(currentRot, targetRot, rotSpeed * dTime);
	}

	public void setCameraFocusOnStart()
	{
		int selectedRaceEvent = gc.setupManager.selectedRaceEvent;
		if (selectedRaceEvent == RaceManager.RACE_EVENT_60M)
		{
			setCameraFocus("60m Start", STATIONARY);
		}
		else if (selectedRaceEvent == RaceManager.RACE_EVENT_100M)
		{
			setCameraFocus("100m Start", STATIONARY);
		}
		if (selectedRaceEvent == RaceManager.RACE_EVENT_200M)
		{
			setCameraFocus("200m Start", STATIONARY);
		}
		if (selectedRaceEvent == RaceManager.RACE_EVENT_400M)
		{
			setCameraFocus("400m Start", STATIONARY);
		}
	}

	public void setCameraFocus(GameObject g, int cameraMode)
	{
		referenceObject = g;
		mode = cameraMode;
		if (mode == STATIONARY)
		{
			cameraShake = false;
			fov = 100;
			posOffset = new Vector3(0f, 0.5f, -5f);
			posSpeed = 7f;
			rotSpeed = 0.2f;
		}
		else if (mode == STATIONARY_SLOW)
		{
			cameraShake = false;
			fov = 100;
			posOffset = new Vector3(0f, 0.5f, -5f);
			posSpeed = 0.05f;
			rotSpeed = 0.05f;
		}
		else if (mode == SIDE_RIGHT)
		{
			cameraShake = false;
			fov = 100;
			posOffset = new Vector3(3.1f * Mathf.Pow(cameraDistance, 1f), 1.34f * Mathf.Pow(cameraDistance, 0.1f), 2.5f * Mathf.Pow(cameraDistance, 1f));
			posSpeed = 8f * Mathf.Pow(2f - cameraDistance, 1f);
			rotSpeed = 10f;
		}
		else if (mode == SIDE_LEFT)
		{
			cameraShake = true;
			fov = 100;
			posOffset = new Vector3(-3f, 0f, 7f);
			posSpeed = 4f;
			rotSpeed = 7f;
		}
		else if (mode == ISOMETRIC)
		{
			cameraShake = false;
			fov = 100;
			posOffset = new Vector3(3f, 2f, 1f);
			posSpeed = 1000f;
			rotSpeed = 10f;
		}
		if (mode == TV)
		{
			cameraShake = true;
			fov = 100;
			posOffset = new Vector3(5f, 2f, 3f);
			posSpeed = 10f;
			rotSpeed = 10f;
		}
		if (mode == FIRSTPERSON)
		{
			cameraShake = false;
			fov = 100;
			posOffset = new Vector3(0f, 0f, 0.1f);
			posSpeed = 100f;
			rotSpeed = 1.5f;
		}
		if (mode == THIRDPERSON)
		{
			cameraShake = true;
			fov = 100;
			posOffset = new Vector3(1f, 3f, -1.5f);
			posSpeed = 5f;
			rotSpeed = 2.5f;
		}
		if (mode == TOPDOWN)
		{
			cameraShake = true;
			fov = 100;
			posOffset = new Vector3(0f, 10f, 0f);
			posSpeed = 100f;
			rotSpeed = 5f;
		}
		animator.SetBool(AnimHashes.shake, cameraShake);
		animator.SetInteger(AnimHashes.fov, fov);
		PlayerAnimationV2 component = g.GetComponent<PlayerAnimationV2>();
		if (component != null)
		{
			referenceHasAnim = true;
			referenceAnim = component;
		}
		else
		{
			referenceHasAnim = false;
			referenceAnim = null;
		}
	}

	public void setCameraFocus(string s, int cameraMode)
	{
		switch (s)
		{
		case "60m Start":
			setCameraFocus(trackReferenceObject_start_60m, cameraMode);
			break;
		case "100m Start":
			setCameraFocus(trackReferenceObject_start_100m, cameraMode);
			break;
		case "200m Start":
			setCameraFocus(trackReferenceObject_start_200m, cameraMode);
			break;
		case "400m Start":
			setCameraFocus(trackReferenceObject_start_400m, cameraMode);
			break;
		}
	}

	public void cycleCameraMode(int viewMode)
	{
		List<int> list = ((viewMode != RaceManager.VIEW_MODE_LIVE) ? cameraModes_replay : cameraModes_live);
		int num = list.IndexOf(mode) + 1;
		if (num >= list.Count)
		{
			num = 0;
		}
		else if (num < 0)
		{
			num = list.Count - 1;
		}
		int cameraMode = list[num];
		setCameraFocus(referenceObject, cameraMode);
		if (viewMode == RaceManager.VIEW_MODE_LIVE)
		{
			cameraGameplayMode = cameraMode;
		}
		else
		{
			cameraReplayMode = cameraMode;
		}
	}
}
