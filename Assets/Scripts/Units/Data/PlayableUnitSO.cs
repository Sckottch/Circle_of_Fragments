using UnityEngine;

[CreateAssetMenu(fileName = "PlayableUnit", menuName = "ScriptableObjects/Units/Playable Unit")]
public class PlayableUnitSO : UnitBaseData
{
    [Space(10)]
    [Header("Character Prefab")]
    public PlayableUnit unitPrefab;

    [Space(10)]
    [Header("Element")]
    public Element element;

    [Space(10)]
    [Header("Character Skills")]
    public SkillDataSO basicSkill;
    public SkillDataSO specialSkill;
    public SkillDataSO ultimateSkill;

}
