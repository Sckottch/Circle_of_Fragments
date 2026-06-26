using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEditor.VersionControl;
using System.Linq;

public class PlayableUnitEditorWindow : EditorWindow
{
    private VisualElement root;
    private VisualTreeAsset visualTree;
    private StyleSheet styleSheet;

    private Button BasicButton;
    private Button SpecialButton;
    private Button UltimateButton;
    private Button PassiveButton;
    private ScrollView tabContent;

    private ObjectField spriteField;
    private VisualElement spritePreviewContainer;

    private PlayableUnitSO playableUnit;
    private SerializedObject serializedUnit;
    private Editor passiveEditor;

    [MenuItem("Tools/Playable Unit Editor")]
    public static void OpenWindow()
    {
        GetWindow<PlayableUnitEditorWindow>("Playable Unit Editor");
    }

    public static void Open(PlayableUnitSO playableUnit)
    {
        var window = GetWindow<PlayableUnitEditorWindow>("Playable Unit Editor");
        window.LoadUnit(playableUnit);
    }

    [OnOpenAsset(0)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj is PlayableUnitSO unit)
        {
            Open(unit);
            return true;
        }
        
        return false;
    }

    private void LoadUnit(PlayableUnitSO unit)
    {
        playableUnit = unit;
        serializedUnit = new SerializedObject(playableUnit);
        CreateGUI();
    }

    private void CreateGUI()
    {
        rootVisualElement.Clear();

        visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
            "Assets/Scripts/Editor/Unit/PlayableUnitEditor/UI/PlayableUnitEditorWindow.uxml"
        );

        styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
            "Assets/Scripts/Editor/Unit/PlayableUnitEditor/UI/PlayableUnitEditorWindow.uss"
        );

        root = visualTree.CloneTree();
        root.styleSheets.Add(styleSheet);

        root.style.flexGrow = 1;
        root.style.flexDirection = FlexDirection.Column;
        root.style.height = Length.Percent(100);
        root.style.width = Length.Percent(100);

        rootVisualElement.Add(root);

        if (playableUnit == null)
        {
            Label label = new("No Playable Unit Selected");
            rootVisualElement.Add(label);
            return;
        }

        root.Bind(serializedUnit);

        BasicButton = root.Q<Button>("BasicTabButton");
        SpecialButton = root.Q<Button>("SpecialTabButton");
        UltimateButton = root.Q<Button>("UltimateTabButton");
        PassiveButton = root.Q<Button>("PassiveTabButton");

        tabContent = root.Q<ScrollView>("TabsContentArea");

        spriteField = root.Q<ObjectField>("UnitSpriteField");
        spritePreviewContainer = root.Q<VisualElement>("PortraitImage");

        SetUnitSkills();
        SetupTabs();
        SetSpritePreview();
    }

    private void SetUnitSkills()
    {
        if (playableUnit.basicSkill == null)
            playableUnit.basicSkill = CreateBasicSkill();
        if (playableUnit.specialSkill == null)
            playableUnit.specialSkill = CreateSpecialSkill();
        if (playableUnit.ultimateSkill == null)
            playableUnit.ultimateSkill = CreateUltimateSkill();

        serializedUnit.Update();
        EditorUtility.SetDirty(playableUnit);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private SkillDataSO CreateBasicSkill()
    {
        SkillDataSO newSkill = CreateInstance<SkillDataSO>();
        newSkill.name = $"{playableUnit.name}_BasicSkill";

        AssetDatabase.AddObjectToAsset(newSkill, playableUnit);

        return newSkill;
    }

    private SkillDataSO CreateSpecialSkill()
    {
        SkillDataSO newSkill = CreateInstance<SkillDataSO>();
        newSkill.name = $"{playableUnit.name}_SpecialSkill";

        AssetDatabase.AddObjectToAsset(newSkill, playableUnit);

        return newSkill;
    }

    private SkillDataSO CreateUltimateSkill()
    {
        SkillDataSO newSkill = CreateInstance<SkillDataSO>();
        newSkill.name = $"{playableUnit.name}_UltimateSkill";

        AssetDatabase.AddObjectToAsset(newSkill, playableUnit);

        return newSkill;
    }

    private void SetupTabs()
    {
        BasicButton.clicked += () => ShowTab(ActionType.BasicAttack);
        SpecialButton.clicked += () => ShowTab(ActionType.Skill);
        UltimateButton.clicked += () => ShowTab(ActionType.Ultimate);
        PassiveButton.clicked += () => ShowTab(ActionType.Passive);

        ShowTab(ActionType.BasicAttack);
    }

    private void ShowTab(ActionType actionType)
    {
        tabContent.Clear();

        BasicButton.RemoveFromClassList("active");
        SpecialButton.RemoveFromClassList("active");
        UltimateButton.RemoveFromClassList("active");
        PassiveButton.RemoveFromClassList("active");    

        switch (actionType)
        {
            case ActionType.BasicAttack:
                SetSkillTab(playableUnit.basicSkill);
                BasicButton.AddToClassList("active");
                break;

            case ActionType.Skill:
                SetSkillTab(playableUnit.specialSkill);
                SpecialButton.AddToClassList("active");
                break;

            case ActionType.Ultimate:
                SetSkillTab(playableUnit.ultimateSkill);
                UltimateButton.AddToClassList("active");
                break;

            case ActionType.Passive:
                SetPassiveTab();
                PassiveButton.AddToClassList("active");
                break;

            default:
                break;
        }
    }

    private void SetSkillTab(SkillDataSO skill)
    {
        if (skill == null)
        {
            Label label = new("No Skill Assigned");
            tabContent.Add(label);
            return;
        }

        Editor editor = Editor.CreateEditor(skill);

        if (editor != null)
        {
            VisualElement skillEditorUI = editor.CreateInspectorGUI();
            tabContent.Add(skillEditorUI);
        }
        else
        {
            Label label = new("Failed to create skill editor.");
            tabContent.Add(label);
        }
    }

    private void SetPassiveTab()
    {
        if (playableUnit.passive == null)
        {
            SetPassiveCreationUI();
            return;
        }

        if (passiveEditor != null)
        {
            DestroyImmediate(passiveEditor);
            passiveEditor = null;
        }

        passiveEditor = Editor.CreateEditor(playableUnit.passive);

        if (passiveEditor != null)
        {
            VisualElement passiveEditorUI = new IMGUIContainer(() => passiveEditor.OnInspectorGUI());
            tabContent.Add(passiveEditorUI);
        }
        else
        {
            Label label = new("Failed to create passive editor.");
            tabContent.Add(label);
        }

        Button passiveRemoveButton = new(() =>
        {
            RemoveUnitPassive();
            ShowTab(ActionType.Passive);
        })
        { text = "Remove Passive" };

        passiveRemoveButton.style.marginTop = 10;
        tabContent.Add(passiveRemoveButton);
    }

    private void SetPassiveCreationUI()
    {
        Label label = new("No Passive Assigned");
        tabContent.Add(label);

        DropdownField passiveTypeDropdown = new();

        PopulatePassiveTypes(passiveTypeDropdown);
        tabContent.Add(passiveTypeDropdown);

        Button createButton = new(() =>
        {
            CreateAndAssignPassive(passiveTypeDropdown.value);
            ShowTab(ActionType.Passive);
        })
        { text = "Create Passive" };

        tabContent.Add(createButton);
    }

    private void PopulatePassiveTypes(DropdownField dropdown)
    {
        var passiveTypes = TypeCache.GetTypesDerivedFrom<PassiveBase>()
            .Where(t => !t.IsAbstract).ToList();

        dropdown.choices = passiveTypes.Select(t => t.Name).ToList();

        if (passiveTypes.Count > 0)
            dropdown.value = dropdown.choices[0];
    }

    private void CreateAndAssignPassive(string typeName)
    {
        var passiveTypes = TypeCache.GetTypesDerivedFrom<PassiveBase>()
            .Where(t => !t.IsAbstract).ToList();

        var selectedType = passiveTypes.FirstOrDefault(t => t.Name == typeName);

        if (selectedType == null)
        {
            Debug.LogError($"Passive type {typeName} not found.");
            return;
        }

        PassiveBase newPassive = ScriptableObject.CreateInstance(selectedType) as PassiveBase;
        newPassive.name = $"{playableUnit.name}_Passive";

        AssetDatabase.AddObjectToAsset(newPassive, playableUnit);
        playableUnit.passive = newPassive;

        serializedUnit.Update();
        EditorUtility.SetDirty(playableUnit);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void RemoveUnitPassive()
    {
        if (playableUnit.passive != null)
        {
            ScriptableObject passiveToRemove = playableUnit.passive;
            playableUnit.passive = null;

            serializedUnit.Update();
            EditorUtility.SetDirty(playableUnit);

            AssetDatabase.RemoveObjectFromAsset(passiveToRemove);
            DestroyImmediate(passiveToRemove, true);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    
    private void SetSpritePreview()
    {
        if (playableUnit.unitSprite != null)
        {
            spritePreviewContainer.style.backgroundImage =
                new StyleBackground(playableUnit.unitSprite);
        }
        else
        {
            spritePreviewContainer.style.backgroundImage = null;
        }
        
        spriteField.RegisterValueChangedCallback(evt =>
        {
            Sprite newSprite = evt.newValue as Sprite;
            if (newSprite != null)
            {
                spritePreviewContainer.style.backgroundImage =
                    new StyleBackground(newSprite);
            }
            else
            {
                spritePreviewContainer.style.backgroundImage = null;
            }
        });
    }

}
