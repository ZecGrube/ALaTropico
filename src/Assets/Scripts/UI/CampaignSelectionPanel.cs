using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using CaudilloBay.Core;
using CaudilloBay.Data;

namespace CaudilloBay.UI
{
    public class CampaignSelectionPanel : MonoBehaviour
    {
        public List<CampaignMission> missions = new List<CampaignMission>();
        public GameObject missionEntryPrefab;
        public Transform listContainer;

        private void Start()
        {
            // In a real project, load from Resources/Missions
            RefreshList();
        }

        public void RefreshList()
        {
            foreach (Transform t in listContainer) Destroy(t.gameObject);

            foreach (var mission in missions)
            {
                // Instantiate entry and setup with mission data
            }
        }

        public void SelectMission(CampaignMission mission)
        {
            GameStateManager.Instance.StartCampaignMission(mission);
        }
    }
}
