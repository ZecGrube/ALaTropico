using UnityEngine;

namespace CaudilloBay.Systems.Automation
{
    public class Robot : MonoBehaviour
    {
        public string robotId;
        public string modelName;
        public float batteryLevel = 100f;
        public float maintenanceStatus = 100f;
        public float energyConsumption = 0.1f;

        public void Update()
        {
            batteryLevel -= energyConsumption * Time.deltaTime;
            if (batteryLevel < 0) batteryLevel = 0;

            maintenanceStatus -= 0.01f * Time.deltaTime;
        }

        public bool IsOperational() => batteryLevel > 0 && maintenanceStatus > 0;
    }
}
