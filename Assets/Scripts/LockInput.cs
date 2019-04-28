using UnityEngine;

/// <summary>
/// Prevents tiles from being interacted with while active.
/// </summary>
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
