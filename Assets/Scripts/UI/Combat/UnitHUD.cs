using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UnitHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider manaSlider;
    [SerializeField] private Image deathCover;

    private Unit unit;

    public void SetUnit(Unit u)
    {
        unit = u;

        nameText.text = unit.UnitName;

        UpdateHealthHUD();
        UpdateManaHUD();

        unit.OnHealthChanged += UpdateHealthHUD;
        unit.OnMaxHealthChanged += HandleMaxHealthChange;
        unit.OnDeath += DisableHUD;
        unit.OnManaChanged += UpdateManaHUD;
    }

    private void HandleMaxHealthChange(Unit obj)
    {
        UpdateHealthHUD();
    }

    private void UpdateHealthHUD()
    {
        healthText.text = $"{Mathf.CeilToInt(unit.CurrentHealth)}/{unit.GetStats().health}";
        healthBar.value = unit.CurrentHealth / unit.GetStats().health;
    }

    private void UpdateManaHUD()
    {
        manaText.text = $"{unit.CurrentMana}/{unit.GetSpecialStats().mana}";
        manaSlider.value = unit.CurrentMana / unit.GetSpecialStats().mana;
    }

    private void DisableHUD(Unit unit)
    {
        if (unit.IsPlayer) 
        {
            deathCover.gameObject.SetActive(true);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void OnDestroy()
    {
        if (unit != null)
        {
            unit.OnHealthChanged -= UpdateHealthHUD;
            unit.OnDeath -= DisableHUD;
            unit.OnManaChanged -= UpdateManaHUD;
            unit.OnMaxHealthChanged -= HandleMaxHealthChange;
        }
    }
}
