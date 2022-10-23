using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Netly.Unity
{
    [CustomEditor(typeof(NetlyCallback))]
    public class CallbackEditor : Editor
    {
        const string _name = "[ Netly ]";
        private NetlyCallback _target;

        private void OnEnable()
        {
            _target = (NetlyCallback)target;
            _target.gameObject.name = _name;
            OnlyAny();
        }

        private void OnlyAny()
        {
            var list = GameObject.FindObjectsOfType<NetlyCallback>();

            for (int i = 1; i < list.Length; i++)
            {
                DestroyImmediate(list[i].gameObject);
            }
        }

        public override void OnInspectorGUI()
        {
            _target.enabled = true;
            _target.gameObject.name = EditorGUILayout.TextField("Name", _target.gameObject.name);
            _target.gameObject.transform.position = Vector3.zero;
            _target.gameObject.transform.localScale = Vector3.one;
            _target.gameObject.transform.rotation = Quaternion.identity;
            _target.gameObject.transform.parent = null;
            OnlyAny();
        }

        public static void CreateNew()
        {
            var list = GameObject.FindObjectsOfType<NetlyCallback>();
            if (list.Length <= 0) new GameObject(_name).AddComponent<NetlyCallback>();
        }
    }
}