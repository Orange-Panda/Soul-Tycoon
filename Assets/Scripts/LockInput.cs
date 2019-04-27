using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockInput : MonoBehaviour
{
	private void OnEnable()
	{
		Tile.inputEnabled = false;
	}

	private void OnDisable()
	{
		Tile.inputEnabled = true;
	}
}
