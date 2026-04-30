using System;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CaudilloBay.Editor
{
    public static class CoverageGuard
    {
        public static void CheckCoverage(string reportPath, float globalThreshold, Dictionary<string, float> moduleThresholds)
        {
            if (!File.Exists(reportPath))
            {
                Debug.LogError($"Coverage report not found at {reportPath}");
                if (Application.isBatchMode) EditorApplication.Exit(1);
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(reportPath);

            XmlNode coverageNode = doc.SelectSingleNode("/coverage");
            if (coverageNode == null)
            {
                Debug.LogError("Invalid Cobertura report format.");
                if (Application.isBatchMode) EditorApplication.Exit(1);
                return;
            }

            float lineRate = float.Parse(coverageNode.Attributes["line-rate"].Value, CultureInfo.InvariantCulture);
            float overallCoverage = lineRate * 100f;

            Debug.Log($"Overall Line Coverage: {overallCoverage:F2}%");

            bool failed = false;
            if (overallCoverage < globalThreshold)
            {
                Debug.LogError($"Overall coverage {overallCoverage:F2}% is below threshold {globalThreshold}%");
                failed = true;
            }

            // Cobertura often groups by assembly as package. Classes are under packages.
            // We'll iterate through all classes and aggregate by their filename/path.
            XmlNodeList classes = doc.GetElementsByTagName("class");

            Dictionary<string, List<float>> moduleRates = new Dictionary<string, List<float>>();
            foreach (var module in moduleThresholds.Keys) moduleRates[module] = new List<float>();

            foreach (XmlNode classNode in classes)
            {
                string filename = classNode.Attributes["filename"].Value; // e.g. Assets/Scripts/Economy/Inventory.cs
                float classRate = float.Parse(classNode.Attributes["line-rate"].Value, CultureInfo.InvariantCulture);

                foreach (var module in moduleThresholds.Keys)
                {
                    if (filename.Replace("\\", "/").Contains(module))
                    {
                        moduleRates[module].Add(classRate);
                    }
                }
            }

            foreach (var entry in moduleThresholds)
            {
                string module = entry.Key;
                float threshold = entry.Value;
                var rates = moduleRates[module];

                if (rates.Count > 0)
                {
                    float sum = 0;
                    foreach (var r in rates) sum += r;
                    float avgRate = (sum / rates.Count) * 100f;

                    Debug.Log($"Module {module} (aggregated from {rates.Count} classes) Coverage: {avgRate:F2}%");
                    if (avgRate < threshold)
                    {
                        Debug.LogError($"Critical module {module} coverage {avgRate:F2}% is below threshold {threshold}%");
                        failed = true;
                    }
                }
                else
                {
                    Debug.LogWarning($"No classes found for module {module}");
                }
            }

            if (failed)
            {
                Debug.LogError("Coverage Guard: THRESHOLD NOT MET.");
                if (Application.isBatchMode) EditorApplication.Exit(1);
            }
            else
            {
                Debug.Log("Coverage Guard: ALL THRESHOLDS PASSED.");
            }
        }

        public static void RunGuard()
        {
            string[] args = System.Environment.GetCommandLineArgs();
            string reportPath = "CodeCoverage/CaudilloBay-opencov/Cobertura.xml";

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-coverageReportPath" && i + 1 < args.Length)
                {
                    reportPath = args[i + 1];
                }
            }

            var modules = new Dictionary<string, float>
            {
                { "Assets/Scripts/Economy", 80f },
                { "Assets/Scripts/Politics", 80f },
                { "Assets/Scripts/Construction", 80f }
            };

            CheckCoverage(reportPath, 70f, modules);
        }
    }
}
