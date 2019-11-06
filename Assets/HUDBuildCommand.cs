using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDBuildCommand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	private Image image;
	private Animator animator;

	private const int MouseSampleCount = 8;

	private Vector3 initialPosition;
	private bool held = false;
	private bool cursorInside = false;

	private static bool trackingCursor;
	private static Vector3[] mouseSamples = new Vector3[MouseSampleCount];

	public static Vector3 AverageMousePosition { get; private set; }
	public static Quaternion GripRotation => Quaternion.Euler(0, 0, Mathf.Clamp((AverageMousePosition.x - Input.mousePosition.x) / 10, -80f, 80f));

	private void Start()
	{
		animator = GetComponent<Animator>();
		image = GetComponent<Image>();
		initialPosition = transform.position;
		if (!trackingCursor) StartCoroutine(TrackCursor());
	}

	private void Update()
	{
		transform.position = Vector3.Lerp(transform.position, held ? Input.mousePosition - new Vector3(10f, -10f, 0) : initialPosition, held ? 0.6f : 0.25f);
		transform.rotation = Quaternion.Slerp(transform.rotation, held ? GripRotation : Quaternion.identity, held ? 0.25f : 0.1f);
		image.color = held || cursorInside ? Color.gray : Color.white;
		Debug.Log(GetBuildableState().ToString());
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
	}

	public BuildableState GetBuildableState()
	{
		if (held)
		{
			return HUDBoundary.instance.GetMouseOverState() ? BuildableState.Held : BuildableState.Hover;
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