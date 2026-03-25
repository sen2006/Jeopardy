using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WindowModeChange : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] List<ScreenModeOptions> options;

    private void Start() {
        dropdown.ClearOptions();
        dropdown.onValueChanged.AddListener(ValueChanged);
        foreach (ScreenModeOptions option in options) {
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
            data.text = option.name;
            dropdown.options.Add(data);
        }
    }

    private void ValueChanged(int id) {
        Screen.fullScreenMode = options[id].mode;
    }

    private void Update() {
        if (dropdown.IsExpanded) return;
        int i = 0;
        foreach (ScreenModeOptions option in options) {
            if (Screen.fullScreenMode == option.mode) {
                dropdown.value = i;
                break;
            }
            i++;
        }

    }
}

[Serializable]
struct ScreenModeOptions {
    public string name;
    public FullScreenMode mode;
}
