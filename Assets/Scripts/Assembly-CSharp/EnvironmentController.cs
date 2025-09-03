using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
	[SerializeField]
	private Material[] skyboxMaterials;

	[SerializeField]
	private Color[] ambientColors;

	[SerializeField]
	private Color[] trackColors;

	[SerializeField]
	private Transform[] lightPositions;

	[SerializeField]
	private float[] lightIntensities;

	[SerializeField]
	private Material trackMaterial;

	[SerializeField]
	private Light light;

	[SerializeField]
	private ParticleSystem rainParticles;

	public static int SUNNY = 0;

	public static int PINK = 1;

	public static int OVERCAST = 2;

	public static int NIGHT = 3;

	public static int DUSK = 4;

	private int currentTheme;

	public void init(int theme)
	{
		currentTheme = theme;
		setTheme(currentTheme);
	}

	public void nextTheme()
	{
		currentTheme++;
		if (currentTheme >= skyboxMaterials.Length)
		{
			currentTheme = 0;
		}
		setTheme(currentTheme);
	}

	public void prevTheme()
	{
		currentTheme--;
		if (currentTheme < 0)
		{
			currentTheme = skyboxMaterials.Length - 1;
		}
		setTheme(currentTheme);
	}

	public void setTheme(int theme)
	{
		setSkybox(theme);
		setTrackColor(theme);
		setLightPosition(theme);
		setLightIntensity(theme);
		PlayerPrefs.SetInt("Theme", theme);
		if (theme == OVERCAST)
		{
			rainParticles.Play();
		}
		else
		{
			rainParticles.Stop();
		}
	}

	private void setSkybox(int theme)
	{
		RenderSettings.skybox = skyboxMaterials[theme];
	}

	private void setAmbientLight(int theme)
	{
		RenderSettings.ambientLight = ambientColors[theme];
	}

	private void setTrackColor(int theme)
	{
		trackMaterial.color = trackColors[theme];
	}

	private void setLightPosition(int theme)
	{
		light.transform.position = lightPositions[theme].position;
	}

	private void setLightIntensity(int theme)
	{
		light.intensity = lightIntensities[theme];
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
