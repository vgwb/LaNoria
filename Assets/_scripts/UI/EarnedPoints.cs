using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace vgwb.lanoria
{
    public class EarnedPoints : MonoBehaviour
    {
        public TMP_Text PlacementPointsTxt;
        public TMP_Text AdjacencyPointsTxt;
        public TMP_Text AreaPointsTxt;

        private void Awake()
        {
            Destroy(gameObject, 1.5f);
        }

        public void SetPlacementPoints(int points)
        {
            if (points > 0) {
                PlacementPointsTxt.text = points.ToString();
            } else {
                PlacementPointsTxt.gameObject.SetActive(false);
            }
        }

        public void SetAdjacencyPoints(int points)
        {
            if (points > 0) {
                AdjacencyPointsTxt.text = points.ToString();
            } else {
                AdjacencyPointsTxt.gameObject.SetActive(false);
            }
        }

        public void SetAreaPoints(int points)
        {
            if (points > 0) {
                AreaPointsTxt.text = points.ToString();
            } else {
                AreaPointsTxt.gameObject.SetActive(false);
            }
        }
    } 
}
