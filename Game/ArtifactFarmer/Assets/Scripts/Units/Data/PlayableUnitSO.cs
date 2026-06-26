using UnityEngine;

[CreateAssetMenu(fileName = "PlayableUnit", menuName = "ScriptableObjects/Units/Playable Unit")]
public class PlayableUnitSO : UnitBaseData
{
    [Space(10)]
    [Header("Prefab")]
    public PlayableUnit unitPrefab;

    [Space(10)]
    [Header("Character Info")]
    public CharacterClass characterClass;
    public Element element;
    public UnifiedStatType ascensionStat; //Provavelmente sera substituido por uma classe que irá conter todos as informações necessarias para ascensão

    [Space(10)]
    [Header("Character Skills")]
    public PassiveBase passive;
    public SkillDataSO basicSkill;
    public SkillDataSO specialSkill;
    public SkillDataSO ultimateSkill;
}
