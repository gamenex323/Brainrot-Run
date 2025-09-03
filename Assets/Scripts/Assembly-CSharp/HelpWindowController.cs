using UnityEngine;

public class HelpWindowController : MonoBehaviour
{
	[SerializeField]
	private GameObject HelpWindow;

	public void show()
	{
		HelpWindow.SetActive(value: true);
	}

	public void hide()
	{
		HelpWindow.SetActive(value: false);
	}
}
