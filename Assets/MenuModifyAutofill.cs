using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuModifyAutofill : MonoBehaviour
{
	public enum AutofillTarget { Repair, Sell }
	public AutofillTarget type;
	public TextMeshProUGUI cost, title;

	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine(UpdateText());
	}

	IEnumerator UpdateText()
	{
		MenuChoice choice = GetComponentInParent<MenuChoice>();

		yield return null;
		switch (type)
		{
			case AutofillTarget.Repair:
				ProductionBuilding building = choice.targetTile.GetComponent<ProductionBuilding>();
				cost.SetText("-" + building.RepairCost.ToString());
				title.SetText(title.text + " (" + building.durability + "/" + building.thisBuilding.durability + ")");
				cost.color = new Color(0.25f, 0f, 0f);
				break;
			case AutofillTarget.Sell:
				cost.SetText("+" + choice.targetTile.building.value);
				cost.color = new Color(0f, 0.25f, 0f);
				break;
		}
		yield break;
	}
}
