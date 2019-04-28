using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
