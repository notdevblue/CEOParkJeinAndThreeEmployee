using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace UI.InGame
{    
    public class GameEndCanvas : MonoBehaviour
    {
        public TMP_Text text;
        public Button btnBack;

        private void Awake()
        {
            btnBack.onClick.AddListener(() => {
                SceneManager.LoadScene("ConnectionScene");
            });
        }

        public void Display(string reason)
        {
            text.text = reason;
            gameObject.SetActive(true);
        }
    }
}