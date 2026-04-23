using UnityEditor;
using UnityEditor.TestTools.CodeCoverage;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;
using System.IO;

namespace CaudilloBay.Editor
{
    public static class CoverageRunner
    {
        [MenuItem("Tools/Tests/Run All Tests with Coverage")]
        public static void RunTestsWithCoverage()
        {
            var api = ScriptableObject.CreateInstance<TestRunnerApi>();
            var settings = new ExecutionSettings(new Filter { testMode = TestMode.EditMode | TestMode.PlayMode });

            // Enable coverage before running tests
            EditorPrefs.SetBool("CodeCoverage_Enabled", true);

            api.Execute(settings);
            Debug.Log("Test execution started with coverage enabled.");
        }

        [MenuItem("Tools/Tests/Open Coverage Report")]
        public static void OpenReport()
        {
            string reportPath = Path.Combine(Application.dataPath, "../CodeCoverage/Report/index.html");
            if (File.Exists(reportPath))
            {
                Application.OpenURL("file://" + Path.GetFullPath(reportPath));
            }
            else
            {
                Debug.LogWarning("Coverage report not found at: " + reportPath);
            }
        }
    }
}
