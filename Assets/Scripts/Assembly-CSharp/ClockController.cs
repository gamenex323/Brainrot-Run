using TMPro;
using UnityEngine;

public class ClockController : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro timeText;

	[SerializeField]
	private TextMeshPro nameText;

	public void SetClockText(float time, string name)
	{
		timeText.text = time.ToString("F2");
		nameText.text = name;
	}
}
