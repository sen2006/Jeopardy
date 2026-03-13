using UnityEngine;
using UnityEngine.UI;

public class EnableButtonWhenSaveValid : MonoBehaviour
{
    [SerializeField] Button button;
    void Update()
    {
        button.interactable = PersistentBoardSave.HasSave();
    }
}
