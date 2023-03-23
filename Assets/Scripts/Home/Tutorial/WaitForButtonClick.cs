using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// When yielded to in a coroutine, waits for the button given in the
/// constructor to be clicked.
/// </summary>
public class WaitForButtonClick : CustomYieldInstruction {

    private bool wasButtonClicked;
    private Button button;

    public WaitForButtonClick (Button button) {
        this.button = button;
        button.onClick.AddListener (OnClickAction);
    }

    public override bool keepWaiting {
        get { return !wasButtonClicked; }
    }

    private void OnClickAction () {
        wasButtonClicked = true;
        button.onClick.RemoveListener (OnClickAction);
    }
}
