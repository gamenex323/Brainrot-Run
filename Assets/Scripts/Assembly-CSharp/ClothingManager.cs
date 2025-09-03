using UnityEngine;

public class ClothingManager : MonoBehaviour
{
	public int numberOfArticles;

	public Mesh[] dummyMeshes;

	public Mesh[] topMeshes;

	public Mesh[] bottomsMeshes;

	public Mesh[] shoesMeshes;

	public Mesh[] socksMeshes;

	public Mesh[] headbandMeshes;

	public Mesh[] sleeveMeshes;

	public Material[] dummyMaterials;

	public Material[] topMaterials;

	public Material[] bottomsMaterials;

	public Material[] shoesMaterials;

	public Material[] socksMaterials;

	public Material[] headbandMaterials;

	public Material[] sleeveMaterials;

	public Shader shader_renderOnTop;

	public string[] skinTones = new string[7] { "#ffad60", "#ffe39f", "#c69076", "#af6e51", "#843722", "#3d0c02", "#260701" };

	private void Start()
	{
	}

	private void Update()
	{
	}

	public Mesh getMesh(string article, int index)
	{
		return article switch
		{
			"dummy" => dummyMeshes[index], 
			"top" => topMeshes[index], 
			"bottoms" => bottomsMeshes[index], 
			"shoes" => shoesMeshes[index], 
			"socks" => socksMeshes[index], 
			"headband" => headbandMeshes[index], 
			"sleeve" => sleeveMeshes[index], 
			_ => null, 
		};
	}

	public Material getMaterial(string article, int index)
	{
		return article switch
		{
			"dummy" => dummyMaterials[index], 
			"top" => topMaterials[index], 
			"bottoms" => bottomsMaterials[index], 
			"shoes" => shoesMaterials[index], 
			"socks" => socksMaterials[index], 
			"headband" => headbandMaterials[index], 
			"sleeve" => sleeveMaterials[index], 
			_ => null, 
		};
	}

	public int[] getRandomMeshNumbers()
	{
		int[] array = new int[numberOfArticles];
		string text = "";
		int num = Random.Range(0, 4);
		text = ((num >= 3) ? "long" : "short");
		array[0] = Random.Range(0, dummyMeshes.Length);
		if (text == "short")
		{
			array[1] = 0;
			array[2] = 0;
		}
		else
		{
			array[1] = 1;
			array[2] = 1;
		}
		array[3] = Random.Range(0, shoesMeshes.Length);
		num = ((Random.Range(0, 2) != 0) ? Random.Range(0, socksMeshes.Length - 1) : (socksMeshes.Length - 1));
		array[4] = num;
		num = Random.Range(0, 4);
		num = ((num >= 3) ? Random.Range(0, headbandMeshes.Length - 1) : (headbandMeshes.Length - 1));
		array[5] = num;
		if (text == "short")
		{
			num = Random.Range(0, 7);
			num = ((num >= 6) ? Random.Range(0, sleeveMeshes.Length - 1) : (sleeveMeshes.Length - 1));
		}
		else
		{
			num = sleeveMeshes.Length - 1;
		}
		array[6] = num;
		return array;
	}

	public int[] getRandomMaterialNumbers()
	{
		int[] array = new int[numberOfArticles];
		array[0] = Random.Range(0, dummyMaterials.Length);
		array[1] = Random.Range(0, topMaterials.Length);
		array[2] = Random.Range(0, bottomsMaterials.Length);
		array[3] = Random.Range(0, shoesMaterials.Length);
		array[4] = Random.Range(0, socksMaterials.Length);
		array[5] = Random.Range(0, headbandMaterials.Length);
		array[6] = Random.Range(0, sleeveMaterials.Length);
		return array;
	}

	public Color[] getRandomColors()
	{
		Random.Range(0, 3);
		float num = Random.Range(0.35f, 0.37f);
		float num2 = Random.Range(-0.4f, 0.4f);
		float num3 = 0.6f + num2;
		float num4 = 0.6f - num2;
		Mathf.Clamp(num3, 0.2f, 1f);
		Mathf.Clamp(num4, 0.2f, 1f);
		Color color = Color.HSVToRGB(num - 0.3f, num3, num4);
		float h = Random.Range(0f, 1f);
		num3 = 1f;
		num4 = Random.Range(1f, 1f);
		Color color2 = Color.HSVToRGB(h, num3, num4);
		Color color3 = Color.white;
		switch (Random.Range(0, 3))
		{
		case 0:
			color3 = color2;
			break;
		case 1:
			color3 = Color.black;
			break;
		}
		Color color4 = Color.white;
		switch (Random.Range(0, 3))
		{
		case 0:
			color4 = color2;
			break;
		case 1:
			color4 = color3;
			break;
		}
		Color color5 = Color.white;
		switch (Random.Range(0, 2))
		{
		case 0:
			color5 = color4;
			break;
		}
		Color color6 = Color.white;
		switch (Random.Range(0, 3))
		{
		case 0:
			color6 = color2;
			break;
		case 1:
			color6 = color3;
			break;
		}
		Color color7 = Color.white;
		switch (Random.Range(0, 2))
		{
		case 0:
			color7 = color2;
			break;
		}
		return new Color[7] { color, color2, color3, color4, color5, color6, color7 };
	}
}
