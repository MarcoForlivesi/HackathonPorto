using UnityEngine;
using UnityEditor;
using ul = UnityEngine.GUILayout;
using el = UnityEditor.EditorGUILayout;
using System;

namespace Netly.Unity
{
    [CustomEditor(typeof(NetlyHost))]
    public class NetlyHostEditor : Editor
    {
        private NetlyHost _target;

        private void OnEnable()
        {
            _target = (NetlyHost)target;

            if (!_target.changedObjName)
            {
                _target.name = _target.objName;
                _target.changedObjName = true;
            }
        }

        public override void OnInspectorGUI()
        {
            // create callback object if not exist
            CallbackEditor.CreateNew();

            el.Space(20);

            float _width = EditorGUIUtility.currentViewWidth;

            ul.BeginHorizontal();
            {
                ul.BeginVertical();
                {
                    ul.Label("Address");
                    _target.ip = el.TextField("", _target.ip, GUILayout.Height(20), GUILayout.MinWidth(_width / 2.5f));
                }
                ul.EndVertical();

                ul.BeginVertical();
                {
                    ul.Label("Port");
                    _target.port = el.IntField("", _target.port, GUILayout.Height(20), GUILayout.MinWidth(_width / 2.5f));
                }
                ul.EndVertical();
            }
            ul.EndHorizontal();

            ul.Space(20);
            {
                el.BeginHorizontal();
                {                    
                    _target.isServer = ul.Toggle(_target.isServer, $"Is server", GUILayout.MinWidth(_width / 3.5f));
                }
                el.EndHorizontal();
            }

            if (_target.isServer)
            {
                el.Space(20);
                ul.BeginVertical();
                {
                    ul.Label("Max client");
                    _target.maxClient = (int)el.Slider(_target.maxClient, 1, 999);
                }
                ul.EndVertical();

                ul.Space(10);
                ul.BeginVertical();
                {
                    ul.Label("Backlog");
                    _target.backlog = (int)el.Slider(_target.backlog, 0, _target.maxClient);
                }
                ul.EndVertical();

                ul.Space(10);
                ul.BeginVertical();
                {
                    ul.Label("Client connected");
                    _ = (int)el.Slider(_target.clientConnected, 0, _target.clientConnected + _target.maxClient);
                }
                ul.EndVertical();

                ul.Space(10);
                ul.BeginVertical();
                {
                    ul.Label("Client disconnected");
                    _ = (int)el.Slider(_target.clientDisconnected, 0, _target.clientDisconnected + _target.maxClient);
                }
                ul.EndVertical();
            }

            ul.Space(20);
        }
    }
}
