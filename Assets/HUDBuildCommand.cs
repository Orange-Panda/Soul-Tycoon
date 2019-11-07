using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Handles the input and appearance for tile building. Can't expliclty build tiles itself.
/// </summary>
public class HUDBuildCommand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	//Component references
	public Image image;
	private Canvas canvas;
	public static HUDBuildCommand instance;

	//Constants
	private const int MouseSampleCount = 8;

	//Private fields
	private Vector3 initialPosition;
	private bool held = false;
	private bool cursorInside = false;
	private static bool trackingCursor;
	private static Vector3[] mouseSamples = new Vector3[MouseSampleCount];

	//Properties
	public static Vector3 AverageMousePosition { get; private set; }
	public static Quaternion GripRotation => Quaternion.Euler(0, 0, Mathf.Clamp((AverageMousePosition.x - Input.mousePosition.x) / 10, -80f, 80f));

	private void Start()
	{
		canvas = GetComponent<Canvas>();
		initialPosition = transform.position;
		instance = this;
		if (!trackingCursor) StartCoroutine(TrackCursor());
	}

	private void Update()
	{
		//Modify the transform and appearance of the object
		transform.position = Vector3.Lerp(transform.position, held ? Input.mousePosition - new Vector3(10f, -10f, 0) : initialPosition, held ? 0.6f : 0.25f);
		transform.rotation = Quaternion.Slerp(transform.rotation, held ? GripRotation : Quaternion.identity, held ? 0.25f : 0.1f);
		image.color = held || cursorInside ? Color.gray : Color.white;
		canvas.enabled = GetBuildableState() == BuildableState.Dragging ? false : true;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		cursorInside = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		cursorInside = false;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		held = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		held = false;
		transform.position = initialPosition;

		if (GetBuildableState() == BuildableState.Dragging)
		{
			TilePlaceable.AttemptBuild();
		}
	}

	public BuildableState GetBuildableState()
	{
		if (held)
		{
			return HUDBoundary.instance.GetMouseOverState() ? BuildableState.Held : BuildableState.Dragging;
		}
		else if (cursorInside)
		{
			return BuildableState.Hover;
		}
		else return BuildableState.Null;
	}

	/// <summary>
	/// Tracks the cursor to get an average position.
	/// </summary>
	private IEnumerator TrackCursor()
	{
		trackingCursor = true;
		while (true)
		{
			for (int i = 0; i < MouseSampleCount; i++)
			{
				mouseSamples[i] = Input.mousePosition;

				AverageMousePosition = Vector3.zero;
				foreach (Vector3 sample in mouseSamples)
				{
					AverageMousePosition += sample / MouseSampleCount;
				}

				yield return null;
			}
		}
	}
}

public enum BuildableState
{ 
	Null, Hover, Held, Dragging
}