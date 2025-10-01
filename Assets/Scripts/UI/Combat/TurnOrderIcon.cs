using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrderIcon : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI iconOrderText;
    [SerializeField] private Image unitIcon;

    private Unit unit;

    public void SetUnit(Unit u, int index)
    {
        unit = u;
        iconOrderText.text = index.ToString();
        unitIcon.sprite = u.UnitData.unitSprite;
    }

}
