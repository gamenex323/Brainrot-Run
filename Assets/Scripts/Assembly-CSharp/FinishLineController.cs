using UnityEngine;

public class FinishLineController : MonoBehaviour
{
	public RaceManager raceManager;

	public ParticleSystem yayParticles;

	public bool isActive;

	private int finishers;

	public void init()
	{
		isActive = false;
		finishers = 0;
	}

	private void OnTriggerEnter(Collider col)
	{
		GameObject gameObject = col.gameObject;
		if (!(gameObject.tag == "Chest"))
		{
			return;
		}
		GameObject gameObject2 = gameObject.transform.parent.parent.parent.parent.parent.parent.parent.gameObject;
		if (!(raceManager.raceTime > 3f))
		{
			return;
		}
		if (gameObject2 == raceManager.player)
		{
			if (isActive)
			{
				raceManager.addFinisher(gameObject2);
				finishers++;
			}
		}
		else
		{
			raceManager.addFinisher(gameObject2);
			finishers++;
		}
	}
}
