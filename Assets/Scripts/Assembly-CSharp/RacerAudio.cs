using UnityEngine;

public class RacerAudio : MonoBehaviour
{
	public AudioSource as_startingGun;

	public AudioSource as_blockRattle;

	public AudioSource as_blockExit;

	public AudioSource as_footfall;

	public AudioSource as_wind;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void playSound(string sound)
	{
		switch (sound)
		{
		case "Starting Gun":
			as_startingGun.Play(0uL);
			break;
		case "Block Rattle":
			as_blockRattle.Play(0uL);
			break;
		case "Block Exit":
			as_blockExit.Play(0uL);
			break;
		case "Footfall":
			as_footfall.Play(0uL);
			break;
		case "Wind":
			as_wind.Play(0uL);
			break;
		}
	}
}
