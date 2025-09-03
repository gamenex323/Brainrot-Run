using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour
{
	public GlobalController gc;

	[SerializeField]
	private Image backgroundImage;

	[SerializeField]
	private RectTransform backgroundImageT;

	[SerializeField]
	private TextMeshProUGUI text;

	[SerializeField]
	private RectTransform rectTransform;

	[SerializeField]
	private RectTransform canvasRectTransform;

	[SerializeField]
	private Vector2 padding;

	public void showUnlockReq(int raceEvent)
	{
		if (raceEvent == RaceManager.RACE_EVENT_100M)
		{
			if (gc.unlockManager.rank_100m > UnlockManager.NONE)
			{
				show(-1f);
				setText(gc.unlockManager.getCurrentRankString(raceEvent));
			}
		}
		else if (raceEvent == RaceManager.RACE_EVENT_200M)
		{
			if (!gc.unlockManager.unlocked_200m)
			{
				show(-1f);
				setText(formatListToUnlockText(gc.unlockManager.unlockReqList_200m, canSatisfyAny: false));
			}
			else if (gc.unlockManager.rank_200m > UnlockManager.NONE)
			{
				show(-1f);
				setText(gc.unlockManager.getCurrentRankString(raceEvent));
			}
		}
		else if (raceEvent == RaceManager.RACE_EVENT_400M)
		{
			if (!gc.unlockManager.unlocked_400m)
			{
				show(-1f);
				setText(formatListToUnlockText(gc.unlockManager.unlockReqList_400m, canSatisfyAny: false));
			}
			else if (gc.unlockManager.rank_400m > UnlockManager.NONE)
			{
				show(-1f);
				setText(gc.unlockManager.getCurrentRankString(raceEvent));
			}
		}
		else if (raceEvent == RaceManager.RACE_EVENT_60M)
		{
			if (!gc.unlockManager.unlocked_60m)
			{
				show(-1f);
				setText(formatListToUnlockText(gc.unlockManager.unlockReqList_60m, canSatisfyAny: false));
			}
			else if (gc.unlockManager.rank_100m > UnlockManager.NONE)
			{
				show(-1f);
				setText(gc.unlockManager.getCurrentRankString(raceEvent));
			}
		}
	}

	public void showUnlockReq(string thingToUnlock)
	{
		if (thingToUnlock == "Character Slot")
		{
			show(-1f);
			string text = "Slots: " + gc.unlockManager.characterSlots;
			text = text + "\n\n" + formatListToUnlockText(gc.unlockManager.unlockReqList_characterSlot, canSatisfyAny: true);
			setText(text);
		}
	}

	private string formatListToUnlockText(List<string> list, bool canSatisfyAny)
	{
		string text = ((!canSatisfyAny) ? "Unlock by getting:" : "Unlock by getting:");
		for (int i = 0; i < list.Count; i++)
		{
			text = text + "\n - " + list[i];
		}
		return text;
	}

	public void setText(string str)
	{
		text.SetText(str);
		text.ForceMeshUpdate();
		Vector2 renderedValues = text.GetRenderedValues(onlyVisibleCharacters: false);
		backgroundImageT.sizeDelta = renderedValues + padding;
	}

	public void show(float time)
	{
		text.enabled = true;
		backgroundImage.enabled = true;
		if (time > -1f)
		{
			StartCoroutine(hideAfterDelay(time));
		}
	}

	public void hide()
	{
		text.enabled = false;
		backgroundImage.enabled = false;
	}

	private IEnumerator hideAfterDelay(float time)
	{
		yield return new WaitForSeconds(time);
		hide();
	}

	private void Start()
	{
		setText("Example Text");
		hide();
	}

	private void Update()
	{
		rectTransform.anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;
	}
}
