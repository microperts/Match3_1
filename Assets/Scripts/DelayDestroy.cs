using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
	public float delay;

	private void OnEnable()
	{
		Destroy(gameObject, delay);
	}
}
