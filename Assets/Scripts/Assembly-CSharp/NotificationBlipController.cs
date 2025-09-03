using UnityEngine;
using UnityEngine.UI;

public class NotificationBlipController : MonoBehaviour
{
	public GameObject blip;

	public Text text;

	public void show()
	{
		blip.SetActive(value: true);
	}

	public void hide()
	{
		blip.SetActive(value: false);
	}

	public void setText(string s)
	{
		text.text = s;
	}
}
