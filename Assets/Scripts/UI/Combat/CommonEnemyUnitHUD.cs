using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommonEnemyUnitHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Slider healthBar;

    private Unit unit;

    public void SetUnit(Unit u)
    {
        unit = u;

        nameText.text = unit.UnitName;

        UpdateHUD();

        unit.OnHealthChanged += UpdateHUD;
        unit.OnMaxHealthChanged += HandleMaxHealthChange;
        unit.OnDeath += DisableHUD;
    }

    private void HandleMaxHealthChange(Unit unit)
    {
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        healthBar.value = (float)unit.CurrentHealth / unit.GetStats().health;
    }

    private void DisableHUD(Unit unit)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (unit != null)
        {
            unit.OnHealthChanged -= UpdateHUD;
            unit.OnDeath -= DisableHUD;
            unit.OnMaxHealthChanged -= HandleMaxHealthChange;
        }
    }
}
