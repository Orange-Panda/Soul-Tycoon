using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Handles the input and appearance for tile buildable menu option. 
/// Can't expliclty build tiles itself.
/// </summary>
public class HUDBuildCommand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	//Component references
	private Image image;
	public static HUDBuildCommand selected;

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
		image = GetComponentInChildren<Image>();
		initialPosition = transform.position;
		if (!trackingCursor) StartCoroutine(TrackCursor());
	}

	private void Update()
	{
		//Cancel
		if (held && Input.GetMouseButtonDown(1))
		{
			Return();
		}
		
		//Modify the transform and appearance of the object
		transform.position = Vector3.Lerp(transform.position, held ? Input.mousePosition : initialPosition, held ? 0.6f : 0.25f);
		transform.rotation = Quaternion.Slerp(transform.rotation, held ? GripRotation : Quaternion.identity, held ? 0.25f : 0.1f);
		image.color = held || cursorInside ? Color.gray : Color.white;
		image.enabled = GetBuildableState() == BuildableState.Dragging ? false : true;
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
		selected = this;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (GetBuildableState() == BuildableState.Dragging)
		{
			TilePlaceable.instance.AttemptBuild();
		}

		Return();
	}

	public void Return()
	{
		held = false;
		transform.position = initialPosition;
		selected = null;
	}

	private BuildableState GetBuildableState()
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
	/// Checks what the global buildable state is.
	/// </summary>
	/// <returns>The buildable state of the selected buildable, otherwise is null.</returns>
	public static BuildableState GetBuildableStateGlobal()
	{
		if (selected)
		{
			return selected.GetBuildableState();
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
	/// <summary> Idle state </summary>
	Null,
	/// <summary> Cursor hovering </summary>
	Hover,
	/// <summary> Selected </summary>
	Held,
	/// <summary> Selected and inside play area </summary>
	Dragging
}