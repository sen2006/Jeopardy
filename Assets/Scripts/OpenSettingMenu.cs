using UnityEngine;
using UnityEngine.InputSystem;

public class OpenSettingMenu : MonoBehaviour {
    [SerializeField] GameObject settingsPrefab;
    [SerializeField] GameObject canvas;
    [SerializeField] bool onEsc;

    public void Update() {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            OpenMenu();
    }

    public void OpenMenu() {
        Instantiate(settingsPrefab, canvas.transform);
    }
}
