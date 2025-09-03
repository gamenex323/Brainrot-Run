using UnityEngine;
using UnityEngine.UI;

public class SetupManager : MonoBehaviour
{
	public int selectedRaceEvent;

	public GlobalController gc;

	public CameraController cameraController;

	public Canvas canvas;

	public GameObject selectionLists;

	public GameObject playerSelectButtonList;

	public GameObject ghostSelectButtonList;

	public GameObject pointerPos_100m;

	public GameObject pointerPos_200m;

	public GameObject pointerPos_400m;

	public GameObject pointerPos_60m;

	public GameObject pointer;

	public Vector2 onScreenLocation;

	public Vector2 offScreenLocation;

	private bool movingElement;

	public float swapSpeed;

	public Text botCountText;

	public int botCount;

	public int botCount_max;

	private void Start()
	{
	}

	public void setSelectedRaceEvent(int raceEvent, bool setCamera)
	{
		selectedRaceEvent = raceEvent;
		gc.selectedRaceEvent = raceEvent;
		toggleSelectionLists(selectedRaceEvent);
		movePointer(raceEvent);
		if (setCamera)
		{
			gc.cameraController.setCameraFocusOnStart();
		}
	}

	public void setSelectedRaceEvent(int raceEvent)
	{
		selectedRaceEvent = raceEvent;
		gc.selectedRaceEvent = raceEvent;
		toggleSelectionLists(selectedRaceEvent);
		movePointer(raceEvent);
		gc.cameraController.setCameraFocusOnStart();
	}

	private void toggleSelectionLists(int raceEvent)
	{
		SelectionListScript component = playerSelectButtonList.GetComponent<SelectionListScript>();
		SelectionListScript component2 = ghostSelectButtonList.GetComponent<SelectionListScript>();
		component.setForEvent(raceEvent);
		component2.init(raceEvent, GlobalController.SAVED_RACER_MEMORY, 7, 0, _replaceLastSelection: false, _showIfNoTime: false);
	}

	public void incrementBotCount(int n)
	{
		if (botCount + n >= 0 && botCount + n <= botCount_max)
		{
			botCount += n;
			botCountText.text = botCount.ToString();
		}
	}

	private void movePointer(int raceEvent)
	{
		Transform transform;
		if (raceEvent == RaceManager.RACE_EVENT_100M)
		{
			transform = pointerPos_100m.transform;
		}
		else if (raceEvent == RaceManager.RACE_EVENT_200M)
		{
			transform = pointerPos_200m.transform;
		}
		else if (raceEvent == RaceManager.RACE_EVENT_400M)
		{
			transform = pointerPos_400m.transform;
		}
		else
		{
			if (raceEvent != RaceManager.RACE_EVENT_60M)
			{
				return;
			}
			transform = pointerPos_60m.transform;
		}
		pointer.transform.position = transform.position;
	}

	public void init(bool setCamera)
	{
		incrementBotCount(7);
		setSelectedRaceEvent(RaceManager.RACE_EVENT_100M, setCamera: false);
	}
}
