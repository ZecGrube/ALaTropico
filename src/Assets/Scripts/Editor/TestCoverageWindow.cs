using UnityEditor;
using UnityEngine;
using UnityEditor.TestTools.TestRunner.Api;

namespace CaudilloBay.Editor
{
    public class TestCoverageWindow : EditorWindow
    {
        [MenuItem("Tools/Test & Coverage Runner")]
        public static void ShowWindow()
        {
            GetWindow<TestCoverageWindow>("Test Runner");
        }

        private void OnGUI()
        {
            GUILayout.Label("Caudillo Bay Test Runner", EditorStyles.boldLabel);

            if (GUILayout.Button("Run EditMode Tests"))
            {
                RunTests(TestMode.EditMode);
            }

            if (GUILayout.Button("Run PlayMode Tests"))
            {
                RunTests(TestMode.PlayMode);
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Run All Tests with Coverage"))
            {
                CoverageRunner.RunTestsWithCoverage();
            }

            if (GUILayout.Button("Open Coverage Report"))
            {
                CoverageRunner.OpenReport();
            }
        }

        private void RunTests(TestMode mode)
        {
            var api = ScriptableObject.CreateInstance<TestRunnerApi>();
            api.Execute(new ExecutionSettings(new Filter { testMode = mode }));
        }
    }
}
