// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.Collections.Generic;
using GameFrameX.Editor;
using GameFrameX.Payment.Runtime;
using GameFrameX.Runtime;
using UnityEditor;
using UnityEngine;

namespace GameFrameX.Payment.Editor
{
    /// <summary>
    /// 支付组件 Inspector
    /// </summary>
    [CustomEditor(typeof(PaymentComponent))]
    public sealed class PaymentComponentInspector : ComponentTypeComponentInspector
    {
        private GUIContent m_componentAndroidTypeGUIContent = new GUIContent("Android平台支付组件");
        private GUIContent m_componentIOSTypeGUIContent = new GUIContent("iOS平台支付组件");

        private SerializedProperty ComponentAndroidType = null;
        private string[] ComponentAndroidTypeNames = null;
        private int ComponentAndroidTypeNameIndex = 0;

        private SerializedProperty ComponentIOSType = null;
        private string[] ComponentIOSTypeNames = null;
        private int ComponentIOSTypeNameIndex = 0;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode & Application.isPlaying);
            {
                EditorGUILayout.LabelField(m_componentAndroidTypeGUIContent);
                int componentAndroidTypeNameIndex = EditorGUILayout.Popup("Component Android Type", ComponentAndroidTypeNameIndex, ComponentAndroidTypeNames);
                if (componentAndroidTypeNameIndex != ComponentAndroidTypeNameIndex)
                {
                    ComponentAndroidTypeNameIndex = componentAndroidTypeNameIndex;
                    ComponentAndroidType.stringValue = componentAndroidTypeNameIndex <= 0 ? null : ComponentAndroidTypeNames[componentAndroidTypeNameIndex];
                }

                EditorGUILayout.LabelField(m_componentIOSTypeGUIContent);
                int componentIOSTypeNameIndex = EditorGUILayout.Popup("Component IOS Type", ComponentIOSTypeNameIndex, ComponentIOSTypeNames);
                if (componentIOSTypeNameIndex != ComponentIOSTypeNameIndex)
                {
                    ComponentIOSTypeNameIndex = componentIOSTypeNameIndex;
                    ComponentIOSType.stringValue = componentIOSTypeNameIndex <= 0 ? null : ComponentIOSTypeNames[componentIOSTypeNameIndex];
                }
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void Enable()
        {
            base.Enable();
            ComponentAndroidType = serializedObject.FindProperty("m_componentAndroidType");
            ComponentIOSType = serializedObject.FindProperty("m_componentIOSType");
            RefreshComponentAndroidTypeNames(typeof(IPaymentManager));
            RefreshComponentIOSTypeNames(typeof(IPaymentManager));
        }

        protected override void RefreshTypeNames()
        {
            RefreshComponentTypeNames(typeof(IPaymentManager));
        }

        private void RefreshComponentAndroidTypeNames(System.Type type)
        {
            List<string> managerTypeNames = new List<string>
            {
                NoneOptionName
            };

            managerTypeNames.AddRange(Utility.Assembly.GetRuntimeTypeNames(type));
            ComponentAndroidTypeNames = managerTypeNames.ToArray();
            ComponentAndroidTypeNameIndex = 0;
            if (!ComponentAndroidType.stringValue.IsNullOrEmpty())
            {
                ComponentAndroidTypeNameIndex = managerTypeNames.IndexOf(ComponentAndroidType.stringValue);
                if (ComponentAndroidTypeNameIndex <= 0)
                {
                    ComponentAndroidTypeNameIndex = 0;
                    ComponentAndroidType.stringValue = null;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void RefreshComponentIOSTypeNames(System.Type type)
        {
            List<string> managerTypeNames = new List<string>
            {
                NoneOptionName
            };

            managerTypeNames.AddRange(Utility.Assembly.GetRuntimeTypeNames(type));
            ComponentIOSTypeNames = managerTypeNames.ToArray();
            ComponentIOSTypeNameIndex = 0;
            if (!ComponentIOSType.stringValue.IsNullOrEmpty())
            {
                ComponentIOSTypeNameIndex = managerTypeNames.IndexOf(ComponentIOSType.stringValue);
                if (ComponentIOSTypeNameIndex <= 0)
                {
                    ComponentIOSTypeNameIndex = 0;
                    ComponentIOSType.stringValue = null;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}