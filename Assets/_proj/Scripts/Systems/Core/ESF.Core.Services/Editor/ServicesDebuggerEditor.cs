#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace ESF.Core.Services.Editor
{
    [CustomEditor(typeof(ServicesDebugger))]
    public class ServicesDebuggerEditor : UnityEditor.Editor
    {
        private bool _sortByTypeName = true;
        private readonly HashSet<object> _drawGuard = new(new ReferenceEqualityComparer());
        private readonly Dictionary<int, bool> _serviceFoldouts = new();
        private readonly Dictionary<int, bool> _objFoldouts = new();

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Services Debugger", EditorStyles.boldLabel);

            object container = Service.ServiceContainer;
            if (container == null)
            {
                EditorGUILayout.HelpBox("Service.s_serviceContainer is null.", MessageType.Info);
                return;
            }

            var mapField = container.GetType().GetField("_servicesMap",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (mapField == null)
            {
                EditorGUILayout.HelpBox("Field '_servicesMap' not found on the container.", MessageType.Warning);
                return;
            }

            var mapObj = mapField.GetValue(container);
            if (mapObj is not IDictionary dict)
            {
                EditorGUILayout.HelpBox("'_servicesMap' is not a Dictionary<Type, object>.", MessageType.Warning);
                return;
            }

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            EditorGUILayout.LabelField($"Registered services: {dict.Count}");
            GUILayout.FlexibleSpace();
            _sortByTypeName = GUILayout.Toggle(_sortByTypeName, "Sort by Type", EditorStyles.miniButton, GUILayout.Width(100));
            if (GUILayout.Button("Refresh", EditorStyles.miniButton, GUILayout.Width(70))) Repaint();
            EditorGUILayout.EndHorizontal();

            // Collect rows
            var rows = new List<(Type keyType, object value)>();
            foreach (DictionaryEntry e in dict)
                rows.Add(((Type)e.Key, e.Value));
            if (_sortByTypeName)
                rows.Sort((a, b) => string.Compare(a.keyType?.FullName, b.keyType?.FullName, StringComparison.Ordinal));

            // Draw rows
            foreach (var (keyType, value) in rows)
                DrawService(keyType, value);
        }

        private void DrawService(Type keyType, object value)
        {
            if (value is not UnityEngine.Object)
            {
                if (value != null && !value.GetType().IsSerializable)
                    return;
            }

            var key = keyType?.Name ?? "(null)";
            var sid = RuntimeHelpers.GetHashCode(value);

            EditorGUILayout.BeginVertical("box");

            EditorGUI.indentLevel++;

            bool open = _serviceFoldouts.TryGetValue(sid, out var o) ? o : false;
            open = EditorGUILayout.Foldout(open, key, true);
            _serviceFoldouts[sid] = open;

            if (!open)
            {
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
                return;
            }

            // UnityEngine.Object? show a handy link
            if (value is UnityEngine.Object uo)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.ObjectField("Unity Object", uo, uo.GetType(), true);
                    if (GUILayout.Button("Ping", GUILayout.Width(60))) EditorGUIUtility.PingObject(uo);
                    if (GUILayout.Button("Select", GUILayout.Width(60))) Selection.activeObject = uo;
                }
            }
            else
            {
                // Managed C# object — draw [Serializable] fields (read-only)
                if (value != null)
                {
                    using (new EditorGUI.DisabledScope(true)) // keep it safe/read-only
                    {
                        _drawGuard.Clear();
                        DrawManagedObject(value, value.GetType(), 0);
                    }
                }
            }

            EditorGUI.indentLevel--;

            EditorGUILayout.EndVertical();
        }

        // Recursively draws public or [SerializeField] instance fields.
        private void DrawManagedObject(object obj, Type t, int depth)
        {
            if (obj == null)
            {
                EditorGUILayout.LabelField("(null)");
                return;
            }

            // guard cycles
            if (!_drawGuard.Add(obj))
            {
                EditorGUILayout.LabelField("(cycle detected)");
                return;
            }

            if (IsLeafType(t))
            {
                DrawLeaf("(value)", obj, t);
                return;
            }

            // Arrays/Lists
            if (obj is IList list)
            {
                for (int i = 0; i < list.Count; i++)
                    DrawChild($"[{i}]", list[i], list[i]?.GetType());
                return;
            }

            // Dictionaries
            if (obj is IDictionary dict)
            {
                foreach (DictionaryEntry de in dict)
                    DrawChild($"[{KeyToString(de.Key)}]", de.Value, de.Value?.GetType());
                return;
            }

            // Complex object: fields only (Unity-like rules)
            var fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var f in fields)
            {
                if (f.IsStatic) continue;
                if (f.IsNotSerialized) continue;

                bool show =
                    f.IsPublic ||
                    Attribute.IsDefined(f, typeof(SerializeField)) ||
                    Attribute.IsDefined(t, typeof(SerializableAttribute)); // be lenient for your use case

                if (!show) continue;

                var val = SafeGet(() => f.GetValue(obj));
                DrawChild(ObjectNames.NicifyVariableName(f.Name), val, f.FieldType);
            }
        }

        private void DrawChild(string label, object value, Type t)
        {
            if (t == null)
            {
                EditorGUILayout.LabelField(label, "(null)");
                return;
            }

            if (IsLeafType(t))
            {
                DrawLeaf(label, value, t);
                return;
            }

            int id = value != null ? RuntimeHelpers.GetHashCode(value) : (label.GetHashCode() ^ t.GetHashCode());
            bool open = _objFoldouts.TryGetValue(id, out var o) ? o : false;
            open = EditorGUILayout.Foldout(open, $"{label}  ({t.Name})", true);
            _objFoldouts[id] = open;
            if (!open) return;

            EditorGUI.indentLevel++;
            DrawManagedObject(value, t, 0);
            EditorGUI.indentLevel--;
        }

        private static void DrawLeaf(string label, object value, Type t)
        {
            if (typeof(UnityEngine.Object).IsAssignableFrom(t))
            {
                EditorGUILayout.ObjectField(label, value as UnityEngine.Object, t, true);
            }
            else if (t.IsEnum)
            {
                EditorGUILayout.LabelField(label, value?.ToString() ?? "(null)");
            }
            else if (t == typeof(string))
            {
                EditorGUILayout.TextField(label, value as string ?? "");
            }
            else if (t == typeof(bool))
            {
                EditorGUILayout.Toggle(label, value is bool b && b);
            }
            else if (t == typeof(int))
            {
                EditorGUILayout.IntField(label, value is int i ? i : 0);
            }
            else if (t == typeof(float))
            {
                EditorGUILayout.FloatField(label, value is float f ? f : 0f);
            }
            else if (t == typeof(double))
            {
                EditorGUILayout.DoubleField(label, value is double d ? d : 0d);
            }
            else if (t == typeof(long))
            {
                long v = value is long l ? l : 0L;
                EditorGUILayout.LongField(label, v);
            }
            else if (t == typeof(Vector2))
            {
                EditorGUILayout.Vector2Field(label, value is Vector2 v ? v : default);
            }
            else if (t == typeof(Vector3))
            {
                EditorGUILayout.Vector3Field(label, value is Vector3 v ? v : default);
            }
            else if (t == typeof(Vector4))
            {
                Vector4 v = value is Vector4 vv ? vv : default;
                EditorGUILayout.Vector4Field(label, v);
            }
            else if (t == typeof(Quaternion))
            {
                var q = value is Quaternion qq ? qq : Quaternion.identity;
                EditorGUILayout.Vector4Field(label + " (x,y,z,w)", new Vector4(q.x, q.y, q.z, q.w));
            }
            else if (t == typeof(Color))
            {
                EditorGUILayout.ColorField(label, value is Color c ? c : default);
            }
            else if (t == typeof(Rect))
            {
                EditorGUILayout.RectField(label, value is Rect r ? r : default);
            }
            else if (t == typeof(Bounds))
            {
                EditorGUILayout.BoundsField(label, value is Bounds b ? b : default);
            }
            else if (t == typeof(AnimationCurve))
            {
                EditorGUILayout.CurveField(label, value as AnimationCurve);
            }
            else
            {
                // Fallback
                EditorGUILayout.LabelField(label, value?.ToString() ?? "(null)");
            }
        }

        private static bool IsLeafType(Type t)
        {
            return t.IsEnum ||
                   t.IsPrimitive ||
                   t == typeof(string) ||
                   typeof(UnityEngine.Object).IsAssignableFrom(t) ||
                   t == typeof(Vector2) || t == typeof(Vector3) || t == typeof(Vector4) ||
                   t == typeof(Quaternion) || t == typeof(Color) ||
                   t == typeof(Rect) || t == typeof(Bounds) ||
                   t == typeof(AnimationCurve) ||
                   t == typeof(float) || t == typeof(double) || t == typeof(decimal) ||
                   t == typeof(int) || t == typeof(long) || t == typeof(short) ||
                   t == typeof(uint) || t == typeof(ulong) || t == typeof(ushort) ||
                   t == typeof(bool) || t == typeof(char);
        }

        private static string KeyToString(object key)
        {
            if (key is Type t) return t.FullName;
            return key?.ToString() ?? "(null)";
        }

        private static T SafeGet<T>(Func<T> getter)
        {
            try
            {
                return getter();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return default;
            }
        }

        private sealed class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            bool IEqualityComparer<object>.Equals(object x, object y) => ReferenceEquals(x, y);
            int IEqualityComparer<object>.GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
        }
    }
}

#endif