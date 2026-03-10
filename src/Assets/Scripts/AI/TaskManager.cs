using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.World;
using CaudilloBay.Data;

namespace CaudilloBay.AI
{
    public class TaskManager : MonoBehaviour
    {
        public static TaskManager Instance { get; private set; }

        private Queue<ConstructionTask> pendingConstructionTasks = new Queue<ConstructionTask>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void AddConstructionTask(Building building)
        {
            pendingConstructionTasks.Enqueue(new ConstructionTask { targetBuilding = building });
            Debug.Log($"New construction task added for {building.displayName}");
        }

        public ConstructionTask? RequestTask()
        {
            if (pendingConstructionTasks.Count > 0)
            {
                return pendingConstructionTasks.Dequeue();
            }
            return null;
        }
    }

    public struct ConstructionTask
    {
        public Building targetBuilding;
    }
}
