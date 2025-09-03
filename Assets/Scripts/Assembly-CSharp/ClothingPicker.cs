using UnityEngine;
using UnityEngine.UI;

public class ClothingPicker : MonoBehaviour
{
	public GameObject previewRacer;

	public PlayerAttributes att;

	public PreviewRacerAnimation pa;

	public ClothingManager clothingManager;

	public Button buttonDummy_prev;

	public Button buttonDummy_next;

	public Button buttonHeadband_prev;

	public Button buttonHeadband_next;

	public Button buttonTop_prev;

	public Button buttonTop_next;

	public Button buttonSleeve_prev;

	public Button buttonSleeve_next;

	public Button buttonBottoms_prev;

	public Button buttonBottoms_next;

	public Button buttonShoes_prev;

	public Button buttonShoes_next;

	public Button buttonSocks_prev;

	public Button buttonSocks_next;

    public void Initialize()
	{
		att = previewRacer.GetComponent<PlayerAttributes>();
		clothingManager = att.clothingManager;
	}

	private void Update()
	{
	}

	public void cycleMesh(string article)
	{
		switch (article)
		{
		case "dummy":
			att.setBodyProportions(PlayerAttributes.RANDOM);
			break;
		case "headband":
		{
			int socksMeshNumber = att.headbandMeshNumber;
			socksMeshNumber++;
			if (socksMeshNumber >= clothingManager.headbandMeshes.Length)
			{
				socksMeshNumber = 0;
			}
			att.headbandMeshNumber = socksMeshNumber;
			break;
		}
		case "top":
		{
			int socksMeshNumber = att.topMeshNumber;
			socksMeshNumber++;
			if (socksMeshNumber >= clothingManager.topMeshes.Length)
			{
				socksMeshNumber = 0;
			}
			att.topMeshNumber = socksMeshNumber;
			break;
		}
		case "sleeve":
		{
			int socksMeshNumber = att.sleeveMeshNumber;
			socksMeshNumber++;
			if (socksMeshNumber >= clothingManager.sleeveMeshes.Length)
			{
				socksMeshNumber = 0;
			}
			att.sleeveMeshNumber = socksMeshNumber;
			break;
		}
		case "bottoms":
		{
			int socksMeshNumber = att.bottomsMeshNumber;
			socksMeshNumber++;
			if (socksMeshNumber >= clothingManager.bottomsMeshes.Length)
			{
				socksMeshNumber = 0;
			}
			att.bottomsMeshNumber = socksMeshNumber;
			break;
		}
		case "shoes":
		{
			int socksMeshNumber = att.shoesMeshNumber;
			socksMeshNumber++;
			if (socksMeshNumber >= clothingManager.shoesMeshes.Length)
			{
				socksMeshNumber = 0;
			}
			att.shoesMeshNumber = socksMeshNumber;
			break;
		}
		case "socks":
		{
			int socksMeshNumber = att.socksMeshNumber;
			socksMeshNumber++;
			if (socksMeshNumber >= clothingManager.socksMeshes.Length)
			{
				socksMeshNumber = 0;
			}
			att.socksMeshNumber = socksMeshNumber;
			break;
		}
		}
		att.setClothing(PlayerAttributes.FROM_THIS);
	}

	public void setRandomClothing()
	{
		_ = PlayerAttributes.RANDOM;
		PlayerAttributes component = previewRacer.GetComponent<PlayerAttributes>();
		int[] randomMeshNumbers = component.clothingManager.getRandomMeshNumbers();
		int num = 0;
		component.dummyMeshNumber = randomMeshNumbers[num];
		num++;
		component.topMeshNumber = randomMeshNumbers[num];
		num++;
		component.bottomsMeshNumber = randomMeshNumbers[num];
		num++;
		component.shoesMeshNumber = randomMeshNumbers[num];
		num++;
		component.socksMeshNumber = randomMeshNumbers[num];
		num++;
		component.headbandMeshNumber = randomMeshNumbers[num];
		num++;
		component.sleeveMeshNumber = randomMeshNumbers[num];
		num++;
		component.setClothing(PlayerAttributes.FROM_THIS);
	}

	public void setRandomBodyProportions()
	{
		previewRacer.GetComponent<PlayerAttributes>().setBodyProportions(PlayerAttributes.RANDOM);
	}

	public void previewResetPos()
	{
		pa.resetPos();
	}

	public void previewLand()
	{
		pa.land();
	}
}
