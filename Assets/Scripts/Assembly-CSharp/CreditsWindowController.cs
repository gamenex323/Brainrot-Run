using UnityEngine;

public class CreditsWindowController : MonoBehaviour
{
	public GameObject CreditsWindow;

	public void show()
	{
		CreditsWindow.SetActive(value: true);
	}

	public void hide()
	{
		CreditsWindow.SetActive(value: false);
	}
}
