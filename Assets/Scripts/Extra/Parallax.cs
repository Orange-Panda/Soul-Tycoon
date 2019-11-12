using UnityEngine;

/// <summary>
/// Moves the object according to the camera position to give the illusion of depth.
/// </summary>
public class Parallax : MonoBehaviour
{
	Transform target;
	[Range(0f, 1f)] public float strength = 0.85f;

	private void Start()
	{
		target = FindObjectOfType<Camera>().transform;
	}

	private void Update()
	{
		Vector2 goal = Vector2.Lerp(new Vector2(0, 0), target.transform.position, strength);
		transform.position = new Vector3(goal.x, goal.y, 0);
	}
}
