using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
	public static KeyCode controlsLeft;

	public static KeyCode controlsRight;

	public GlobalController gc;

	public GameObject SettingsWindow;

	[SerializeField]
	private GameObject controlsButtonLeft;

	[SerializeField]
	private GameObject controlsButtonRight;

	[SerializeField]
	private GameObject controlsButtonLeft2;

	[SerializeField]
	private GameObject controlsButtonRight2;

	[SerializeField]
	private TMP_Dropdown displayDropdown;

	[SerializeField]
	private Slider cameraSlider;

	[SerializeField]
	private float cameraDistance;

	public void init(KeyCode l, KeyCode r, float camD, string displayMode)
	{
		controlsLeft = l;
		controlsRight = r;
		controlsButtonLeft.transform.Find("Text").GetComponent<Text>().text = l.ToString();
		controlsButtonRight.transform.Find("Text").GetComponent<Text>().text = r.ToString();
		controlsButtonLeft2.transform.Find("Text").GetComponent<Text>().text = l.ToString();
		controlsButtonRight2.transform.Find("Text").GetComponent<Text>().text = r.ToString();
		displayDropdown.value = displayDropdown.options.FindIndex((TMP_Dropdown.OptionData option) => option.text == displayMode);
		cameraSlider.value = (cameraDistance = camD);
	}

	public void show()
	{
		SettingsWindow.SetActive(value: true);
	}

	public void hide()
	{
		SettingsWindow.SetActive(value: false);
	}

	public void setControls(int side, KeyCode k)
	{
		if (side == 0)
		{
			controlsLeft = k;
		}
		else
		{
			controlsRight = k;
		}
	}

	public void getControlsInput(int side)
	{
		StartCoroutine(readInput(side));
	}

	private IEnumerator readInput(int side)
	{
		Text text;
		Text text2;
		Button button;
		Button component;
		KeyCode origKey;
		if (side == 0)
		{
			text = controlsButtonLeft.transform.Find("Text").GetComponent<Text>();
			text2 = controlsButtonLeft2.transform.Find("Text").GetComponent<Text>();
			button = controlsButtonLeft.GetComponent<Button>();
			component = controlsButtonLeft2.GetComponent<Button>();
			origKey = controlsLeft;
		}
		else
		{
			text = controlsButtonRight.transform.Find("Text").GetComponent<Text>();
			text2 = controlsButtonRight.transform.Find("Text").GetComponent<Text>();
			button = controlsButtonRight.GetComponent<Button>();
			component = controlsButtonRight.GetComponent<Button>();
			origKey = controlsRight;
		}
		text.text = "[PRESS KEY]";
		text2.text = "[PRESS KEY]";
		button.interactable = false;
		component.interactable = false;
		KeyCode i = KeyCode.None;
		float t = 0f;
		while (t < 5f)
		{
			foreach (KeyCode value in Enum.GetValues(typeof(KeyCode)))
			{
				if (Input.GetKey(value))
				{
					i = value;
					t = 5f;
					break;
				}
			}
			t += Time.deltaTime;
			yield return null;
		}
		if (i != 0)
		{
			text.text = i.ToString();
			text2.text = i.ToString();
			setControls(side, i);
		}
		else
		{
			text.text = origKey.ToString();
			text2.text = origKey.ToString();
		}
		button.interactable = true;
	}

	public void setDisplayMode()
	{
		string text = displayDropdown.options[displayDropdown.value].text;
		gc.cameraController.initViewSettings(text);
		PlayerPrefs.SetString("Display Mode", text);
	}

	public void adjustCameraDistance()
	{
		cameraDistance = Mathf.Pow(cameraSlider.value, 1f);
		gc.cameraController.setCameraDistance(cameraDistance);
	}

	public void saveSettings()
	{
		PlayerPrefs.SetInt("Controls Left", (int)controlsLeft);
		PlayerPrefs.SetInt("Controls Right", (int)controlsRight);
		PlayerPrefs.SetFloat("Camera Distance", cameraDistance);
	}
}
