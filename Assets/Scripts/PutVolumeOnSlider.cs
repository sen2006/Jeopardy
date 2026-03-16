using UnityEngine;
using UnityEngine.UI;

public class PutVolumeOnSlider : MonoBehaviour
{
    [SerializeField] Slider slider;

    private void Start() {
        slider.value = Settings.GetSettings().volume;
    }
}
