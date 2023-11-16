using System;
using System.IO;
using CryptoQuest.AbilitySystem;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Abilities.PostNormalAttackPassive;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Gameplay.Battle.Core;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using IndiGames.Tools.ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Gameplay.Abilities
{
    public class PassiveAbilitySOEditor : ScriptableObjectBrowserEditor<PassiveAbility>
    {
        private const string DEFAULT_NAME = "";
        private const int ROW_OFFSET = 1;

        #region Indexing

        private const int ID_INDEX = 0;
        private const int LOCALIZED_KEY_INDEX = 1;
        private const int ELEMENT_INDEX = 6;
        private const int MP_INDEX = 8;
        private const int EFFECT_TRIGGER_TIMING_INDEX = 9;
        private const int SKILL_TYPE_INDEX = 10;
        private const int CATEGORY_TYPE_INDEX = 11;
        private const int MAIN_EFFECT_TYPE_INDEX = 12;
        private const int MAIN_EFFECT_TARGET_PARAMETER_INDEX = 13;
        private const int TARGET_TYPE_INDEX = 15;
        private const int CONTINUOUS_TURNS_INDEX = 16;
        private const int VALUE_TYPE_INDEX = 17;
        private const int BASE_POWER_INDEX = 18;
        private const int POWER_UPPER_LIMIT_INDEX = 19;
        private const int POWER_LOWER_LIMIT_INDEX = 20;
        private const int SKILL_POWER_THRESHOLD_INDEX = 21;
        private const int POWER_VALUE_ADDED_INDEX = 22;
        private const int POWER_VALUE_REDUCED_INDEX = 23;
        private const int SUCCESS_RATE_INDEX = 36;
        private const int SCENARIO_INDEX = 37;
        private const int VFX_INDEX = 38;
        private const int SUB_EFFECT_MAIN_EFFECT_TYPE_INDEX = 24;
        private const int SUB_EFFECT_MAIN_EFFECT_TARGET_PARAMETER_INDEX = 25;
        private const int SUB_EFFECT_MAIN_TARGET_TYPE_INDEX = 26;
        private const int SUB_EFFECT_CONTINUOUS_TURNS_INDEX = 27;
        private const int SUB_EFFECT_BASE_POWER_INDEX = 28;

        #endregion

        private AbilityAssetMappingEditor _mappingEditor;

        public PassiveAbilitySOEditor()
        {
            CreateDataFolder = false;
            DefaultStoragePath = "Assets/ScriptableObjects/Character/Skills/Passive";
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] allLines = File.ReadAllLines(directory);
            LoadMappings();
            for (int index = ROW_OFFSET; index < allLines.Length; index++)
            {
                // get data form tsv file
                string[] splitedData = allLines[index].Split('\t');
                if (string.IsNullOrEmpty(splitedData[0])) continue;
                string name = DEFAULT_NAME + splitedData[0];
                string path = DefaultStoragePath + "/" + name + ".asset";

                AbilityDataStruct dataModel = new();

                dataModel.Id = splitedData[ID_INDEX];
                dataModel.LocalizedKey = splitedData[LOCALIZED_KEY_INDEX];
                dataModel.ElementId = splitedData[ELEMENT_INDEX];
                dataModel.Mp = string.IsNullOrEmpty(splitedData[MP_INDEX])
                    ? 0
                    : int.Parse(splitedData[MP_INDEX]);
                dataModel.EffectTriggerTimingId = splitedData[EFFECT_TRIGGER_TIMING_INDEX];
                dataModel.SkillTypeId = splitedData[SKILL_TYPE_INDEX];
                dataModel.CategoryTypeId = splitedData[CATEGORY_TYPE_INDEX];
                dataModel.MainEffectTypeId = splitedData[MAIN_EFFECT_TYPE_INDEX];
                dataModel.MainEffectTargetParameterId = splitedData[MAIN_EFFECT_TARGET_PARAMETER_INDEX];
                dataModel.TargetTypeId = splitedData[TARGET_TYPE_INDEX];
                dataModel.ContinuousTurns = int.Parse(splitedData[CONTINUOUS_TURNS_INDEX]);
                dataModel.ValueType = splitedData[VALUE_TYPE_INDEX];
                dataModel.BasePower = string.IsNullOrEmpty(splitedData[BASE_POWER_INDEX])
                    ? 0
                    : float.Parse(splitedData[BASE_POWER_INDEX]);
                dataModel.PowerUpperLimit = string.IsNullOrEmpty(splitedData[BASE_POWER_INDEX])
                    ? 0
                    : float.Parse(splitedData[POWER_UPPER_LIMIT_INDEX]);
                dataModel.PowerLowerLimit = string.IsNullOrEmpty(splitedData[POWER_LOWER_LIMIT_INDEX])
                    ? 0
                    : float.Parse(splitedData[POWER_LOWER_LIMIT_INDEX]);
                dataModel.SkillPowerThreshold = string.IsNullOrEmpty(splitedData[SKILL_POWER_THRESHOLD_INDEX])
                    ? 0
                    : float.Parse(splitedData[SKILL_POWER_THRESHOLD_INDEX]);
                dataModel.PowerValueAdded = string.IsNullOrEmpty(splitedData[POWER_VALUE_ADDED_INDEX])
                    ? 0
                    : float.Parse(splitedData[POWER_VALUE_ADDED_INDEX]);
                dataModel.PowerValueReduced = string.IsNullOrEmpty(splitedData[POWER_VALUE_REDUCED_INDEX])
                    ? 0
                    : float.Parse(splitedData[POWER_VALUE_REDUCED_INDEX]);
                dataModel.SuccessRate = string.IsNullOrEmpty(splitedData[SUCCESS_RATE_INDEX])
                    ? 0
                    : float.Parse(splitedData[SUCCESS_RATE_INDEX]);
                dataModel.ScenarioId = splitedData[SCENARIO_INDEX];
                dataModel.VfxId = splitedData[VFX_INDEX];

                if (!string.IsNullOrEmpty(splitedData[SUB_EFFECT_MAIN_EFFECT_TYPE_INDEX]))
                {
                    SubEffectDataStruct subData = new()
                    {
                        EffectTypeId = splitedData[SUB_EFFECT_MAIN_EFFECT_TYPE_INDEX],
                        EffectTargetParameterId = splitedData[SUB_EFFECT_MAIN_EFFECT_TARGET_PARAMETER_INDEX],
                        TargetTypeId = splitedData[SUB_EFFECT_MAIN_TARGET_TYPE_INDEX],
                        ContinuousTurns = int.Parse(splitedData[SUB_EFFECT_CONTINUOUS_TURNS_INDEX]),
                        BasePower = float.Parse(splitedData[SUB_EFFECT_BASE_POWER_INDEX]),
                    };
                    dataModel.SubEffectData = subData;
                    if (subData.EffectTypeId == "99") continue;
                }

                if (dataModel.MainEffectTypeId == "99" || dataModel.MainEffectTypeId == "4") continue;


                PassiveAbility instance = null;
                instance = (PassiveAbility)AssetDatabase.LoadAssetAtPath(path,
                    typeof(PassiveAbility));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = CreateInstance(dataModel.MainEffectTypeId);
                }

                if (instance == null) continue;
                SetInstanceProperty(instance, dataModel);
                instance.name = name;

                if (!AssetDatabase.Contains(instance))
                {
                    AssetDatabase.CreateAsset(instance, path);
                    AssetDatabase.SaveAssets();
                    callback(instance);
                }
                else
                {
                    EditorUtility.SetDirty(instance);
                }
            }
        }

        private void SetEffects(PassiveAbility instance, AbilityDataStruct data)
        {
            var serializedObject = new SerializedObject(instance);
            var property = serializedObject.FindProperty("_effect");
            var mainEffect = GetEffect(data.MainEffectTargetParameterId);
            if (property == null || mainEffect == null) return;
            property.objectReferenceValue = mainEffect;
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private PassiveAbility CreateInstance(string effectTypeId)
        {
            return effectTypeId == "2"
                ? ScriptableObject.CreateInstance<PassiveWithEffectAbility>()
                : ScriptableObject.CreateInstance<PassiveAbility>();
        }

        private PostNormalAttackPassiveBase GetInstance(ConditionalTargetMap targetMap, string targetTypeId)
        {
            foreach (var map in targetMap.ConditionalTypeMaps)
            {
                if (map.Ids.Contains(targetTypeId))
                {
                    Debug.Log(map.Value);
                    return map.Value;
                }
            }


            return null;
        }

        private void SetInstanceProperty(PassiveAbility instance,
            AbilityDataStruct data)
        {
            SkillInfo skillInfo = new SkillInfo();
            skillInfo.Id = int.Parse(data.Id);
            skillInfo.SkillParameters = new SkillParameters();
            skillInfo.SkillType = ESkillType.Conditional;
            skillInfo.SkillParameters.Element = GetElement(data.ElementId);
            skillInfo.SkillParameters.BasePower = data.BasePower;
            skillInfo.SkillParameters.PowerUpperLimit = data.PowerUpperLimit;
            skillInfo.SkillParameters.PowerLowerLimit = data.PowerLowerLimit;
            skillInfo.SkillParameters.SkillPowerThreshold = data.SkillPowerThreshold;
            skillInfo.SkillParameters.PowerValueAdded = data.PowerValueAdded;
            skillInfo.SkillParameters.PowerValueReduced = data.PowerValueReduced;
            skillInfo.SkillParameters.ContinuesTurn = data.ContinuousTurns;
            skillInfo.SkillParameters.EffectType = GetEffectType(data.MainEffectTypeId);
            skillInfo.SkillParameters.IsFixed = data.ValueType == "1" ? true : false;
            skillInfo.Cost = data.Mp;
            skillInfo.UsageScenarioSO = data.ScenarioId == "1"
                ? EAbilityUsageScenario.Field
                : data.ScenarioId == "2"
                    ? EAbilityUsageScenario.Battle
                    : EAbilityUsageScenario.Field | EAbilityUsageScenario.Battle;

            CustomExecutionAttributeCaptureDef attributeCaptureDef = new();
            attributeCaptureDef.Attribute = GetAttribute(data.MainEffectTargetParameterId);
            skillInfo.SkillParameters.TargetAttribute = attributeCaptureDef;
            GameplayEffectContext ctx = new GameplayEffectContext(skillInfo);
            SetEffects(instance, data);

            var serializedObject = new SerializedObject(instance);
            var property = serializedObject.FindProperty("_context");
            property.boxedValue = ctx;
            SetImmuneTags(serializedObject, data);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private void SetImmuneTags(SerializedObject serializedObject, AbilityDataStruct data)
        {
            var property = serializedObject.FindProperty("tags.ActivationTags");
            var immuneTags = GetImmuneTags(data);
            property.ClearArray();
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            for (var i = 0; i < immuneTags.Length; i++)
            {
                property.InsertArrayElementAtIndex(i);
                property.GetArrayElementAtIndex(i).objectReferenceValue = immuneTags[i];
            }
        }

        private TagScriptableObject[] GetImmuneTags(AbilityDataStruct dataStruct)
        {
            var gameEffect = _mappingEditor.GameEffectMaps.Find(x => x.Id == dataStruct.MainEffectTargetParameterId);
            if (gameEffect == null) return Array.Empty<TagScriptableObject>();
            return gameEffect.Value.ApplicationTagRequirements.IgnoreTags;
        }

        private Elemental GetElement(string id)
        {
            foreach (var map in _mappingEditor.ElementMaps)
            {
                if (map.Id == id)
                {
                    return map.Value;
                }
            }

            return null;
        }

        private GameplayEffectDefinition GetEffect(string targetParam)
        {
            foreach (var buffEffect in _mappingEditor.BuffEffects)
            {
                if (buffEffect.Id == targetParam)
                    return buffEffect.Value;
            }

            return null;
        }

        private AttributeScriptableObject GetAttribute(string id)
        {
            foreach (var map in _mappingEditor.TargetParameterMaps)
            {
                if (map.Id == id)
                {
                    return map.Value;
                }
            }

            return null;
        }

        private EEffectType GetEffectType(string id)
        {
            foreach (var map in _mappingEditor.EffectTypeMaps)
            {
                if (map.Id == id)
                {
                    return map.Value;
                }
            }

            return default;
        }

        private void LoadMappings()
        {
            var guid = AssetDatabase.FindAssets("t:AbilityAssetMappingEditor");
            _mappingEditor =
                AssetDatabase.LoadAssetAtPath<AbilityAssetMappingEditor>(AssetDatabase.GUIDToAssetPath(guid[0]));
        }
    }
}