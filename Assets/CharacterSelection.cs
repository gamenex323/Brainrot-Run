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
        SetCharacterUI();
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
        FindObjectOfType<GlobalController>().racerPrefab = characterInfos[currentCharacter].characterPrefab;
    }

    public void ActiveSelection()
    {
        Invoke(nameof(PanelOnWithDelay),1f);
    }
    void PanelOnWithDelay()
    {
        Debug.Log("Character Selection On", gameObject);
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
[System.Serializable]
public class CharacterInfo
{
    public string name;
    public Sprite avatar;
    public GameObject characterPrefab;

}
