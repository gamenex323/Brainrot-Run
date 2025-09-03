using UnityEngine;
using UnityEngine.UI;

public class SelectionButtonScript : MonoBehaviour
{
	public SelectionListScript list;

	public bool selected;

	public string id;

	public string racerName;

	public string displayName;

	public string time;

	public static Color32 playerButtonColor = new Color32(250, 196, 192, byte.MaxValue);

	public static Color32 ghostButtonColor = new Color32(207, byte.MaxValue, 236, byte.MaxValue);

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void setFromRacer(string _id, int raceEvent)
	{
		id = _id;
		racerName = PlayerPrefs.GetString(_id).Split(':')[1];
		displayName = _id.Split('_')[1];
		base.gameObject.transform.Find("NameText").GetComponent<Text>().text = racerName;
		time = PlayerPrefs.GetString(_id).Split(':')[3 + raceEvent];
		if (float.Parse(time) == -1f)
		{
			base.gameObject.transform.Find("TimeText").GetComponent<Text>().text = "--";
		}
		else
		{
			base.gameObject.transform.Find("TimeText").GetComponent<Text>().text = float.Parse(time).ToString("F3");
		}
	}

	public void removeThisButtonAndForgetAssociatedRacer()
	{
		if (selected)
		{
			list.numSelected--;
			list.selectedButtonIDs.Remove(id);
		}
		list.gc.forgetRacer(id, new string[1] { list.sourceMemory }, forReplay: false);
		list.removeButton(id);
	}

	public void toggle()
	{
		if (!selected)
		{
			if (list.numSelected < list.maxSelectable)
			{
				if (!list.getButton(id).GetComponent<SelectionButtonScript>().selected)
				{
					setColor("selected");
					list.numSelected++;
					list.selectedButtonIDs.Add(id);
					if (list == list.gc.playerSelectButtonList)
					{
						list.setPreviewRacer(id);
					}
					selected = true;
				}
			}
			else
			{
				if (list.replaceLastSelection && list.selectedButtonIDs != null && list.selectedButtonIDs.Count != 0)
				{
					list.minSelectable--;
					list.getButton(list.selectedButtonIDs[list.selectedButtonIDs.Count - 1]).GetComponent<SelectionButtonScript>().toggle(setting: false);
					list.minSelectable++;
				}
				setColor("selected");
				list.numSelected++;
				list.selectedButtonIDs.Add(id);
				if (list == list.gc.playerSelectButtonList)
				{
					list.setPreviewRacer(id);
				}
				selected = true;
			}
			if (list.pa != null)
			{
				list.pa.resetPos();
			}
		}
		else if (list.numSelected > list.minSelectable)
		{
			setColor("unselected");
			list.numSelected--;
			list.selectedButtonIDs.Remove(id);
			selected = false;
		}
		if (list == list.gc.ghostSelectButtonList)
		{
			list.gc.setupManager.botCount_max = 7 - list.numSelected;
			if (list.gc.setupManager.botCount > list.gc.setupManager.botCount_max)
			{
				list.gc.setupManager.incrementBotCount(list.gc.setupManager.botCount_max - list.gc.setupManager.botCount);
			}
		}
		if (list.numSelected == 0)
		{
			if (list.pa != null)
			{
				list.pa.setVisibility(visible: false);
			}
		}
		else if (list.pa != null)
		{
			list.pa.setVisibility(visible: true);
			list.pa.land();
		}
	}

	public void toggle(bool setting)
	{
		if (selected != setting)
		{
			toggle();
		}
	}

	public void setColor(string state)
	{
		Image component = GetComponent<Image>();
		Color.RGBToHSV(component.color, out var H, out var S, out var V);
		component.color = Color.HSVToRGB(S: (!(state == "selected")) ? (S / 3f) : (S * 3f), H: H, V: V);
	}

	public void showTooltip()
	{
		if (list.ttc != null)
		{
			TooltipController ttc = list.ttc;
			ttc.show(3f);
			ttc.setText("Set by " + displayName);
		}
	}

	public void hideTooltip()
	{
		if (list.ttc != null)
		{
			list.ttc.hide();
		}
	}

	public void showDeleteDialog()
	{
		if (list.ddc == null)
		{
			removeThisButtonAndForgetAssociatedRacer();
		}
		else if (list.buttonIDs.Count > 1)
		{
			list.ddc.list1.getButton(id).GetComponent<SelectionButtonScript>();
			list.ddc.list1.buttonToDelete = list.ddc.list1.getButton(id).GetComponent<SelectionButtonScript>();
			if (list.ddc.list2.getButton(id) != null)
			{
				list.ddc.list2.getButton(id).GetComponent<SelectionButtonScript>();
				list.ddc.list2.buttonToDelete = list.ddc.list2.getButton(id).GetComponent<SelectionButtonScript>();
			}
			list.ddc.show();
		}
	}

	public void init(SelectionListScript s)
	{
		list = s;
		base.transform.SetParent(list.grid.transform, worldPositionStays: false);
	}
}
