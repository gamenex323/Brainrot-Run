using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
	public GlobalController gc;

	public Button audioButton;

	public Sprite audio_on;

	public Sprite audio_off;

	public Slider audioSlider;

	public AudioClip[] playlist;

	public List<AudioClip> tracks;

	private int trackIndex;

	public bool sound;

	public float volume;

	public AudioSource audio;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void play()
	{
		StartCoroutine("playMusic");
	}

	private IEnumerator playMusic()
	{
		while (true)
		{
			if (!audio.isPlaying)
			{
				nextTrack();
			}
			yield return new WaitForSeconds(1f);
		}
	}

	public void nextTrack()
	{
		if (trackIndex == playlist.Length - 1)
		{
			trackIndex = -1;
		}
		AudioClip clip = playlist[++trackIndex];
		audio.clip = clip;
		audio.Play();
	}

	public void toggleMusic()
	{
		if (sound)
		{
			mute();
		}
		else
		{
			unmute();
		}
	}

	public void setVolume(float v)
	{
		volume = v;
		audio.volume = v;
		gc.audioController.setVolume(v);
	}

	public void setVolumeFromSlider()
	{
		setVolume(audioSlider.value);
	}

	public void mute()
	{
		setVolume(0f);
		sound = false;
		audioButton.image.sprite = audio_off;
	}

	public void unmute()
	{
		setVolumeFromSlider();
		sound = true;
		audioButton.image.sprite = audio_on;
	}

	public void init(float vol, bool intro)
	{
		int count = tracks.Count;
		playlist = new AudioClip[count];
		for (int i = 0; i < count; i++)
		{
			int index;
			if (i == 0 && intro)
			{
				Debug.Log("playing first track");
				index = 0;
			}
			else
			{
				index = Random.Range(0, tracks.Count);
			}
			playlist[i] = tracks[index];
			tracks.RemoveAt(index);
		}
		trackIndex = -1;
		sound = true;
		setVolume(vol);
		audioSlider.value = vol;
		play();
	}
}
