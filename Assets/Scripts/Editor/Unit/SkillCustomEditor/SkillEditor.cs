using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.VersionControl;

[CustomEditor(typeof(SkillDataSO))]
public class SkillEditor : Editor
{
    private VisualTreeAsset visualTree;
    private StyleSheet styleSheet;
    private VisualElement rootElement;

    private SkillDataSO skillData;
    private List<Type> skillEffectTypes;
    private DropdownField skillEffectDropdown;
    private Button addSkillEffectButton;
    private VisualElement skillEffectsContainer;

    public override VisualElement CreateInspectorGUI()
    {
        skillData = (SkillDataSO)target;

        visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/Unit/SkillCustomEditor/SkillEditorLayout.uxml");
        styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/Unit/SkillCustomEditor/SkillEdiotrStyle.uss");

        rootElement = visualTree.Instantiate();
        rootElement.styleSheets.Add(styleSheet);

        SerializedObject so = new(skillData);
        rootElement.Bind(so);

        skillEffectsContainer = rootElement.Q<VisualElement>("SkillEffectsContainer");
        addSkillEffectButton = rootElement.Q<Button>("AddSkillEffectButton");
        skillEffectDropdown = rootElement.Q<DropdownField>("SkillEffectDropdown");

        PopulateSkillEffectDropdown();
        addSkillEffectButton.clicked += OnAddSkillEffectButtonClicked;

        PopulateSkillEffects();

        return rootElement;
    }

    private void PopulateSkillEffectDropdown()
    {
        skillEffectTypes = TypeCache.GetTypesDerivedFrom<SkillEffect>()
            .Where(t => !t.IsAbstract).ToList();

        skillEffectDropdown.choices = skillEffectTypes.Select(t => t.Name).ToList();
        if (skillEffectDropdown.choices.Count > 0)
        {
            skillEffectDropdown.value = skillEffectDropdown.choices[0];
        }
    }

    private void OnAddSkillEffectButtonClicked()
    {
        string selectedTypeName = skillEffectDropdown.value;
        if (string.IsNullOrEmpty(selectedTypeName)) return;

        Type selectedType = skillEffectTypes.FirstOrDefault(t => t.Name == selectedTypeName);
        if (selectedType != null)
        {
            SkillEffect newEffect = ScriptableObject.CreateInstance(selectedType) as SkillEffect;
            newEffect.name = $"{skillData.skillName}_{selectedTypeName}";

            AssetDatabase.AddObjectToAsset(newEffect, skillData);
            skillData.skillEffects.Add(newEffect);

            EditorUtility.SetDirty(skillData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            PopulateSkillEffects();
        }
        else
        {
            Debug.LogError($"Type {selectedTypeName} not found.");
        }
    }

    private void PopulateSkillEffects()
    {
        skillEffectsContainer.Clear();

        foreach (SkillEffect effect in skillData.skillEffects)
        {
            Foldout foldout = new()
            {
                text = effect.GetType().Name,
                value = false
            };
            foldout.AddToClassList("skill-effect-foldout");

            Editor editor = Editor.CreateEditor(effect);
            if (editor != null)
            {
                VisualElement effectInspector = new IMGUIContainer(() => editor.OnInspectorGUI());

                effectInspector.style.marginRight = 4;
                effectInspector.style.marginLeft = 4;
                effectInspector.style.marginBottom = 4;
                effectInspector.style.marginTop = 4;

                foldout.Add(effectInspector);
            }

            Button removeButton = new(() =>
            {
                RemoveSubEffect(effect);
            })
            { text = "Remove Effect" };

            removeButton.AddToClassList("remove-effect-button");
            foldout.Add(removeButton);

            skillEffectsContainer.Add(foldout);
        }
    }

    private void RemoveSubEffect(SkillEffect effect)
    {
        if (skillData.skillEffects.Contains(effect))
        {
            skillData.skillEffects.Remove(effect);
            DestroyImmediate(effect, true);

            EditorUtility.SetDirty(skillData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            PopulateSkillEffects();
        }
    }

}