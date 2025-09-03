using UnityEngine;
using UnityEngine.UI;

public class DeleteDialogController : MonoBehaviour
{
	[SerializeField]
	private GlobalController gc;

	[SerializeField]
	private GameObject root;

	[SerializeField]
	public SelectionListScript list1;

	[SerializeField]
	public SelectionListScript list2;

	[SerializeField]
	private RectTransform rectTransform;

	[SerializeField]
	private RectTransform canvasRectTransform;

	[SerializeField]
	private Toggle deleteGhostToggle;

	public void delete()
	{
		list1.deleteButtonToDelete();
		if (deleteGhostToggle.isOn)
		{
			list2.deleteButtonToDelete();
		}
		gc.unlockManager.unlockCharacterSlot();
	}

	public void show()
	{
		rectTransform.anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x + new Vector3(0f, -270f, 0f);
		root.SetActive(value: true);
	}

	public void hide()
	{
		root.SetActive(value: false);
	}

	private void Start()
	{
		hide();
	}

	private void Update()
	{
	}
}
