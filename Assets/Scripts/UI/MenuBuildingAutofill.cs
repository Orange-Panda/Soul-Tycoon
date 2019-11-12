using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Automatically fills in the data for a building button.
/// </summary>
public class MenuBuildingAutofill : MonoBehaviour
{
	public BuildingProperties properties;
	[Space]
	public Image icon;
	public TextMeshProUGUI title, cost, description;

	private void Start()
	{
		title.SetText(properties.title);
		description.SetText(properties.description);
		cost.SetText(properties.cost.ToString());
		icon.sprite = properties.icon;
	}
}
