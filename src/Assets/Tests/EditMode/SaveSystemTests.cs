using NUnit.Framework;
using CaudilloBay.Core;
using UnityEngine;
using System.IO;

namespace CaudilloBay.Tests
{
    public class SaveSystemTests
    {
        private SaveSystem saveSystem;
        private GameObject go;
        private string testFile = "test_save.json";

        [SetUp]
        public void Setup()
        {
            go = new GameObject("SaveSystem");
            saveSystem = go.AddComponent<SaveSystem>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(go);
            string path = Path.Combine(Application.persistentDataPath, testFile);
            if (File.Exists(path)) File.Delete(path);
        }

        [Test]
        public void SaveGame_CreatesFile()
        {
            saveSystem.SaveGame(testFile);
            string path = Path.Combine(Application.persistentDataPath, testFile);
            Assert.IsTrue(File.Exists(path));
        }

        [Test]
        public void DeleteSave_RemovesFile()
        {
            saveSystem.SaveGame(testFile);
            saveSystem.DeleteSave(testFile);
            string path = Path.Combine(Application.persistentDataPath, testFile);
            Assert.IsFalse(File.Exists(path));
        }
    }
}
