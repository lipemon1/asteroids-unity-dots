using Asteroids.Statics;
using TMPro;
using UnityEngine;

namespace Asteroids.Views
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _scoreText;
        [SerializeField] string _scoreLabel;
        
        void Awake()
        {
            ScoreHandler.OnScoreChanged += OnScoreChanged;
            OnScoreChanged(0);
        }

        void OnScoreChanged(int newScore)
        {
            _scoreText.text = $"{_scoreLabel}: {newScore}";
        }
    }   
}
