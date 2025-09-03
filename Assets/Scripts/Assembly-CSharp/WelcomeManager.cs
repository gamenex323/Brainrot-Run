using UnityEngine;

public class WelcomeManager : MonoBehaviour
{
	public GameObject WelcomeWindow;

	public void show()
	{
		WelcomeWindow.SetActive(value: true);
	}

	public void hide()
	{
		WelcomeWindow.SetActive(value: false);
	}
}
