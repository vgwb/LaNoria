using UnityEngine;
using TMPro;

namespace vgwb.lanoria
{
    public class UI_Leaderboard_Entry : MonoBehaviour
    {
        public TextMeshProUGUI ScoreText;
        public TextMeshProUGUI PositionText;
        public GameObject playerBackground;

        public void Set(int score, int position, bool isPlayer)
        {
            ScoreText.text = score.ToString();
            if (position > 0) {
                PositionText.text = position.ToString();
            }
            playerBackground.SetActive(isPlayer);
        }

        public void Enable(bool enabled)
        {
            gameObject.SetActive(enabled);
        }

    }
}
