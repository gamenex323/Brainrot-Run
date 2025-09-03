using TMPro;
using UnityEngine;

public class SplitsController : MonoBehaviour
{
	public RaceManager rm;

	[SerializeField]
	private GameObject splitLines_100m;

	[SerializeField]
	private GameObject splitLines_200m;

	[SerializeField]
	private GameObject splitLines_400m;

	[SerializeField]
	private GameObject splitUI;

	[SerializeField]
	private TextMeshProUGUI splitText;

	private string splitName;

	private bool first;

	private float leaderSplit;

	private float playerSplit;

	private float differential;

	public void init(int raceEvent, GameObject player)
	{
		first = false;
		splitUI.SetActive(value: false);
		if (raceEvent == RaceManager.RACE_EVENT_100M)
		{
			splitLines_100m.SetActive(value: true);
			splitLines_200m.SetActive(value: false);
			splitLines_400m.SetActive(value: false);
			splitName = "60m";
		}
		else if (raceEvent == RaceManager.RACE_EVENT_200M)
		{
			splitLines_100m.SetActive(value: false);
			splitLines_200m.SetActive(value: true);
			splitLines_400m.SetActive(value: false);
			splitName = "100m";
		}
		else if (raceEvent == RaceManager.RACE_EVENT_400M)
		{
			splitLines_100m.SetActive(value: false);
			splitLines_200m.SetActive(value: false);
			splitLines_400m.SetActive(value: true);
			splitName = "200m";
		}
	}

	public void register(GameObject racer)
	{
		float num = (float)rm.raceTick / 100f;
		if (!first)
		{
			first = true;
			leaderSplit = num;
		}
		if (racer == rm.player)
		{
			playerSplit = num;
			differential = playerSplit - leaderSplit;
			showSplit();
		}
	}

	private void showSplit()
	{
		splitUI.SetActive(value: true);
		if (differential > 0f)
		{
			splitText.text = splitName + ": " + playerSplit.ToString("F2") + "s (<color=#ffa8a8>+" + differential.ToString("F2") + "</color>)";
		}
		else
		{
			splitText.text = splitName + ": " + playerSplit.ToString("F2") + "s";
		}
	}
}
