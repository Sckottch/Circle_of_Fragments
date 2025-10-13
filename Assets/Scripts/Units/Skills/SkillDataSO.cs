using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/Skills/SkillData", order = 1)]
public class SkillDataSO : ScriptableObject
{
    [Space(10)]
    [Header("Informações Basicas")]
    public string skillName;
    [TextArea] public string skillDescription;
    public Sprite skillIcon;

    [Space(10)]
    [Header("Custo")]
    public float manaCost;

    [Header("Tipo de Habilidade e Escalonamento")]
    public ActionType skillType;
    public StatType scalingType;

    [Header("Tipo de Alvo")]
    public bool isSelf;
    public bool isMainTargetAlly;

    [Space(10)]
    [Header("Efeitos da Skill")]
    public List<SkillEffect> skillEffects;
}
