using System.Collections;
using UnityEngine;

public class HurdleController : MonoBehaviour
{
	public Collider barCollider;

	public Collider stickCollider_right;

	public Collider stickCollider_left;

	public Collider[] colliders;

    private void Start()
	{
        colliders  = GetComponents<BoxCollider>();

    }

	private void Update()
	{
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Ground")
        {
            StartCoroutine(WaitAndIgnoreCollision(collision.collider, 0.1f));
        }
    }

    private IEnumerator WaitAndIgnoreCollision(Collider otherCollider, float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (BoxCollider coll in colliders)
        {
            Physics.IgnoreCollision(coll, otherCollider);
        }
        foreach (BoxCollider coll in colliders)
        {
            Physics.IgnoreCollision(coll, otherCollider);
        }
    }

}
