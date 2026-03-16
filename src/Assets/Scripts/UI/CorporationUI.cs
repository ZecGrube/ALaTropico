using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using CaudilloBay.Economy;
using CaudilloBay.Politics;

namespace CaudilloBay.UI
{
    public class CorporationUI : MonoBehaviour
    {
        public GameObject corporationEntryPrefab;
        public Transform listContainer;

        public void RefreshList()
        {
            foreach (Transform child in listContainer)
            {
                Destroy(child.gameObject);
            }

            if (CorporationManager.Instance == null) return;

            foreach (var corp in CorporationManager.Instance.corporations)
            {
                GameObject entry = Instantiate(corporationEntryPrefab, listContainer);
                // Setup entry UI components (Text, Buttons)
                // entry.GetComponentInChildren<Text>().text = $"{corp.corporationName} ({corp.type}) - ${corp.treasury:F0}";

                // Button buyBtn = entry.transform.Find("BuyButton").GetComponent<Button>();
                // buyBtn.onClick.AddListener(() => BuyShares(corp));

                // Button lobbyBtn = entry.transform.Find("LobbyButton").GetComponent<Button>();
                // lobbyBtn.onClick.AddListener(() => OpenLobbyPanel(corp));
            }
        }

        public void BuyShares(Corporation corp)
        {
            float cost = corp.sharePrice * 10;
            if (EconomyManager.Instance.treasuryBalance >= cost)
            {
                EconomyManager.Instance.AddFunds(-cost);
                corp.IssueShares(10); // Simplified: state buying newly issued shares
                Debug.Log($"Bought shares in {corp.corporationName}");
                RefreshList();
            }
        }

        public void LobbyFaction(Corporation corp, FactionType faction)
        {
            float cost = 500f;
            if (corp.treasury >= cost)
            {
                corp.Lobby(faction, cost);
                Debug.Log($"{corp.corporationName} lobbied {faction}");
                RefreshList();
            }
        }
    }
}
