using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InitialCharacterSetup : MonoBehaviour
{
    public ClothingPicker clothingPicker;
    public ColorPicker colorPicker;

    public InputField nameInputField;

    public ButtonHandler btnHandler;
    public GlobalController globalController;
    public void RandomizeCharacter()
    {
        clothingPicker.gameObject.SetActive(true);

        nameInputField.text = TextReader.getRandomName();

        clothingPicker.Initialize();

        clothingPicker.setRandomClothing();
        colorPicker.setRandomColors();
        clothingPicker.setRandomBodyProportions();
        clothingPicker.previewResetPos();
        clothingPicker.previewLand();

        btnHandler.createCharacter();
        globalController.goScreenAfterCharacterCreation();
        
        //clothingPicker.gameObject.SetActive(false);
    }
}
