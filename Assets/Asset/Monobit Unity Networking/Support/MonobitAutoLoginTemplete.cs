using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using MonobitEngine;

namespace Monobit.Support
{
    [Serializable]
    [RequireComponent(typeof(MonobitView))]
    [AddComponentMenu("Monobit Networking Support/Monobit Auto Login Templete &v")]
    public class MonobitAutoLoginTemplete : MonobitEngine.MonoBehaviour
    {
        [SerializeField]
        public GameObject InstantiatePrefab = null;

        [SerializeField]
        public Vector3 camPosition = new Vector3(1, 1, -3);

        [SerializeField]
        public Quaternion camRotation = Quaternion.identity;

		private GameObject gameObject = null;

		private bool bStart = false;
		private bool bSelectMenu = false;

        void OnGUI()
        {
            if (bSelectMenu == false)
            {
                if (!MonobitNetwork.isConnect)
                {
                    if (GUILayout.Button("Connect", GUILayout.Width(150)))
                    {
                        bSelectMenu = true;
                        MonobitNetwork.autoJoinLobby = true;
                        MonobitNetwork.ConnectServer("MonobitAutoLoginTemplete_v0.1");
                    }
                }
                else if (MonobitNetwork.inRoom)
				{
					if (!bStart)
					{
						if (GUILayout.Button("GameStart", GUILayout.Width(150)))
						{
							bSelectMenu = true;
							monobitView.RPC("GameStart", MonobitTargets.All, null);
						}
					}
					else
					{
						if (GUILayout.Button("Disconnect", GUILayout.Width(150)))
						{
							MonobitNetwork.DisconnectServer();
						}
					}
                }
            }
        }

        void OnConnectToServerFailed(DisconnectCause cause)
        {
            bSelectMenu = false;
            Monobit.Utl.LogE("OnConnectToServerFailed cause={0}", cause);
        }
        void OnDisconnectedFromServer()
        {
            bSelectMenu = false;

            // 全てのオブジェクトを消すため、シーンを再ロードする。
            Application.LoadLevel(Application.loadedLevel);
        }
        void OnJoinedLobby()
        {
			bSelectMenu = false;
			Monobit.Utl.LogD("OnJoinedLobby");
			MonobitNetwork.JoinRandomRoom();
		}
		void OnJoinRoomFailed()
        {
            Monobit.Utl.LogD("OnJoinRoomFailed");
            MonobitNetwork.CreateRoom("AutoLoginRoom");
        }
        void OnMonobitRandomJoinFailed()
        {
            Monobit.Utl.LogD("OnMonobitRandomJoinFailed");
            MonobitNetwork.CreateRoom("AutoLoginRoom");
        }
        void OnJoinedRoom()
        {
			Monobit.Utl.LogD("OnJoinedRoom");
		}

		[MunRPC]
		void GameStart()
		{
			bStart = true;
			bSelectMenu = false;
            if (InstantiatePrefab == null || gameObject != null)
            {
                return;
            }
			gameObject = MonobitNetwork.Instantiate(InstantiatePrefab.name, Vector3.zero, Quaternion.identity, 0) as GameObject;
            if (gameObject != null)
            {
                Camera mainCamera = GameObject.FindObjectOfType<Camera>();
                mainCamera.GetComponent<Camera>().enabled = false;

                Camera camera = gameObject.GetComponentInChildren<Camera>();
                if (camera == null)
                {
                    GameObject camObj = new GameObject();
                    camObj.name = "Camera";
                    camera = camObj.AddComponent<Camera>();
                    camera.transform.parent = gameObject.transform;
                }
                camera.transform.position = camPosition;
                camera.transform.rotation = camRotation;
            }
        }
    }

#if UNITY_EDITOR
    /**
     * MonobitAutoLoginTemplete のInspector表示用クラス.
     */
    [CustomEditor(typeof(MonobitAutoLoginTemplete))]
    public class MonobitAutoLoginTempleteInspector : Editor
    {
        /** MonobitAutoLoginTempletet 本体. */
        MonobitAutoLoginTemplete obj;

        /**
         * Inspector上のGUI表示.
         */
        public override void OnInspectorGUI()
        {
            GUILayout.Space(5);

            // 本体の取得
            obj = target as MonobitAutoLoginTemplete;

            // 標題の表示
            EditorGUILayout.LabelField("Instantiate Player", EditorStyles.boldLabel);

            EditorGUI.indentLevel = 2;

            // プレハブの登録
            obj.InstantiatePrefab = EditorGUILayout.ObjectField("Prefab", obj.InstantiatePrefab, typeof(GameObject), false) as GameObject;

            // 登録したプレハブが Resources 内に存在するかどうかを調べる
            if (obj.InstantiatePrefab != null)
            {
                GameObject tmp = Resources.Load(obj.InstantiatePrefab.name, typeof(GameObject)) as GameObject;
                if (tmp == null)
                {
                    EditorGUILayout.HelpBox("This Prefab is not included in the 'Resources' folder .", MessageType.Warning, true);
                }
            }

            // 座標・回転量を入力
            obj.camPosition = EditorGUILayout.Vector3Field("cam position", obj.camPosition);
            Vector3 camRotation = obj.camRotation.eulerAngles;
            camRotation = EditorGUILayout.Vector3Field("cam rotation", camRotation);
            obj.camRotation.eulerAngles = camRotation;

            EditorGUI.indentLevel = 0;
            GUILayout.Space(5);

            // データの更新
            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(obj);
            }
        }
    }
#endif // UNITY_EDITOR
}

