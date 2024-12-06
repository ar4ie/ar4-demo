using TMPro;
using UnityEngine;

namespace Ar4.Zones.UI
{
    public class ZoneScreenView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI scoreText;
        
        public void UpdateScore(int score)
        {
            scoreText.text = $"Score: {score}"; // todo I18
        }
    }
}