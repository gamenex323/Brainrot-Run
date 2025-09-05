using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    public Animator animator;

    public RuntimeAnimatorController[] animatorControllers;

    public ClothingManager clothingManager;

	public static int DEFAULT_PATH_LENGTH = 9000;

	public static int RANDOM = 1;

	public static int FROM_THIS = 2;

	public static int DEFAULT = 3;

	public string id;

	public string racerName;

	public float[] personalBests;

	public int totalScore;

	public int lane;

	public bool isRacing;

	public float finishTime;

	public string resultString;

	public string resultTag;

	public string resultColor;

	public int pathLength;

	public int leanLockTick;

	public float[] velMagPath;

	public float[] velPathX;

	public float[] velPathY;

	public float[] velPathZ;

	public float[] posPathX;

	public float[] posPathY;

	public float[] posPathZ;

	public int[] rightInputPath;

	public int[] leftInputPath;

	public float[] sphere1Prog;

	public float[] sphere2Prog;

	public SkinnedMeshRenderer smr_dummy;

	public SkinnedMeshRenderer smr_top;

	public SkinnedMeshRenderer smr_bottoms;

	public SkinnedMeshRenderer smr_shoes;

	public SkinnedMeshRenderer smr_socks;

	public SkinnedMeshRenderer smr_headband;

	public SkinnedMeshRenderer smr_sleeve;

	public int dummyMeshNumber;

	public int topMeshNumber;

	public int bottomsMeshNumber;

	public int shoesMeshNumber;

	public int socksMeshNumber;

	public int headbandMeshNumber;

	public int sleeveMeshNumber;

	public int dummyMaterialNumber;

	public int topMaterialNumber;

	public int bottomsMaterialNumber;

	public int shoesMaterialNumber;

	public int socksMaterialNumber;

	public int headbandMaterialNumber;

	public int sleeveMaterialNumber;

	public float[] dummyRGB;

	public float[] topRGB;

	public float[] bottomsRGB;

	public float[] shoesRGB;

	public float[] socksRGB;

	public float[] headbandRGB;

	public float[] sleeveRGB;

	public Transform head;

	public Transform neck;

	public Transform torso;

	public Transform thighRight;

	public Transform thighLeft;

	public Transform shinRight;

	public Transform shinLeft;

	public Transform upperArmRight;

	public Transform upperArmLeft;

	public Transform lowerArmRight;

	public Transform lowerArmLeft;

	public Transform footRight;

	public Transform footLeft;

	public float headX;

	public float headY;

	public float headZ;

	public float neckX;

	public float neckY;

	public float neckZ;

	public float torsoX;

	public float torsoY;

	public float torsoZ;

	public float armX;

	public float armY;

	public float armZ;

	public float legX;

	public float legY;

	public float legZ;

	public float feetX;

	public float feetY;

	public float feetZ;

	public float height;

	public float weight;

	public float POWER;

	public float TRANSITION_PIVOT_SPEED;

	public float QUICKNESS;

	public float KNEE_DOMINANCE;

	public float TURNOVER;

	public float FITNESS;

	public float LAUNCH_POWER;

	public float CURVE_POWER;

	public float CRUISE;



	public int animatorNum;

	public int leadLeg;

	public float armSpeedFlexL;

	public float armSpeedExtendL;

	public float armSpeedFlexR;

	public float armSpeedExtendR;

	private void Start()
	{
		resultTag = "";
		if (base.tag.StartsWith("Player"))
		{
			resultColor = "yellow";
		}
		else if (base.tag.StartsWith("Ghost"))
		{
			resultColor = "#5962ff";
		}
		else if (base.tag.StartsWith("Bot"))
		{
			resultColor = "#666666";
		}
	}

	public void setInfo(int setting)
	{
		if (setting != FROM_THIS && setting == RANDOM)
		{
			racerName = TextReader.getRandomName();
		}
		id = generateID(racerName, PlayFabManager.thisUserDisplayName);
	}

	public void setClothing(int setting)
	{
		if (setting == DEFAULT)
		{
			dummyMeshNumber = 0;
			topMeshNumber = 0;
			bottomsMeshNumber = 0;
			shoesMeshNumber = 0;
			socksMeshNumber = 0;
			headbandMeshNumber = 0;
			sleeveMeshNumber = 2;
			dummyMaterialNumber = 0;
			topMaterialNumber = 0;
			bottomsMaterialNumber = 0;
			shoesMaterialNumber = 0;
			socksMaterialNumber = 0;
			headbandMaterialNumber = 0;
			sleeveMaterialNumber = 0;
			dummyRGB = new float[3] { 0.62f, 0.5f, 0.25f };
			topRGB = new float[3] { 1f, 1f, 1f };
			bottomsRGB = new float[3] { 1f, 1f, 1f };
			shoesRGB = new float[3] { 1f, 1f, 1f };
			socksRGB = new float[3] { 1f, 1f, 1f };
			headbandRGB = new float[3] { 1f, 1f, 1f };
			sleeveRGB = new float[3] { 1f, 1f, 1f };
		}
		else if (setting != FROM_THIS && setting == RANDOM)
		{
			int[] randomMeshNumbers = clothingManager.getRandomMeshNumbers();
			int num = 0;
			dummyMeshNumber = randomMeshNumbers[num];
			num++;
			topMeshNumber = randomMeshNumbers[num];
			num++;
			bottomsMeshNumber = randomMeshNumbers[num];
			num++;
			shoesMeshNumber = randomMeshNumbers[num];
			num++;
			socksMeshNumber = randomMeshNumbers[num];
			num++;
			headbandMeshNumber = randomMeshNumbers[num];
			num++;
			sleeveMeshNumber = randomMeshNumbers[num];
			num++;
			int[] randomMaterialNumbers = clothingManager.getRandomMaterialNumbers();
			num = 0;
			dummyMaterialNumber = randomMaterialNumbers[num];
			num++;
			topMaterialNumber = randomMaterialNumbers[num];
			num++;
			bottomsMaterialNumber = randomMaterialNumbers[num];
			num++;
			shoesMaterialNumber = randomMaterialNumbers[num];
			num++;
			socksMaterialNumber = randomMaterialNumbers[num];
			num++;
			headbandMaterialNumber = randomMaterialNumbers[num];
			num++;
			sleeveMaterialNumber = randomMaterialNumbers[num];
			num++;
			Color[] randomColors = clothingManager.getRandomColors();
			num = 0;
			dummyRGB = new float[3]
			{
				randomColors[num].r,
				randomColors[num].g,
				randomColors[num].b
			};
			num++;
			topRGB = new float[3]
			{
				randomColors[num].r,
				randomColors[num].g,
				randomColors[num].b
			};
			num++;
			bottomsRGB = new float[3]
			{
				randomColors[num].r,
				randomColors[num].g,
				randomColors[num].b
			};
			num++;
			shoesRGB = new float[3]
			{
				randomColors[num].r,
				randomColors[num].g,
				randomColors[num].b
			};
			num++;
			socksRGB = new float[3]
			{
				randomColors[num].r,
				randomColors[num].g,
				randomColors[num].b
			};
			num++;
			headbandRGB = new float[3]
			{
				randomColors[num].r,
				randomColors[num].g,
				randomColors[num].b
			};
			num++;
			sleeveRGB = new float[3]
			{
				randomColors[num].r,
				randomColors[num].g,
				randomColors[num].b
			};
			num++;
		}
		int[] array = new int[7] { shoesMeshNumber, socksMeshNumber, topMeshNumber, bottomsMeshNumber, sleeveMeshNumber, headbandMeshNumber, dummyMeshNumber };
		int[] array2 = new int[7] { shoesMaterialNumber, socksMaterialNumber, topMaterialNumber, bottomsMaterialNumber, sleeveMaterialNumber, headbandMaterialNumber, dummyMaterialNumber };
		float[][] array3 = new float[7][] { shoesRGB, socksRGB, topRGB, bottomsRGB, sleeveRGB, headbandRGB, dummyRGB };
		string[] array4 = new string[7] { "shoes", "socks", "top", "bottoms", "sleeve", "headband", "dummy" };
		SkinnedMeshRenderer[] array5 = new SkinnedMeshRenderer[7] { smr_shoes, smr_socks, smr_top, smr_bottoms, smr_sleeve, smr_headband, smr_dummy };
		for (int i = 0; i < array5.Length; i++)
		{
			SkinnedMeshRenderer obj = array5[i];
			Mesh mesh = clothingManager.getMesh(array4[i], array[i]);
			Material material = Object.Instantiate(clothingManager.getMaterial(array4[i], array2[i]));
			//material.color = new Color(array3[i][0], array3[i][1], array3[i][2]);
			material.color = Color.white;
			material.SetFloat("_Mode", 2f);
			material.SetInt("_SrcBlend", 5);
			material.SetInt("_DstBlend", 10);
			material.SetInt("_ZWrite", 1);
			material.DisableKeyword("_ALPHATEST_ON");
			material.EnableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = 3000 - i;
			obj.sharedMesh = mesh;
			obj.sharedMaterial = material;
		}
	}

	public void setBodyProportions(int setting)
	{
		if (setting == DEFAULT)
		{
			headX = 1f;
			neckX = 1f;
			torsoX = 1f;
			armX = 1f;
			legX = 1f;
			feetX = 1f;
			headY = 1f;
			neckY = 1f;
			torsoY = 1f;
			armY = 1f;
			legY = 1f;
			feetY = 1f;
			headZ = 1f;
			neckZ = 1f;
			torsoZ = 1f;
			armZ = 1f;
			legZ = 1f;
			feetZ = 1f;
		}
		else if (setting != FROM_THIS && setting == RANDOM)
		{
			headX = 1f;
			neckX = 1f;
			torsoX = 1f;
			armX = 1f;
			legX = 1f;
			feetX = 1f;
			headY = 1f;
			neckY = 1f;
			torsoY = 1f;
			armY = 1f;
			legY = 1f;
			feetY = 1f;
			headZ = 1f;
			neckZ = 1f;
			torsoZ = 1f;
			armZ = 1f;
			legZ = 1f;
			feetZ = 1f;
			torsoX = (Random.Range(0.95f, 1.05f) + Random.Range(0.95f, 1.05f)) / 2f;
			torsoY = Random.Range(0.9f, 1.1f);
			torsoZ = torsoY * Random.Range(0.8f, 1.2f);
			neckX *= 1f / torsoX;
			neckY *= 1f / torsoY;
			neckZ *= 1f / torsoZ;
			legX *= torsoX;
			legY *= 1f;
			legZ = 1f;
			armX *= Mathf.Pow(1f / torsoY, 0.75f);
			neckX *= Random.Range(0.5f, 1.5f);
			float num = Random.Range(0.95f, 1.05f);
			armX *= num;
			legX *= num;
			headX *= 1f / neckX * (1f / torsoX);
			feetX *= 2f - thighRight.localScale.x;
			feetZ *= 2f - thighRight.localScale.x;
		}
		setTorsoProportions(torsoX, torsoY, torsoZ);
		setHeadProportions(headX, headY, headZ);
		setNeckProportions(neckX, neckY, neckZ);
		setArmProportions(armX, armY, armZ);
		setLegProportions(legX, legY, legZ);
		setFeetProportions(feetX, feetY, feetZ);
		adjustHeightAndWeight();
		void adjustHeightAndWeight()
		{
			height *= Mathf.Pow(torsoX, 0.5f) * Mathf.Pow(legX, 0.5f) * Mathf.Pow(neckX, 0.05f);
			weight *= Mathf.Pow(height / 70f, 2.48f) * Mathf.Pow(torsoY * torsoZ, 0.8f);
		}
		void setArmProportions(float scaleX, float scaleY, float scaleZ)
		{
			Vector3 localScale3 = new Vector3(scaleX, scaleY, scaleZ);
			upperArmRight.localScale = localScale3;
			upperArmLeft.localScale = localScale3;
			lowerArmRight.localScale = localScale3;
			lowerArmLeft.localScale = localScale3;
		}
		void setFeetProportions(float scaleX, float scaleY, float scaleZ)
		{
			footRight.localScale = new Vector3(scaleX, scaleY, scaleZ);
			footLeft.localScale = footRight.localScale;
		}
		void setHeadProportions(float scaleX, float scaleY, float scaleZ)
		{
			head.localScale = new Vector3(scaleX, scaleY, scaleZ);
		}
		void setLegProportions(float scaleX, float scaleY, float scaleZ)
		{
			Vector3 localScale = new Vector3(scaleX, scaleY, scaleZ);
			Vector3 localScale2 = new Vector3(scaleX, 1f / scaleX, 1f / scaleX);
			thighRight.localScale = localScale;
			thighLeft.localScale = localScale;
			shinRight.localScale = localScale2;
			shinLeft.localScale = localScale2;
		}
		void setNeckProportions(float scaleX, float scaleY, float scaleZ)
		{
			neck.localScale = new Vector3(scaleX, scaleY, scaleZ);
		}
		void setTorsoProportions(float scaleX, float scaleY, float scaleZ)
		{
			torso.localScale = new Vector3(scaleX, scaleY, scaleZ);
		}
	}

	public void setStats(int setting)
	{
		float num = 2.8875f;
		float tRANSITION_PIVOT_SPEED = 200f;
		float qUICKNESS = 1f;
		float num2 = 1f;
		float tURNOVER = 1f;
		float num3 = 1f;
		float lAUNCH_POWER = 1f;
		float cURVE_POWER = 1f;
		float cRUISE = 1f;
		if (setting == DEFAULT)
		{
			POWER = num;
			TRANSITION_PIVOT_SPEED = tRANSITION_PIVOT_SPEED;
			QUICKNESS = qUICKNESS;
			KNEE_DOMINANCE = num2;
			TURNOVER = tURNOVER;
			FITNESS = num3;
			LAUNCH_POWER = lAUNCH_POWER;
			CURVE_POWER = cURVE_POWER;
			CRUISE = cRUISE;
		}
		else if (setting == FROM_THIS)
		{
			num = POWER;
			tRANSITION_PIVOT_SPEED = TRANSITION_PIVOT_SPEED;
			qUICKNESS = QUICKNESS;
			num2 = KNEE_DOMINANCE;
			tURNOVER = TURNOVER;
			num3 = FITNESS;
			lAUNCH_POWER = LAUNCH_POWER;
			cURVE_POWER = CURVE_POWER;
			cRUISE = CRUISE;
			POWER = num;
			TRANSITION_PIVOT_SPEED = tRANSITION_PIVOT_SPEED;
			QUICKNESS = qUICKNESS;
			KNEE_DOMINANCE = num2;
			TURNOVER = tURNOVER;
			FITNESS = num3;
			LAUNCH_POWER = lAUNCH_POWER;
			CURVE_POWER = cURVE_POWER;
			CRUISE = cRUISE;
		}
		else if (setting == RANDOM)
		{
			qUICKNESS = 0.95f * Mathf.Pow(2f - legX, 0.1f);
			tURNOVER = Mathf.Pow(2f - legX, 0.5f);
			cRUISE = Random.Range(0.5f, 1f);
			lAUNCH_POWER = Mathf.Pow(num2, 1f) * Mathf.Pow(2f - (cRUISE + 0.25f), 0.5f);
			num2 = Mathf.Pow(2f - legX, 1f);
			num *= Mathf.Pow(legX, 0.5f) * Mathf.Pow(2f - (cRUISE + 0.5f), 0.05f);
			num3 *= Mathf.Pow(cRUISE, 0.35f);
			POWER = num;
			TRANSITION_PIVOT_SPEED = tRANSITION_PIVOT_SPEED;
			QUICKNESS = qUICKNESS;
			KNEE_DOMINANCE = num2;
			TURNOVER = tURNOVER;
			FITNESS = num3;
			LAUNCH_POWER = lAUNCH_POWER;
			CURVE_POWER = cURVE_POWER;
			CRUISE = cRUISE;
		}
	}

	public void setAnimations(int setting)
	{
		if (setting != FROM_THIS && setting == RANDOM)
		{
			int num = Random.Range(0, animatorControllers.Length);
			animator.runtimeAnimatorController = animatorControllers[num];
			animatorNum = num;
			leadLeg = Random.Range(0, 2);
			armSpeedFlexL = Random.Range(0.9f, 1.1f);
			armSpeedExtendL = 2f - armSpeedFlexL;
			if (base.tag.StartsWith("Bot"))
			{
				armSpeedFlexR = armSpeedFlexL;
				armSpeedExtendR = armSpeedExtendL;
			}
			else
			{
				armSpeedFlexR = Random.Range(0.9f, 1.1f);
				armSpeedExtendR = 2f - armSpeedFlexR;
			}
		}
		animator.runtimeAnimatorController = animatorControllers[animatorNum];
	}

	public void setPaths(int setting)
	{
		velMagPath = new float[setting];
		velPathX = new float[setting];
		velPathY = new float[setting];
		velPathZ = new float[setting];
		posPathX = new float[setting];
		posPathZ = new float[setting];
		posPathY = new float[setting];
		rightInputPath = new int[setting];
		leftInputPath = new int[setting];
		sphere1Prog = new float[setting];
		sphere2Prog = new float[setting];
	}

	public void copyAttributesFromOther(GameObject other, string whichAttributes)
	{
		PlayerAttributes component = other.GetComponent<PlayerAttributes>();
		switch (whichAttributes)
		{
		case "all":
			id = component.id;
			racerName = component.racerName;
			personalBests = component.personalBests;
			totalScore = component.totalScore;
			resultString = component.resultString;
			copyAttributesFromOther(other, "stats");
			copyAttributesFromOther(other, "ghost data");
			copyAttributesFromOther(other, "clothing");
			copyAttributesFromOther(other, "body proportions");
			copyAttributesFromOther(other, "animation properties");
			break;
		case "info":
			racerName = component.racerName;
			personalBests = component.personalBests;
			totalScore = component.totalScore;
			break;
		case "stats":
			POWER = component.POWER;
			QUICKNESS = component.QUICKNESS;
			TRANSITION_PIVOT_SPEED = component.TRANSITION_PIVOT_SPEED;
			KNEE_DOMINANCE = component.KNEE_DOMINANCE;
			TURNOVER = component.TURNOVER;
			FITNESS = component.FITNESS;
			LAUNCH_POWER = component.LAUNCH_POWER;
			CURVE_POWER = component.CURVE_POWER;
			CRUISE = component.CRUISE;
			break;
		case "ghost data":
			pathLength = component.pathLength;
			leanLockTick = component.leanLockTick;
			velMagPath = component.velMagPath;
			velPathX = component.velPathX;
			velPathY = component.velPathY;
			velPathZ = component.velPathZ;
			posPathX = component.posPathX;
			posPathZ = component.posPathZ;
			posPathY = component.posPathY;
			rightInputPath = component.rightInputPath;
			leftInputPath = component.leftInputPath;
			sphere1Prog = component.sphere1Prog;
			sphere2Prog = component.sphere2Prog;
			break;
		case "clothing":
		{
			topMeshNumber = component.topMeshNumber;
			bottomsMeshNumber = component.bottomsMeshNumber;
			shoesMeshNumber = component.shoesMeshNumber;
			socksMeshNumber = component.socksMeshNumber;
			headbandMeshNumber = component.headbandMeshNumber;
			sleeveMeshNumber = component.sleeveMeshNumber;
			topMaterialNumber = component.topMaterialNumber;
			bottomsMaterialNumber = component.bottomsMaterialNumber;
			shoesMaterialNumber = component.shoesMaterialNumber;
			socksMaterialNumber = component.socksMaterialNumber;
			headbandMaterialNumber = component.headbandMaterialNumber;
			sleeveMaterialNumber = component.sleeveMaterialNumber;
			float[] array = component.dummyRGB;
			dummyRGB = new float[3]
			{
				array[0],
				array[1],
				array[2]
			};
			array = component.topRGB;
			topRGB = new float[3]
			{
				array[0],
				array[1],
				array[2]
			};
			array = component.bottomsRGB;
			bottomsRGB = new float[3]
			{
				array[0],
				array[1],
				array[2]
			};
			array = component.shoesRGB;
			shoesRGB = new float[3]
			{
				array[0],
				array[1],
				array[2]
			};
			array = component.socksRGB;
			socksRGB = new float[3]
			{
				array[0],
				array[1],
				array[2]
			};
			array = component.headbandRGB;
			headbandRGB = new float[3]
			{
				array[0],
				array[1],
				array[2]
			};
			array = component.sleeveRGB;
			sleeveRGB = new float[3]
			{
				array[0],
				array[1],
				array[2]
			};
			break;
		}
		case "body proportions":
			headX = component.headX;
			headY = component.headY;
			headZ = component.headZ;
			neckX = component.neckX;
			neckY = component.neckY;
			neckZ = component.neckZ;
			torsoX = component.torsoX;
			torsoY = component.torsoY;
			torsoZ = component.torsoZ;
			armX = component.armX;
			armY = component.armY;
			armZ = component.armZ;
			legX = component.legX;
			legY = component.legY;
			legZ = component.legZ;
			feetX = component.feetX;
			feetY = component.feetY;
			feetZ = component.feetZ;
			height = component.height;
			weight = component.weight;
			break;
		case "animation properties":
			animatorNum = component.animatorNum;
			leadLeg = component.leadLeg;
			armSpeedFlexL = component.armSpeedFlexL;
			armSpeedExtendL = component.armSpeedExtendL;
			armSpeedFlexR = component.armSpeedFlexR;
			armSpeedExtendR = component.armSpeedExtendR;
			break;
		}
	}

	public void renderInForeground()
	{
		Shader shader_renderOnTop = clothingManager.shader_renderOnTop;
		Material material = smr_dummy.materials[0];
		Color color = material.color;
		material.shader = shader_renderOnTop;
		material.color = color;
		smr_dummy.materials[0] = material;
		material = smr_top.materials[0];
		color = material.color;
		material.shader = shader_renderOnTop;
		material.color = color;
		smr_top.materials[0] = material;
		material = smr_bottoms.materials[0];
		color = material.color;
		material.shader = shader_renderOnTop;
		material.color = color;
		smr_bottoms.materials[0] = material;
		material = smr_shoes.materials[0];
		color = material.color;
		material.shader = shader_renderOnTop;
		material.color = color;
		smr_shoes.materials[0] = material;
		material = smr_socks.materials[0];
		color = material.color;
		material.shader = shader_renderOnTop;
		material.color = color;
		smr_socks.materials[0] = material;
		material = smr_headband.materials[0];
		color = material.color;
		material.shader = shader_renderOnTop;
		material.color = color;
		smr_headband.materials[0] = material;
		material = smr_sleeve.materials[0];
		color = material.color;
		material.shader = shader_renderOnTop;
		material.color = color;
		smr_sleeve.materials[0] = material;
	}

	public static string generateID(string racerName, string displayName)
	{
		string text = racerName + "_" + displayName + "_";
		for (int i = 0; i < 9; i++)
		{
			text += Random.Range(0, 10);
		}
		return text;
	}
}
