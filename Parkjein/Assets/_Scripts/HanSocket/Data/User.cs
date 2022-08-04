using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HanSocket.Data
{
   
   public class User : MonoBehaviour
   {
      const int MAXHP = 100;

      public int id;
      public int hp;
      public string nickname;
      public int kills = 0;

      [SerializeField]
      private TMP_Text _nicknameText;

      [SerializeField]
      private Slider _hpBar;

      private Vector2 _targetPos;
      private const float _t = (1.0f / (30.0f / 6.0f));

      public void Init(int id, int hp, string nickname, Vector2 pos)
      {
         transform.position = pos;
         this._targetPos = pos;
         
         this.id = id;
         this.hp = hp;

         this._hpBar.maxValue = MAXHP;
         this._hpBar.value    = hp;

         this.nickname        = nickname;
         gameObject.name      = nickname;
         _nicknameText.text   = nickname;
      }

      public void Reset()
      {
         this.id = -1;
         this.nickname = "";
      }

      public void SetHP(int hp)
      {
         _hpBar.value = hp;
      }

      public void SetTargetPos(Vector2 pos)
      {
         _targetPos = pos;
      }

      private void Update() 
      {
         transform.position = Vector2.Lerp(transform.position, _targetPos, _t);
      }
   }
}