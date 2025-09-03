using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionListScript : MonoBehaviour
{
	public GlobalController gc;

	public GameObject selectionButtonPrefab;

	public GameObject grid;

	public List<string> buttonIDs;

	public List<string> selectedButtonIDs;

	public int maxSelectable;

	public int minSelectable;

	public int numSelected;

	public bool replaceLastSelection;

	public string sourceMemory;

	public GameObject previewRacer;

	public PreviewRacerAnimation pa;

	public GameObject deleteDialog;

	public DeleteDialogController ddc;

	public TooltipController ttc;

	public SelectionButtonScript buttonToDelete;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public GameObject getButton(string id)
	{
		foreach (Transform item in grid.transform)
		{
			GameObject gameObject = item.gameObject;
			if (gameObject.GetComponent<SelectionButtonScript>().id == id)
			{
				return gameObject;
			}
		}
		return null;
	}

	public GameObject getFirst()
	{
		return grid.transform.GetChild(0).gameObject;
	}

	public GameObject addButton(string id, int raceEvent, Color32 color32)
	{
		GameObject gameObject = Object.Instantiate(selectionButtonPrefab);
		gameObject.GetComponent<Image>().color = color32;
		SelectionButtonScript component = gameObject.GetComponent<SelectionButtonScript>();
		component.init(this);
		component.setFromRacer(id, raceEvent);
		for (int i = 0; i < buttonIDs.Count; i++)
		{
			if (float.Parse(component.time) < float.Parse(getButton(buttonIDs[i]).GetComponent<SelectionButtonScript>().time))
			{
				gameObject.transform.SetSiblingIndex(i);
				break;
			}
		}
		buttonIDs.Add(id);
		return gameObject;
	}

	public void removeButton(string id)
	{
		foreach (Transform item in grid.transform)
		{
			GameObject gameObject = item.gameObject;
			if (gameObject.GetComponent<SelectionButtonScript>().id == id)
			{
				Object.Destroy(gameObject);
				buttonIDs.Remove(id);
				break;
			}
		}
	}

	public void setForEvent(int raceEvent)
	{
		foreach (Transform item in grid.transform)
		{
			SelectionButtonScript component = item.gameObject.GetComponent<SelectionButtonScript>();
			component.setFromRacer(component.id, raceEvent);
		}
	}

	public void toggleAllOff()
	{
		foreach (Transform item in grid.transform)
		{
			item.gameObject.GetComponent<SelectionButtonScript>().toggle(setting: false);
		}
		numSelected = 0;
	}

	public void clear()
	{
		foreach (Transform item in grid.transform)
		{
			if (item.gameObject != gc.newRacerButton)
			{
				Object.Destroy(item.gameObject);
			}
		}
		buttonIDs.Clear();
		selectedButtonIDs.Clear();
	}

	public void setPreviewRacer(string id)
	{
		GameObject gameObject = gc.loadRacer(id, 0, "Untagged", fromLeaderboard: false, forReplay: false);
		PlayerAttributes component = previewRacer.GetComponent<PlayerAttributes>();
		component.copyAttributesFromOther(gameObject, "body proportions");
		component.copyAttributesFromOther(gameObject, "clothing");
		component.copyAttributesFromOther(gameObject, "stats");
		component.setClothing(PlayerAttributes.FROM_THIS);
		component.setBodyProportions(PlayerAttributes.FROM_THIS);
		component.setStats(PlayerAttributes.FROM_THIS);
		Object.Destroy(gameObject);
	}

	public void init(int _raceEvent, string _sourceMemory, int _maxSelectable, int _minSelectable, bool _replaceLastSelection, bool _showIfNoTime)
	{
		clear();
		buttonIDs = new List<string>();
		selectedButtonIDs = new List<string>();
		maxSelectable = _maxSelectable;
		minSelectable = _minSelectable;
		numSelected = 0;
		replaceLastSelection = _replaceLastSelection;
		sourceMemory = _sourceMemory;
		string[] array = PlayerPrefs.GetString(_sourceMemory).Split(':');
		if (array.Length != 0)
		{
			Color32 color = new Color32(0, 0, 0, 0);
			color = ((!_replaceLastSelection) ? SelectionButtonScript.ghostButtonColor : SelectionButtonScript.playerButtonColor);
			string text = "";
			for (int i = 0; i < array.Length; i++)
			{
				text = array[i];
				if (text != "")
				{
					if (_showIfNoTime || PlayerPrefs.GetString(text).Split(':')[3 + _raceEvent] != "-1")
					{
						addButton(text, _raceEvent, color);
					}
					if (pa != null)
					{
						pa.setVisibility(visible: false);
					}
				}
			}
		}
		gc.setupManager.botCount_max = 7;
	}

	public void deleteButtonToDelete()
	{
		if (buttonToDelete != null)
		{
			buttonToDelete.removeThisButtonAndForgetAssociatedRacer();
		}
		if (ddc != null)
		{
			ddc.hide();
		}
	}
}
