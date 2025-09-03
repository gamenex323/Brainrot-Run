using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModesViewController : View
{
    enum btnType { SPRINT, HURDLES, RELAYS, RACEOFTHEDAY, PRACTICE, CLOSE}

    public Button sprintBtn, hurdlesBtn, relaysBtn, raceOfTheDayBtn, practiceBtn, closeBtn;

    public override void Initialize()
    {
        sprintBtn.onClick.AddListener(()=>ButtonClicked(btnType.SPRINT));
        hurdlesBtn.onClick.AddListener(()=>ButtonClicked(btnType.HURDLES));
        relaysBtn.onClick.AddListener(()=>ButtonClicked(btnType.RELAYS));
        raceOfTheDayBtn.onClick.AddListener(()=>ButtonClicked(btnType.RACEOFTHEDAY));
        practiceBtn.onClick.AddListener(()=>ButtonClicked(btnType.PRACTICE));
        closeBtn.onClick.AddListener(()=>ButtonClicked(btnType.CLOSE));
    }

    private void ButtonClicked(btnType clickedButtontype)
    {
        switch (clickedButtontype)
        {
            case btnType.SPRINT:

                break;
            case btnType.HURDLES:

                break;
            case btnType.RELAYS:

                break;
            case btnType.RACEOFTHEDAY:

                break;
            case btnType.PRACTICE:

                break;
            case btnType.CLOSE:
                Hide();
                break;
            default:
                break;
        }
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Show(ViewContext context = null)
    {
        base.Show(context);
    }
}
