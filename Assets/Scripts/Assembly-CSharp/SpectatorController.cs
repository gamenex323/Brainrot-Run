using System.Collections;
using UnityEngine;

public class SpectatorController : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	private void Start()
	{
		StartCoroutine(initAnimation());
	}

	private IEnumerator initAnimation()
	{
		int r2 = 0;
		while (r2 != 1)
		{
			r2 = Random.Range(0, 11);
			yield return new WaitForSeconds(0.1f);
		}
		animator.SetBool(AnimHashes.cheer, value: true);
		r2 = Random.Range(0, 2);
		if (r2 == 1)
		{
			Vector3 localScale = base.gameObject.transform.localScale;
			localScale.x *= -1f;
			base.gameObject.transform.localScale = localScale;
		}
	}

	private void Update()
	{
	}
}
