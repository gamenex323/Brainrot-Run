using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntryButtonController : MonoBehaviour
{
	public LeaderboardManager lbm;

	public Button button;

	public Image image;

	public int raceEvent;

	public string pfid;

	public int position;

	public string userName;

	public float score;

	public string racerName;

	public string date;

	public void init(LeaderboardManager _lbm, int _raceEvent, string _pfid, int _position, string _userName, float _score, string _racerName, string _date)
	{
		lbm = _lbm;
		button = GetComponent<Button>();
		image = GetComponent<Image>();
		raceEvent = _raceEvent;
		pfid = _pfid;
		position = _position;
		userName = _userName;
		score = _score;
		racerName = _racerName;
		date = _date;
		base.transform.Find("Text_Placing").gameObject.GetComponent<TextMeshProUGUI>().text = (position + 1).ToString();
		base.transform.Find("Text_Username").gameObject.GetComponent<TextMeshProUGUI>().text = userName;
		base.transform.Find("Text_Racername").gameObject.GetComponent<TextMeshProUGUI>().text = racerName;
		string text;
		string text2;
		if (_raceEvent <= 3)
		{
			text = "F3";
			text2 = " sec";
		}
		else
		{
			text = "F0";
			text2 = " pts";
		}
		base.transform.Find("Text_Score").gameObject.GetComponent<TextMeshProUGUI>().text = score.ToString(text) + text2;
		if (_raceEvent > 3)
		{
			button.interactable = false;
		}
	}

	public void downloadRacer()
	{
		StartCoroutine(lbm.downloadRacer(raceEvent, pfid));
		image.color = new Color(0.37f, 0.37f, 1f);
	}

	public void showTooltip()
	{
		TooltipController tooltipController = lbm.tooltipController;
		string text = "Set by " + userName + " on " + date + "\nAthlete: <color=#b8daff>" + racerName + "</color>";
		text = ((raceEvent > 3) ? (text + "\n\n<color=yellow>(Not downloadable)</color>") : (text + "\n\n<color=yellow>Click to download ghost</color>"));
		tooltipController.show(-1f);
		tooltipController.setText(text);
	}

	public void hideTooltip()
	{
		lbm.tooltipController.hide();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
