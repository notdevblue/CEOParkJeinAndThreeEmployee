using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetUI : MonoBehaviour
{
    [SerializeField]
    private PlayerUI myUI;
    public PlayerUI MyUI { get => myUI; set => myUI = value; }
}
