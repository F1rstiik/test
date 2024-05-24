using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    public Text CoinText;
    public Slider HP;
    public Player Player;
    // Start is called before the first frame update
    void Start()
    {
        CoinText = GameObject.Find("Interface/Coins").GetComponent<Text>();
        HP = GameObject.Find("Interface/HP").GetComponent<Slider>();
        Player = GameObject.Find("Player").GetComponent <Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeHP()
    {
        HP.value = Player.HP / Player.MaxHP;
    }
}
