using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
	public Camera camera;

	public GameObject previewRacer;

	public GameObject colorButtonGrid;

	public GameObject colorPicker_skin;

	public GameObject skinTextureObject;

	public GameObject colorPicker_clothing;

	public GameObject clothingTextureObject;

	public Color selectedColor;

	[SerializeField]
	private bool colorPickerOpen;

	public Button buttonDummy;

	public Button buttonHeadband;

	public Button buttonTop;

	public Button buttonSleeve;

	public Button buttonBottoms;

	public Button buttonShoes;

	public Button buttonSocks;

	public Button[] buttons;

	public Button selectedButton;

	public string selectedArticle;

	[SerializeField]
	private TooltipController tooltipController;

	private void Start()
	{
	}

	private void Update()
	{
		if (!colorPickerOpen || !Input.GetMouseButton(0) || !Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out var hitInfo))
		{
			return;
		}
		Renderer component = hitInfo.transform.GetComponent<Renderer>();
		MeshCollider meshCollider = hitInfo.collider as MeshCollider;
		if ((component.gameObject == skinTextureObject || component.gameObject == clothingTextureObject) && !(component == null) && !(component.sharedMaterial == null) && !(component.sharedMaterial.mainTexture == null) && !(meshCollider == null))
		{
			Texture2D texture2D = component.material.mainTexture as Texture2D;
			Vector2 textureCoord = hitInfo.textureCoord;
			textureCoord.x *= texture2D.width;
			textureCoord.y *= texture2D.height;
			selectedColor = texture2D.GetPixel(Mathf.FloorToInt(textureCoord.x), Mathf.FloorToInt(textureCoord.y));
			PlayerAttributes component2 = previewRacer.GetComponent<PlayerAttributes>();
			if (selectedArticle == "dummy")
			{
				component2.dummyRGB = new float[3] { selectedColor.r, selectedColor.g, selectedColor.b };
			}
			else if (selectedArticle == "headband")
			{
				component2.headbandRGB = new float[3] { selectedColor.r, selectedColor.g, selectedColor.b };
			}
			else if (selectedArticle == "top")
			{
				component2.topRGB = new float[3] { selectedColor.r, selectedColor.g, selectedColor.b };
			}
			else if (selectedArticle == "sleeve")
			{
				component2.sleeveRGB = new float[3] { selectedColor.r, selectedColor.g, selectedColor.b };
			}
			else if (selectedArticle == "bottoms")
			{
				component2.bottomsRGB = new float[3] { selectedColor.r, selectedColor.g, selectedColor.b };
			}
			else if (selectedArticle == "shoes")
			{
				component2.shoesRGB = new float[3] { selectedColor.r, selectedColor.g, selectedColor.b };
			}
			else if (selectedArticle == "socks")
			{
				component2.socksRGB = new float[3] { selectedColor.r, selectedColor.g, selectedColor.b };
			}
			component2.setClothing(PlayerAttributes.FROM_THIS);
		}
	}

	public void setSelectedArticle(string article)
	{
		selectedArticle = article;
		float num = 27.27f;
		if (selectedArticle == "dummy")
		{
			num *= 1f;
		}
		else if (selectedArticle == "headband")
		{
			num *= 2f;
		}
		else if (selectedArticle == "top")
		{
			num *= 3f;
		}
		else if (selectedArticle == "sleeve")
		{
			num *= 4f;
		}
		else if (selectedArticle == "bottoms")
		{
			num *= 5f;
		}
		else if (selectedArticle == "shoes")
		{
			num *= 6f;
		}
		else if (selectedArticle == "socks")
		{
			num *= 7f;
		}
	}

	public void openColorPicker(string s)
	{
		string[] array = s.Split('_');
		string text = array[0];
		int num = int.Parse(array[1]);
		_ = colorButtonGrid.transform.Find("ColorPickerButton (" + num + ")").gameObject;
		if (colorPickerOpen)
		{
			closeColorPicker();
		}
		GameObject gameObject = null;
		if (text == "skin")
		{
			gameObject = colorPicker_skin;
		}
		else if (text == "clothing")
		{
			gameObject = colorPicker_clothing;
		}
		gameObject.SetActive(value: true);
		colorPickerOpen = true;
	}

	public void closeColorPicker()
	{
		colorPicker_skin.SetActive(value: false);
		colorPicker_clothing.SetActive(value: false);
		colorPickerOpen = false;
	}

	public void setRandomColors()
	{
		PlayerAttributes component = previewRacer.GetComponent<PlayerAttributes>();
		Color[] randomColors = component.clothingManager.getRandomColors();
		int num = 0;
		Color color = randomColors[num];
		component.dummyRGB = new float[3] { color.r, color.g, color.b };
		num++;
		color = randomColors[num];
		component.topRGB = new float[3] { color.r, color.g, color.b };
		num++;
		color = randomColors[num];
		component.bottomsRGB = new float[3] { color.r, color.g, color.b };
		num++;
		color = randomColors[num];
		component.shoesRGB = new float[3] { color.r, color.g, color.b };
		num++;
		color = randomColors[num];
		component.socksRGB = new float[3] { color.r, color.g, color.b };
		num++;
		color = randomColors[num];
		component.headbandRGB = new float[3] { color.r, color.g, color.b };
		num++;
		color = randomColors[num];
		component.sleeveRGB = new float[3] { color.r, color.g, color.b };
		num++;
		component.setClothing(PlayerAttributes.FROM_THIS);
	}
}
