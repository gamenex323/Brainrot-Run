using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
	[SerializeField]
	private Button[] eventButtons;

	[SerializeField]
	private GameObject loadingIcon;

	public int leaderboardMode;

	public static int GLOBAL = 0;

	public static int FRIENDS = 1;

	[SerializeField]
	private Text headerText;

	[SerializeField]
	private GameObject noConnectionIndicator;

	[SerializeField]
	private GameObject selectEventIndicator;

	[SerializeField]
	private GlobalController gc;

	[SerializeField]
	private GameObject grid;

	[SerializeField]
	private GameObject entryPrefab;

	[SerializeField]
	private int selectedRaceEvent;

	public int maxDownloadableGhosts;

	public int pageNum;

	public int playerRank;

	private int entriesOnPage;

	public TooltipController tooltipController;

	public Text pageNumText;

	public Text yourRankNumText;

	public Button yourRankButton;

	public void setLeaderboardMode(int mode)
	{
		leaderboardMode = mode;
		if (mode == GLOBAL)
		{
			headerText.text = "Global Leaderboard";
		}
		else
		{
			headerText.text = "Friends Leaderboard";
		}
	}

	public void toggleLeaderboardMode()
	{
		if (leaderboardMode == GLOBAL)
		{
			setLeaderboardMode(FRIENDS);
		}
		else
		{
			setLeaderboardMode(GLOBAL);
		}
	}

	public void setEmpty()
	{
		clearGrid();
		selectEventIndicator.SetActive(value: true);
	}

	public void init(int raceEvent, bool fromPageZero)
	{
		selectEventIndicator.SetActive(value: false);
		entriesOnPage = 10;
		PlayFabManager.selectedRaceEvent = raceEvent;
		yourRankButton.interactable = false;
		selectedRaceEvent = raceEvent;
		clearGrid();
		if (fromPageZero)
		{
			pageNum = 0;
		}
		pageNumText.text = (pageNum + 1).ToString();
		StartCoroutine(initLeaderboard(raceEvent, pageNum));
		StartCoroutine(getThisUserInfo());
	}

	public void init(int raceEvent)
	{
		init(raceEvent, fromPageZero: true);
	}

	public void refresh()
	{
		init(selectedRaceEvent, fromPageZero: true);
	}

	private IEnumerator initLeaderboard(int raceEvent, int _pageNum)
	{
		PlayFabManager.leaderboardLoaded = false;
		noConnectionIndicator.SetActive(value: false);
		disableEventButtons();
		if (!PlayFabManager.loggedIn)
		{
			PlayFabManager.loginError = false;
			StartCoroutine(gc.handleLogin(successMsg: true, failMsg: false));
			yield return new WaitUntil(() => PlayFabManager.loggedIn || PlayFabManager.loginError);
		}
		if (!PlayFabManager.loggedIn)
		{
			noConnectionIndicator.SetActive(value: true);
		}
		else
		{
			if (leaderboardMode == GLOBAL)
			{
				PlayFabManager.getLeaderboard(raceEvent, friendsOnly: false, _pageNum * 10, 10 + _pageNum * 10);
			}
			else
			{
				PlayFabManager.getLeaderboard(raceEvent, friendsOnly: true, _pageNum * 10, 10 + _pageNum * 10);
			}
			yield return new WaitUntil(() => PlayFabManager.leaderboardLoaded || PlayFabManager.leaderboardGetError);
			if (PlayFabManager.leaderboardGetError)
			{
				noConnectionIndicator.SetActive(value: true);
			}
			else
			{
				string[] array = PlayFabManager.leaderboardString.Split(':');
				entriesOnPage = array.Length;
				for (int i = 0; i < array.Length - 1; i++)
				{
					string[] array2 = array[i].Split('*');
					string pfid = array2[0];
					int position = int.Parse(array2[1]);
					string userName = array2[2];
					float num = ((raceEvent > 3) ? float.Parse(array2[3]) : (float.Parse(array2[3]) / -10000f));
					string racerName = array2[4];
					string date = array2[5];
					if (num != 0f)
					{
						Object.Instantiate(entryPrefab, grid.transform).GetComponent<LeaderboardEntryButtonController>().init(this, raceEvent, pfid, position, userName, num, racerName, date);
					}
				}
			}
			PlayFabManager.leaderboardLoaded = false;
		}
		enableEventButtons();
	}

	private IEnumerator getThisUserInfo()
	{
		PlayFabManager.thisUserInfoRetrieved = false;
		PlayFabManager.getThisUserLeaderboardInfo();
		yield return new WaitUntil(() => PlayFabManager.thisUserInfoRetrieved);
		yourRankNumText.text = "#" + (PlayFabManager.thisUserPosition + 1);
		yourRankButton.interactable = true;
	}

	public void getLeaderboardAroundThisUser()
	{
		int num = (PlayFabManager.thisUserPosition + 10) / 10 - 1;
		pageNum = num;
		init(PlayFabManager.selectedRaceEvent, fromPageZero: false);
	}

	public IEnumerator downloadRacer(int raceEvent, string pfid)
	{
		setEntryButtonsEnabled(enabled: false);
		PlayFabManager.userLeaderboardInfoRetrieved = false;
		PlayFabManager.getLeaderboardEntryInfo(raceEvent, pfid);
		yield return new WaitUntil(() => PlayFabManager.userLeaderboardInfoRetrieved);
		string id = PlayerAttributes.generateID(PlayFabManager.userRacerName, PlayFabManager.userDisplayName);
		GameObject gameObject = gc.loadRacer(id, raceEvent, "Ghost (Back End)", fromLeaderboard: true, forReplay: false);
		PlayerAttributes component = gameObject.GetComponent<PlayerAttributes>();
		for (int i = 0; i < component.personalBests.Length; i++)
		{
			if (i != raceEvent)
			{
				component.personalBests[i] = -1f;
			}
		}
		gc.saveRacer(gameObject, raceEvent, new string[1] { GlobalController.SAVED_RACER_MEMORY }, sendToLeaderboard: false, forReplay: false);
		Object.Destroy(gameObject);
		int num = gc.downloadedGhosts + 1;
		PlayerPrefs.SetInt("Downloaded Ghosts", num);
		gc.downloadedGhosts = num;
		tooltipController.setText("Downloaded!");
		tooltipController.show(3f);
		setEntryButtonsEnabled(enabled: true);
	}

	public void nextPage()
	{
		if (entriesOnPage >= 10)
		{
			pageNum++;
			init(selectedRaceEvent, fromPageZero: false);
		}
	}

	public void prevPage()
	{
		if (pageNum > 0)
		{
			pageNum--;
			init(selectedRaceEvent, fromPageZero: false);
		}
	}

	private void clearGrid()
	{
		foreach (Transform item in grid.transform)
		{
			Object.Destroy(item.gameObject);
		}
	}

	private void setEntryButtonsEnabled(bool enabled)
	{
		foreach (Transform item in grid.transform)
		{
			item.gameObject.GetComponent<Button>().interactable = enabled;
		}
	}

	private void disableEventButtons()
	{
		loadingIcon.SetActive(value: true);
		Button[] array = eventButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].interactable = false;
		}
	}

	private void enableEventButtons()
	{
		loadingIcon.SetActive(value: false);
		Button[] array = eventButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].interactable = true;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
