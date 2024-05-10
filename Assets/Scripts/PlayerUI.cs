using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    public Text CoinText;
    // Start is called before the first frame update
    void Start()
    {
        CoinText = GameObject.Find("Interface/Coins").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
