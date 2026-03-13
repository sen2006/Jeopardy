using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneSwitch : MonoBehaviour
{
    [SerializeField] string sceneName;

    public void Trigger() {
        SceneManager.LoadScene(sceneName);
    }
}
