using System;
using UnityEditor;
using UnityEngine;

namespace EditorTools.Favorites
{
    public class StringInputDialog : EditorWindow
    {
        private const string inputField = "input field";

        private string text;
        private string ok;
        private string cancel;
        private string input;
        private Action<string> callback;

        public static void Show(string title,
            string text,
            string initialInput,
            string ok,
            string cancel,
            Action<string> callback)
        {
            EditorApplication.delayCall += () =>
            {
                var window = GetWindow<StringInputDialog>(true, title);
                window.text = text;
                window.ok = ok;
                window.input = initialInput;
                window.cancel = cancel;
                window.callback = callback;

                window.maxSize = new Vector2(300, 100);
                window.minSize = window.maxSize;
                window.ShowModalUtility();
            };
        }

        private void OnGUI()
        {
            GUILayout.Space(20);

            GUILayout.Label(text);

            GUI.FocusControl(inputField);
            GUI.SetNextControlName(inputField);
            input = GUILayout.TextField(input);

            DisplayButtons();
        }

        private void DisplayButtons()
        {
            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(ok) || IsEnterPressed())
            {
                callback(input);
                Close();
            }

            if (GUILayout.Button(cancel) || IsEscapePressed())
            {
                Close();
            }

            GUILayout.EndHorizontal();
        }

        private static bool IsEnterPressed()
        {
            var e = Event.current;

            return e.isKey && (e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter);
        }

        private static bool IsEscapePressed()
        {
            var e = Event.current;

            return e.isKey && (e.keyCode == KeyCode.Escape);
        }
    }
}