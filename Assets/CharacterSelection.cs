using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public static CharacterSelection instance;
    public CharacterInfo[] characterInfos;
    public Text characterName;
    public Image characterAvatar;
    public static int currentCharacter;
    // Start is called before the first frame update
    void Start()
    {
        if(!instance == null) instance = this;   
    }

    public void OnRightClick()
    {
        

        if (currentCharacter < characterInfos.Length-1)
        {
            currentCharacter++;
            SetCharacterUI();
        }
    }
    public void OnLeftClick()
    {
        

        if (currentCharacter > 0)
        {
            currentCharacter--;
            SetCharacterUI();
        }
    }

    void SetCharacterUI()
    {
        characterName.text = characterInfos[currentCharacter].name;
        characterAvatar.sprite = characterInfos[currentCharacter].avatar;
    }
}
[System.Serializable]
public class CharacterInfo
{
    public string name;
    public Sprite avatar;

}
