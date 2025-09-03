using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnergyMeterController : MonoBehaviour
{
	private float energy;

	public Image UIImage;

	public bool hasUIImage;

	private float UIAlpha;

	public ParticleSystem sweatParticles;

	public GameObject root;

	public Camera camera;

	public bool sweatFlag;

	public TextMeshProUGUI energytext;

	private void Start()
	{
		init();
	}

	private void Update()
	{
		if (energytext)
			energytext.text = "Energy: " + Mathf.RoundToInt(energy);

		if (energy < 100f && hasUIImage)
		{
			UIImage.transform.localScale = new Vector3(energy * 0.01f, 1f, 1f);
			Color color = Color.Lerp(Color.red, Color.green, energy / 120f);
			if (energy < 37f)
			{
				UIAlpha = Mathf.PingPong(Time.time * 3f, 1f);
				color.a = UIAlpha;
			}
			UIImage.color = color;

		}
	}

	public void adjustForEnergyLevel(float _energy)
	{
		energy = _energy;
		if (energy < 60f)
		{
			if (!sweatFlag)
			{
				sweatFlag = true;
				sweatParticles.Play();
			}
			else
			{
				float num = 1f - energy / 100f;
				sweatParticles.transform.localScale = Vector3.one * num;
			}
		}
	}

	public void setUIImage(Image img, TextMeshProUGUI eText)
	{
		UIImage = img;
		energytext = eText;
		hasUIImage = true;
	}

	public void init()
	{
		sweatFlag = false;
		sweatParticles.Stop();
		energy = 100f;
		UIAlpha = 1f;
		if (hasUIImage)
		{
			UIImage.transform.localScale = Vector3.one;
			UIImage.color = Color.green;
		}
	}
}
