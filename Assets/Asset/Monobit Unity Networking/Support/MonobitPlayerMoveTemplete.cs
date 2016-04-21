using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR
using MonobitEngine;

namespace Monobit.Support
{
    [Serializable]
    [RequireComponent(typeof(MonobitView))]
    [RequireComponent(typeof(MonobitTransformView))]
    [RequireComponent(typeof(MonobitAnimatorView))]
    [AddComponentMenu("Monobit Networking Support/Monobit Player Move Templete &v")]
    public class MonobitPlayerMoveTemplete : MonobitEngine.MonoBehaviour
    {
        /** MonobitAnimatorView 本体. */
        [SerializeField]
        public MonobitAnimatorView animView = null;

        public enum KeyCode
        {
            Everytime,
            Horizontal,
            Vertical,
            Fire1,
            Fire2,
            Fire3,
            Jump,
        };

        public enum AxisAction
        {
            Positive,
            Negative,
            Zero,
        };

        public enum ButtonAction
        {
            Press,
            Up,
            Down,
        };

        public enum ActionType
        {
            Move,
            Rotate,
            ChangeAnimLayerWeight,
            ChangeAnimParam,
            Instantiate,
        }

        public enum InstantiateType
        {
            Absolute,
            Relative,
            RandomAbsolute,
        }

        [Serializable]
        public class AnimLayerInfo
        {
            [SerializeField]
            public int m_Index;         /**< アニメーションレイヤーのインデックス. */
            [SerializeField]
            public string m_Name;       /**< アニメーションレイヤー名. */
            [SerializeField]
            public float m_animWeight;  /**< アニメーションウェイト値. */
        };

        [Serializable]
        public class AnimParamInfo
        {
            [SerializeField]
            public int m_Index;                             /**< アニメーションレイヤーのインデックス. */
            [SerializeField]
            public AnimatorControllerParameterType m_Type;  /**< 同期するアニメーションパラメータの型. */
            [SerializeField]
            public string m_Name;                           /**< アニメーションパラメータ名. */
            [SerializeField]
            public float m_floatValue;                      /**< アニメーションパラメータ値（float）. */
            [SerializeField]
            public int m_intValue;                          /**< アニメーションパラメータ値（Int）. */
            [SerializeField]
            public bool m_boolValue;                        /**< アニメーションパラメータ値（bool）. */
        }

        [Serializable]
        public class MonobitKeySettings
        {
            [SerializeField]
            public KeyCode keyCode;
            [SerializeField]
            public AxisAction axisAction;
            [SerializeField]
            public ButtonAction buttonAction;
            [SerializeField]
            public ActionType actionType;
            [SerializeField]
            public Vector3 Position;
            [SerializeField]
            public Vector3 Rotation;
            [SerializeField]
            public int SelectLayer;
            [SerializeField]
            public List<AnimLayerInfo> layerInfo = new List<AnimLayerInfo>();
            [SerializeField]
            public int SelectParam;
            [SerializeField]
            public List<AnimParamInfo> paramInfo = new List<AnimParamInfo>();
            [SerializeField]
            public GameObject instantiatePrefab = null;
            [SerializeField]
            public InstantiateType instantiateType;
            [SerializeField]
            public Vector3 PositionMin;
            [SerializeField]
            public Vector3 PositionMax;
            [SerializeField]
            public Vector3 RotationMin;
            [SerializeField]
            public Vector3 RotationMax;
        };

        [SerializeField]
        public List<MonobitKeySettings> KeyAndAnimSettings = new List<MonobitKeySettings>();

        void Update()
        {
            if( animView == null )
            {
                animView = gameObject.GetComponent<MonobitAnimatorView>();
            }
            if (!monobitView.isMine)
            {
                return;
            }

            foreach (var settings in KeyAndAnimSettings)
            {
                DoAction(settings, GetKeyActiveValue(settings));
            }
        }

        void DoAction(MonobitKeySettings settings, float keyActiveValue)
        {
            switch (settings.actionType)
            {
                case ActionType.Move:
                    gameObject.transform.position += gameObject.transform.right * settings.Position.x * keyActiveValue;
                    gameObject.transform.position += gameObject.transform.up * settings.Position.y * keyActiveValue;
                    gameObject.transform.position += gameObject.transform.forward * settings.Position.z * keyActiveValue;
                    break;
                case ActionType.Rotate:
                    gameObject.transform.Rotate(settings.Rotation * keyActiveValue);
                    break;
                case ActionType.ChangeAnimLayerWeight:
                    if (animView != null && animView.m_Animator != null)
                    {
                        AnimLayerInfo info = settings.layerInfo[settings.SelectLayer];
                        animView.m_Animator.SetLayerWeight(info.m_Index, info.m_animWeight * keyActiveValue);
                    }
                    break;
                case ActionType.ChangeAnimParam:
                    if (keyActiveValue > 0.0f)
                    {
                        if (animView != null && animView.m_Animator != null)
                        {
                            AnimParamInfo info = settings.paramInfo[settings.SelectParam];
                            switch (info.m_Type)
                            {
                                case AnimatorControllerParameterType.Bool:
                                    animView.m_Animator.SetBool(info.m_Name, info.m_boolValue);
                                    break;
                                case AnimatorControllerParameterType.Float:
                                    animView.m_Animator.SetFloat(info.m_Name, info.m_floatValue * keyActiveValue);
                                    break;
                                case AnimatorControllerParameterType.Int:
                                    animView.m_Animator.SetInteger(info.m_Name, (int)(info.m_intValue * keyActiveValue));
                                    break;
                                case AnimatorControllerParameterType.Trigger:
                                    break;
                            }
                        }
                    }
                    break;
                case ActionType.Instantiate:
                    if (keyActiveValue >= 1.0f)
                    {
                        switch (settings.instantiateType)
                        {
                            case InstantiateType.Absolute:
                                {
                                    Quaternion rotation = Quaternion.Euler(settings.Rotation);
                                    MonobitNetwork.Instantiate(settings.instantiatePrefab.name, settings.Position, rotation, 0);
                                }
                                break;
                            case InstantiateType.Relative:
                                {
                                    Vector3 position = gameObject.transform.position;
                                    position += gameObject.transform.right * settings.Position.x;
                                    position += gameObject.transform.up * settings.Position.y;
                                    position += gameObject.transform.forward * settings.Position.z;
                                    Quaternion rotation = gameObject.transform.rotation * Quaternion.Euler(settings.Rotation);
                                    MonobitNetwork.Instantiate(settings.instantiatePrefab.name, position, rotation, 0);
                                }
                                break;
                            case InstantiateType.RandomAbsolute:
                                {
                                    Vector3 position = new Vector3(UnityEngine.Random.Range(settings.PositionMin.x, settings.PositionMax.x),
                                                                   UnityEngine.Random.Range(settings.PositionMin.y, settings.PositionMax.y),
                                                                   UnityEngine.Random.Range(settings.PositionMin.z, settings.PositionMax.z));
                                    Quaternion rotation = Quaternion.Euler(UnityEngine.Random.Range(settings.RotationMin.x, settings.RotationMax.x),
                                                                           UnityEngine.Random.Range(settings.RotationMin.y, settings.RotationMax.y),
                                                                           UnityEngine.Random.Range(settings.RotationMin.z, settings.RotationMax.z));
                                    MonobitNetwork.Instantiate(settings.instantiatePrefab.name, position, rotation, 0);
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        private float GetKeyActiveValue(MonobitKeySettings settings)
        {
            if (settings.keyCode == KeyCode.Horizontal && settings.axisAction == AxisAction.Positive)
            {
                if (Input.GetAxis("Horizontal") > 0.0f)
                {
                    return Math.Abs(Input.GetAxis("Horizontal"));
                }
                return 0.0f;
            }
            else if (settings.keyCode == KeyCode.Horizontal && settings.axisAction == AxisAction.Negative)
            {
                if (Input.GetAxis("Horizontal") < 0.0f)
                {
                    return Math.Abs(Input.GetAxis("Horizontal"));
                }
                return 0.0f;
            }
            else if (settings.keyCode == KeyCode.Horizontal && settings.axisAction == AxisAction.Zero)
            {
                if (Input.GetAxis("Horizontal") == 0.0f)
                {
                    return 1.0f;
                }
                return 0.0f;
            }
            else if (settings.keyCode == KeyCode.Vertical && settings.axisAction == AxisAction.Positive)
            {
                if (Input.GetAxis("Vertical") > 0.0f)
                {
                    return Math.Abs(Input.GetAxis("Vertical"));
                }
                return 0.0f;
            }
            else if (settings.keyCode == KeyCode.Vertical && settings.axisAction == AxisAction.Negative)
            {
                if (Input.GetAxis("Vertical") < 0.0f)
                {
                    return Math.Abs(Input.GetAxis("Vertical"));
                }
                return 0.0f;
            }
            else if (settings.keyCode == KeyCode.Vertical && settings.axisAction == AxisAction.Zero)
            {
                if (Input.GetAxis("Vertical") == 0.0f)
                {
                    return 1.0f;
                }
                return 0.0f;
            }
            else if (settings.keyCode == KeyCode.Fire1)
            {
                return IsKeyAction("Fire1", settings.buttonAction) ? 1.0f : 0.0f;
            }
            else if (settings.keyCode == KeyCode.Fire2)
            {
                return IsKeyAction("Fire2", settings.buttonAction) ? 1.0f : 0.0f;
            }
            else if (settings.keyCode == KeyCode.Fire3)
            {
                return IsKeyAction("Fire3", settings.buttonAction) ? 1.0f : 0.0f;
            }
            else if (settings.keyCode == KeyCode.Jump)
            {
                return IsKeyAction("Jump", settings.buttonAction) ? 1.0f : 0.0f;
            }
            else if (settings.keyCode == KeyCode.Everytime)
            {
                return 1.0f;
            }
            return 0.0f;
        }

        bool IsKeyAction(string buttonName, ButtonAction action)
        {
            switch (action)
            {
                case ButtonAction.Press:
                    return Input.GetButton(buttonName);
                case ButtonAction.Down:
                    return Input.GetButtonDown(buttonName);
                case ButtonAction.Up:
                    return Input.GetButtonUp(buttonName);
            }
            return false;
        }
    }

#if UNITY_EDITOR
    /**
     * MonobitAutoLoginComponent のInspector表示用クラス.
     */
    [CustomEditor(typeof(MonobitPlayerMoveTemplete))]
    public class MonobitPlayerMoveTempleteInspector : Editor
    {
        /** MonobitAutoLoginComponent 本体. */
        MonobitPlayerMoveTemplete obj;

        /** MonobitView 本体. */
        MonobitView view;

        /** MonobitAnimatorView 本体. */
        MonobitAnimatorView animView;

        /**
         *
         */
        void AddMonobitObserverdComponent()
        {
            if( view == null )
            {
                view = obj.gameObject.GetComponent<MonobitView>();
            }
            if ( view != null )
            {
                if (view.InternalObservedComponents == null)
                {
                    view.InternalObservedComponents = new List<Component>();
                }
                else
                {
                    if ( view.InternalObservedComponents.FindAll(item => item != null && item.GetType() == typeof(MonobitTransformView)).Count == 0 )
                    {
                        view.InternalObservedComponents.Add(obj.gameObject.GetComponent<MonobitTransformView>());
                    }
                    if (view.InternalObservedComponents.FindAll(item => item != null && item.GetType() == typeof(MonobitAnimatorView)).Count == 0)
                    {
                        view.InternalObservedComponents.Add(obj.gameObject.GetComponent<MonobitAnimatorView>());
                    }
                }
            }
        }
        
        /**
         * Inspector上のGUI表示.
         */
        public override void OnInspectorGUI()
        {
            // 本体の取得
            obj = target as MonobitPlayerMoveTemplete;
            if (!EditorApplication.isPlaying)
            {
                // MonobitObservedComponentにMonobitTransformViewとMonobitAnimatorViewを自動追加
                AddMonobitObserverdComponent();

                // キー操作とアニメーションパラメータの登録
                EntryKeyAndAnimation();
            }

            GUILayout.Space(5);

            // プロパティの取得
            //SerializedProperty property = serializedObject.FindProperty("KeyAndAnimSettings");

            // 標題と追加の表示
            EditorGUILayout.LabelField("Key And Anim Settings List", EditorStyles.boldLabel);

            GUI.enabled = !EditorApplication.isPlaying;
            EditorGUI.indentLevel = 2;

            // 各リスト項目と削除ボタンの表示
            for (int i = 0; i < obj.KeyAndAnimSettings.Count; ++i)
            {
                obj.KeyAndAnimSettings[i].keyCode = (MonobitPlayerMoveTemplete.KeyCode)EditorGUILayout.EnumPopup("Key Assign", obj.KeyAndAnimSettings[i].keyCode);
                if (obj.KeyAndAnimSettings[i].keyCode == MonobitPlayerMoveTemplete.KeyCode.Horizontal || obj.KeyAndAnimSettings[i].keyCode == MonobitPlayerMoveTemplete.KeyCode.Vertical)
                {
                    obj.KeyAndAnimSettings[i].axisAction = (MonobitPlayerMoveTemplete.AxisAction)EditorGUILayout.EnumPopup("Axis Action", obj.KeyAndAnimSettings[i].axisAction);
                }
                else
                {
                    obj.KeyAndAnimSettings[i].buttonAction = (MonobitPlayerMoveTemplete.ButtonAction)EditorGUILayout.EnumPopup("Button Action", obj.KeyAndAnimSettings[i].buttonAction);
                }

                obj.KeyAndAnimSettings[i].actionType = (MonobitPlayerMoveTemplete.ActionType)EditorGUILayout.EnumPopup("Action Type", obj.KeyAndAnimSettings[i].actionType);
                switch (obj.KeyAndAnimSettings[i].actionType)
                {
                    case MonobitPlayerMoveTemplete.ActionType.Move:
                        {
                            obj.KeyAndAnimSettings[i].Position = EditorGUILayout.Vector3Field("Position Increase", obj.KeyAndAnimSettings[i].Position);
                        }
                        break;
                    case MonobitPlayerMoveTemplete.ActionType.Rotate:
                        {
                            obj.KeyAndAnimSettings[i].Rotation = EditorGUILayout.Vector3Field("Rotation Increase", obj.KeyAndAnimSettings[i].Rotation);
                        }
                        break;
                    case MonobitPlayerMoveTemplete.ActionType.ChangeAnimLayerWeight:
                        {
                            List<string> name = new List<string>();
                            foreach (var layer in obj.KeyAndAnimSettings[i].layerInfo)
                            {
                                name.Add(layer.m_Name);
                            }
                            obj.KeyAndAnimSettings[i].SelectLayer = EditorGUILayout.Popup("Select Anim Layer", obj.KeyAndAnimSettings[i].SelectLayer, name.ToArray());
                            var selected = obj.KeyAndAnimSettings[i].layerInfo[obj.KeyAndAnimSettings[i].SelectLayer];
                            selected.m_animWeight = EditorGUILayout.FloatField("Anim Weight[" + selected.m_Name + "]", selected.m_animWeight);
                        }
                        break;
                    case MonobitPlayerMoveTemplete.ActionType.ChangeAnimParam:
                        {
                            List<string> name = new List<string>();
                            foreach (var param in obj.KeyAndAnimSettings[i].paramInfo)
                            {
                                name.Add(param.m_Name);
                            }
                            obj.KeyAndAnimSettings[i].SelectParam = EditorGUILayout.Popup("Select Anim Param", obj.KeyAndAnimSettings[i].SelectParam, name.ToArray());

                            var selected = obj.KeyAndAnimSettings[i].paramInfo[obj.KeyAndAnimSettings[i].SelectParam];
                            switch (selected.m_Type)
                            {
                                case AnimatorControllerParameterType.Bool:
                                    selected.m_boolValue = EditorGUILayout.Toggle("Anim Flag[" + selected.m_Name + "]", selected.m_boolValue);
                                    break;
                                case AnimatorControllerParameterType.Float:
                                    selected.m_floatValue = EditorGUILayout.FloatField("Anim Value[" + selected.m_Name + "]", selected.m_floatValue);
                                    break;
                                case AnimatorControllerParameterType.Int:
                                    selected.m_intValue = EditorGUILayout.IntField("Anim Value[" + selected.m_Name + "]", selected.m_intValue);
                                    break;
                                case AnimatorControllerParameterType.Trigger:
                                    break;
                            }
                        }
                        break;
                    case MonobitPlayerMoveTemplete.ActionType.Instantiate:
                        {
                            obj.KeyAndAnimSettings[i].instantiatePrefab = EditorGUILayout.ObjectField("Prefab", obj.KeyAndAnimSettings[i].instantiatePrefab, typeof(GameObject), false) as GameObject;

                            // 登録したプレハブが Resources 内に存在するかどうかを調べる
                            if (obj.KeyAndAnimSettings[i].instantiatePrefab != null)
                            {
                                GameObject tmp = Resources.Load(obj.KeyAndAnimSettings[i].instantiatePrefab.name, typeof(GameObject)) as GameObject;
                                if (tmp == null)
                                {
                                    EditorGUILayout.HelpBox("This Prefab is not included in the 'Resources' folder .", MessageType.Warning, true);
                                }
                            }

                            obj.KeyAndAnimSettings[i].instantiateType = (MonobitPlayerMoveTemplete.InstantiateType)EditorGUILayout.EnumPopup("Instantiate Type", obj.KeyAndAnimSettings[i].instantiateType);
                            switch (obj.KeyAndAnimSettings[i].instantiateType)
                            {
                                case MonobitPlayerMoveTemplete.InstantiateType.Absolute:
                                    {
                                        obj.KeyAndAnimSettings[i].Position = EditorGUILayout.Vector3Field("Absolute Position", obj.KeyAndAnimSettings[i].Position);
                                        obj.KeyAndAnimSettings[i].Rotation = EditorGUILayout.Vector3Field("Absolute Rotation", obj.KeyAndAnimSettings[i].Rotation);
                                    }
                                    break;
                                case MonobitPlayerMoveTemplete.InstantiateType.Relative:
                                    {
                                        obj.KeyAndAnimSettings[i].Position = EditorGUILayout.Vector3Field("Relative Position", obj.KeyAndAnimSettings[i].Position);
                                        obj.KeyAndAnimSettings[i].Rotation = EditorGUILayout.Vector3Field("Relative Rotation", obj.KeyAndAnimSettings[i].Rotation);
                                    }
                                    break;
                                case MonobitPlayerMoveTemplete.InstantiateType.RandomAbsolute:
                                    {
                                        obj.KeyAndAnimSettings[i].PositionMin = EditorGUILayout.Vector3Field("Min Position", obj.KeyAndAnimSettings[i].PositionMin);
                                        obj.KeyAndAnimSettings[i].PositionMax = EditorGUILayout.Vector3Field("Max Position", obj.KeyAndAnimSettings[i].PositionMax);
                                        obj.KeyAndAnimSettings[i].RotationMin = EditorGUILayout.Vector3Field("Min Rotation", obj.KeyAndAnimSettings[i].RotationMin);
                                        obj.KeyAndAnimSettings[i].RotationMax = EditorGUILayout.Vector3Field("Max Rotation", obj.KeyAndAnimSettings[i].RotationMax);
                                    }
                                    break;
                            }
                        }
                        break;
                }

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Remove", GUILayout.Width(75.0f)))
                {
                    obj.KeyAndAnimSettings.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
            }

            // 追加ボタンの表示
            GUILayout.BeginHorizontal();
            GUILayout.Space(30);
            if (GUILayout.Button("Add Key And Anim Settings List Column"))
            {
                obj.KeyAndAnimSettings.Add(new MonobitPlayerMoveTemplete.MonobitKeySettings());
            }
            GUILayout.EndHorizontal();

            GUI.enabled = true;

            EditorGUI.indentLevel = 0;
            GUILayout.Space(5);

            // データの更新
            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(obj);
            }
        }

        private void EntryKeyAndAnimation()
        {
            if (animView == null)
            {
                animView = obj.gameObject.GetComponent<MonobitAnimatorView>();
            }
            if (animView != null)
            {
                List<MonobitAnimatorView.AnimLayerInfo> animLayer = animView.SyncAnimLayers;
                List<MonobitAnimatorView.AnimParamInfo> animParam = animView.SyncAnimParams;

	            foreach (var settings in obj.KeyAndAnimSettings)
	            {
	                for (int i = settings.layerInfo.Count - 1; i >= 0; --i)
	                {
	                    bool bFound = false;
	                    foreach (var layer in animLayer)
	                    {
	                        if (layer.m_Name == settings.layerInfo[i].m_Name)
	                        {
	                            bFound = true;
	                            break;
	                        }
	                    }
	                    if (bFound == false)
	                    {
	                        settings.layerInfo.RemoveAt(i);
	                    }
	                }
	                for (int i = settings.paramInfo.Count - 1; i >= 0; --i)
	                {
	                    bool bFound = false;
	                    foreach (var param in animParam)
	                    {
	                        if (param.m_Name == settings.paramInfo[i].m_Name)
	                        {
	                            bFound = true;
	                            break;
	                        }
	                    }
	                    if (bFound == false)
	                    {
	                        settings.paramInfo.RemoveAt(i);
	                    }
	                }
	            }
	
	            foreach (var settings in obj.KeyAndAnimSettings)
	            {
	                foreach (var layer in animLayer)
	                {
	                    bool bFound = false;
	                    foreach (var layerInfo in settings.layerInfo)
	                    {
	                        if (layer.m_Name == layerInfo.m_Name)
	                        {
	                            bFound = true;
	                            break;
	                        }
	                    }
	                    if (bFound == false)
	                    {
	                        settings.layerInfo.Add(new MonobitPlayerMoveTemplete.AnimLayerInfo
                            {
                                m_Index = layer.m_Index,
                                m_Name = layer.m_Name,
                                m_animWeight = (animView.m_Animator.isActiveAndEnabled) ? animView.m_Animator.GetLayerWeight(layer.m_Index) : 0.0f
                            });
	                    }
	                }
	                foreach (var param in animParam)
	                {
	                    bool bFound = false;
	                    foreach (var paramInfo in settings.paramInfo)
	                    {
	                        if (param.m_Name == paramInfo.m_Name)
	                        {
	                            bFound = true;
	                            break;
	                        }
	                    }
	                    if (bFound == false)
	                    {
	                        switch (param.m_Type)
	                        {
	                            case AnimatorControllerParameterType.Bool:
	                                settings.paramInfo.Add(new MonobitPlayerMoveTemplete.AnimParamInfo
                                    {
                                        m_Type = param.m_Type,
                                        m_Name = param.m_Name,
                                        m_boolValue = (animView.m_Animator.isActiveAndEnabled) ? animView.m_Animator.GetBool(param.m_Name): false
                                    });
	                                break;
	                            case AnimatorControllerParameterType.Float:
	                                settings.paramInfo.Add(new MonobitPlayerMoveTemplete.AnimParamInfo
                                    {
                                        m_Type = param.m_Type,
                                        m_Name = param.m_Name,
                                        m_floatValue = (animView.m_Animator.isActiveAndEnabled) ? animView.m_Animator.GetFloat(param.m_Name): 0.0f
                                    });
	                                break;
	                            case AnimatorControllerParameterType.Int:
	                                settings.paramInfo.Add(new MonobitPlayerMoveTemplete.AnimParamInfo
                                    {
                                        m_Type = param.m_Type,
                                        m_Name = param.m_Name,
                                        m_intValue = (animView.m_Animator.isActiveAndEnabled) ? animView.m_Animator.GetInteger(param.m_Name): 0
                                    });
	                                break;
	                            case AnimatorControllerParameterType.Trigger:
	                                break;
	                        }
	
	                    }
	                }
	            }

            }
        }
    }
#endif // UNITY_EDITOR
}
