using System.Collections;
using TMPro;
using UnityEngine;

public class CodeButton : MonoBehaviour {
    [SerializeField] private TMP_Text codeText;

    private string _roomId;
    private Coroutine _clickEffectCoroutine;

    public void Init(string roomId) {
        _roomId = roomId;

        string stringId = CodeConverter.toString(ulong.Parse(roomId));

            codeText.text = stringId;
    }

    public void CopyCode() {
        GUIUtility.systemCopyBuffer = _roomId;

        if (_clickEffectCoroutine != null)
            StopCoroutine(_clickEffectCoroutine);
        _clickEffectCoroutine = StartCoroutine(ClickEffect());

    }

    private WaitForSeconds _wait = new(0.13f);
    private WaitForSeconds _waitToReturn = new(1f);
    private IEnumerator ClickEffect() {
        codeText.text = "Copied!";

        yield return _waitToReturn;

        codeText.text = "";
        for (int i = 0; i < _roomId.Length; i++) {
            codeText.text += _roomId[i];
            yield return _wait;
        }
    }
}