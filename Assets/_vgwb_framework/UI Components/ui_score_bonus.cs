using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace vgwb.framework.ui
{
    public class ui_score_bonus : MonoBehaviour
    {
        public TextMeshProUGUI ScoreText;

        public void Init(int value)
        {
            ScoreText.text = (value >= 0 ? "+" : "-") + value;
            transform.DOLocalMoveY(80, 1);
            GetComponent<CanvasGroup>().DOFade(0, 1);
            Destroy(gameObject, 1.1f);
        }
    }
}
