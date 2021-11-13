using Asteroids.Statics;
using TMPro;
using UnityEngine;

namespace Asteroids.Views
{
    public class LifeView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _lifeText;
        [SerializeField] string _lifeLabel;
        
        void Awake()
        {
            LifeHandler.OnPlayerLifeChange += OnPlayerLifeChange;
            OnPlayerLifeChange(5);
        }

        void OnPlayerLifeChange(int curLife)
        {
            _lifeText.text = $"{_lifeLabel}: {curLife}";

            if (curLife <= 0)
            {
                Debug.Log("Player is Dead");
            }
        }
    }   
}
