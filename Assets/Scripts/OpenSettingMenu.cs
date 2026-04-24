using UnityEngine;
using UnityEngine.InputSystem;

public class OpenSettingMenu : MonoBehaviour {
    [SerializeField] GameObject settingsPrefab;
    [SerializeField] GameObject canvas;
    [SerializeField] bool onEsc;

    private GameObject obj;

    public void Update() {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && obj == null)
            OpenMenu();
    }

    public void OpenMenu() {
        obj = Instantiate(settingsPrefab, canvas.transform);
    }
}
