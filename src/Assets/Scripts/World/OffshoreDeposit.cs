using UnityEngine;

namespace CaudilloBay.World
{
    [System.Serializable]
    public class OffshoreDeposit
    {
        public string resourceId;
        public Vector2 position;
        public float depth;
        public float quantity;
        public float richness = 1.0f;

        public bool isDiscovered = false;
        public bool isClaimed = true;

        public OffshoreDeposit(string id, Vector2 pos, float d, float qty)
        {
            resourceId = id;
            position = pos;
            depth = d;
            quantity = qty;
        }

        public float Extract(float amount)
        {
            float extracted = Mathf.Min(amount, quantity);
            quantity -= extracted;
            return extracted;
        }
    }
}
