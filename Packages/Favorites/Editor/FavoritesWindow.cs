using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EditorTools.Favorites
{
    /// <summary>
    /// An editor window that lets you store assets and GameObjects in it for quickly finding them again.
    /// </summary>
    public class FavoritesWindow : EditorWindow, IHasCustomMenu
    {
        private readonly struct BackgroundColor : IDisposable
        {
            private readonly Color previousColor;

            public BackgroundColor(Color color)
            {
                previousColor = GUI.backgroundColor;
                GUI.backgroundColor = color;
            }

            public void Dispose()
            {
                GUI.backgroundColor = previousColor;
            }

            public static BackgroundColor Multiply(Color color)
            {
                return new BackgroundColor(color * GUI.backgroundColor);
            }
        }

        [Serializable]
        private struct ObjectInfo : ISerializationCallbackReceiver
        {
            public Object obj;
            [SerializeField] private string serializedId;
            public GlobalObjectId id { get; private set; }
            [SerializeField] private string _name;

            public string name
            {
                get
                {
                    if (obj)
                    {
                        _name = obj.name;
                    }

                    return _name;
                }
            }

            public ObjectInfo(Object obj)
            {
                this.obj = obj;
                id = GlobalObjectId.GetGlobalObjectIdSlow(obj);
                serializedId = id.ToString();
                _name = obj.name;
            }

            public ObjectInfo(GlobalObjectId id, string name)
            {
                obj = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id);
                this.id = id;
                serializedId = id.ToString();
                _name = obj ? obj.name : name;
            }

            public override bool Equals(object obj)
            {
                if (GetType() != obj.GetType())
                {
                    return false;
                }

                return this.obj == ((ObjectInfo)obj).obj;
            }

            public override int GetHashCode()
            {
                return obj.GetHashCode();
            }

            public void FindReferenceIfNull()
            {
                if (!obj)
                {
                    obj = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id);
                }
            }

            public void OnBeforeSerialize()
            {
            }

            public void OnAfterDeserialize()
            {
                GlobalObjectId.TryParse(serializedId, out var id);
                this.id = id;
            }
        }

        private static class DragAndDrop
        {
            private const float minDragPixels = 7.2f;
            private static Vector2 clickPosition;
            private static Object dragObject;

            public static void MakeDragSource(Rect rect, Object objectToDrag)
            {
                var currentEvent = Event.current;
                if (rect.Contains(currentEvent.mousePosition))
                {
                    if (currentEvent.type == EventType.MouseDown)
                    {
                        clickPosition = currentEvent.mousePosition;
                        dragObject = objectToDrag;
                    }
                    else if (dragObject == objectToDrag &&
                             currentEvent.type == EventType.MouseDrag &&
                             Vector2.Distance(clickPosition, currentEvent.mousePosition) >= minDragPixels)
                    {
                        UnityEditor.DragAndDrop.PrepareStartDrag();
                        UnityEditor.DragAndDrop.objectReferences = new Object[] { objectToDrag };
                        // PrepareStartDrag is supposed to clear paths, but it didn't really.
                        UnityEditor.DragAndDrop.paths = Array.Empty<string>();
                        UnityEditor.DragAndDrop.StartDrag(objectToDrag.name);
                        currentEvent.Use();
                    }
                }
            }
        }

        private ReorderableList list;
        [SerializeField] private List<ObjectInfo> items = default;
        private static GUIStyle buttonStyle;
        private static GUIStyle removeStyle;
        private Vector2 scrollPosition;


        [MenuItem("Window/Favorites")]
        private static void Open()
        {
            var window = GetWindow<FavoritesWindow>();

            window.SetWindowTitle();
        }

        private void SetWindowTitle(string title = null)
        {
            if (title == null)
            {
                title = "Favorites";
            }

            var icon = EditorGUIUtility.IconContent("d_Favorite").image;
            titleContent = new GUIContent(title, icon);
            Repaint();
        }

        private void OnEnable()
        {
            RefreshList();
        }

        private void OnHierarchyChange()
        {
            RefreshList();
        }

        private void GenerateList()
        {
            if (items == null)
            {
                items = new List<ObjectInfo>();
            }

            if (list != null) return;

            list = new ReorderableList(items, typeof(Object), true, false, false, false);

            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                if (index >= items.Count)
                {
                    GUI.Label(rect, "Error");
                    return;
                }

                var typeRect = new Rect(rect.x, rect.y, 100, rect.height);
                var buttonRect = new Rect(typeRect.xMax, rect.y, rect.width - 30 - 150, rect.height);
                var removeRect = new Rect(buttonRect.xMax + 5, rect.y, 25, rect.height);

                var referencedObject = items[index].obj;
                if (referencedObject != null)
                {
                    var isPrefab = referencedObject is GameObject && PrefabUtility.IsPartOfAnyPrefab(referencedObject);
                    GUI.Label(typeRect, isPrefab ? "Prefab" : referencedObject.GetType().Name);

                    DisplayItemButton(items[index], buttonRect);
                }
                else
                {
                    GUI.Label(typeRect, "Missing");
                    GUI.Label(buttonRect, items[index].name);
                }

                using (BackgroundColor.Multiply(new Color(1, 0.5f, 0.5f)))
                {
                    if (GUI.Button(removeRect, "X", removeStyle))
                    {
                        items.RemoveAt(index);
                    }
                }
            };

            list.drawHeaderCallback = rect => { GUI.Label(rect, titleContent.text); };
        }

        private void DisplayItemButton(ObjectInfo info, Rect buttonRect)
        {
            var isScene = info.obj is SceneAsset;
            var isPrefab = PrefabUtility.IsPartOfAnyPrefab(info.obj);

            if (isScene || isPrefab)
            {
                var openButtonRect = new Rect(buttonRect);
                openButtonRect.xMin = openButtonRect.xMax - 48;

                if (GUI.Button(openButtonRect, "Open"))
                {
                    if (isScene)
                    {
                        EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(info.obj));
                    }
                    else
                    {
                        AssetDatabase.OpenAsset(info.obj);
                    }
                }

                buttonRect.xMax -= 50;
            }

            DragAndDrop.MakeDragSource(buttonRect, info.obj);
            var icon = GetIconForObject(info.obj);
            var buttonContent = new GUIContent(info.name, icon);
            if (GUI.Button(buttonRect, buttonContent, buttonStyle))
            {
                Selection.activeObject = info.obj;
                EditorGUIUtility.PingObject(info.obj);
            }
        }

        private static Texture GetIconForObject(Object obj)
        {
            if (obj is GameObject)
            {
                return EditorGUIUtility.IconContent("GameObject Icon").image;
            }

            return AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(obj));
        }

        private void OnGUI()
        {
            GenerateList();
            GenerateStyles();

            const int padding = 10;

            var addRect = new Rect(padding, padding, position.width - padding * 2, EditorGUIUtility.singleLineHeight);
            var newObject = EditorGUI.ObjectField(addRect, new GUIContent("Add object"), null, typeof(Object), true);
            if (newObject)
            {
                var objectInfo = new ObjectInfo(newObject);
                items.Remove(objectInfo);
                items.Insert(0, objectInfo);
            }

            var scrollRect = new Rect(5, 35, position.width - 5, position.height - 35);
            var viewRect = new Rect(0, 0, position.width - 20, items.Count * list.elementHeight + 28);
            scrollPosition = GUI.BeginScrollView(scrollRect, scrollPosition, viewRect, false, true, GUIStyle.none,
                GUI.skin.verticalScrollbar);

            if (items != null && items.Count > 0)
            {
                list.DoList(viewRect);
            }
            else
            {
                DrawHintsWindow();
            }

            GUI.EndScrollView();
        }

        private void GenerateStyles()
        {
            if (buttonStyle != null) return;

            buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.alignment = TextAnchor.MiddleLeft;

            removeStyle = new GUIStyle(GUI.skin.button);
            removeStyle.border = new RectOffset(4, 4, 4, 4);
        }

        private void RefreshList()
        {
            if (items == null) return;

            for (var i = 0; i < items.Count; i++)
            {
                if (items[i].obj == null)
                {
                    items[i] = new ObjectInfo(items[i].id, items[i].name);
                }
                else
                {
                    items[i] = new ObjectInfo(items[i].obj);
                }
            }

            Repaint();
        }

        private void DrawHintsWindow()
        {
            const string hints = "• Drag an object into the box at the top to add it to the list.\n"
                                 + "• You can click on an object to select it or drag it somewhere.\n"
                                 + "• Use the context menu (top right) to open an additional favorites window with its own list.\n"
                                 + "• Use the context menu again to change the window title.";
            EditorGUILayout.HelpBox(hints, MessageType.Info);
        }

        void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Open Additional Window"), false, () =>
            {
                var window = CreateWindow<FavoritesWindow>(typeof(FavoritesWindow));
                window.SetWindowTitle();
            });

            menu.AddItem(new GUIContent("Change Window Title"), false,
                () =>
                {
                    StringInputDialog.Show("Rename FavoritesWindow", "Please enter a new title:", titleContent.text,
                        "Ok", "Cancel", SetWindowTitle);
                });
        }
    }
}