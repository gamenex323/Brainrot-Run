using System.Collections;
using UnityEngine;

public class PreviewRacerAnimation : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private Transform bodyT;

	[SerializeField]
	private Transform footT;

	[SerializeField]
	private Transform platformT;

	[SerializeField]
	private Transform visibleLoc;

	[SerializeField]
	private Transform invisibleLoc;

	public void resetPos()
	{
		bodyT.position += base.transform.TransformDirection(Vector3.up);
		Physics.Raycast(footT.position, footT.TransformDirection(Vector3.down), out var hitInfo);
		float distance = hitInfo.distance;
		bodyT.transform.position += base.transform.TransformDirection(Vector3.down * distance);
	}

	public void land()
	{
		StartCoroutine(previewRacerLand());
	}

	private IEnumerator previewRacerLand()
	{
		animator.SetBool(AnimHashes.land, value: true);
		yield return new WaitForSeconds(0.2f);
		animator.SetBool(AnimHashes.land, value: false);
	}

	public void setVisibility(bool visible)
	{
		if (visible)
		{
			bodyT.transform.position = visibleLoc.position;
		}
		else
		{
			bodyT.transform.position = invisibleLoc.position;
		}
		platformT.transform.position = bodyT.transform.position;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
