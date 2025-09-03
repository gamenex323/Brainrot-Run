using System;
using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
	public AudioSource[] audioSources;

	public static int GUNSHOT = 0;

	public static int BLOCK_RATTLE = 1;

	public static int BLOCK_EXIT = 2;

	public static int CHEERING = 3;

	public static int VOICE_READY = 4;

	public static int VOICE_SET = 5;

	public float soundVolume;

	public void playSound(int soundIndex, float delay)
	{
		audioSources[soundIndex].PlayDelayed(Convert.ToUInt64(delay));
	}

	public void setVolume(float vol)
	{
		soundVolume = vol * 5f;
		AudioSource[] array = audioSources;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].volume = soundVolume;
		}
		PlayerPrefs.SetFloat("Audio Volume", vol);
	}

	public void easeSoundVolume(int soundIndex, float vol)
	{
		StartCoroutine(easeVolume(soundIndex, vol));
	}

	private IEnumerator easeVolume(int soundIndex, float vol)
	{
		AudioSource audio = audioSources[soundIndex];
		while (Mathf.Abs(audioSources[soundIndex].volume - vol) > 0.1f)
		{
			float volume = audioSources[soundIndex].volume;
			audio.volume = Mathf.Lerp(volume, vol, 60f * Time.deltaTime);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		audioSources[soundIndex].volume = vol;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
