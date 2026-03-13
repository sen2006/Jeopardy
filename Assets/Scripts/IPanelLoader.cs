using UnityEngine;

public interface IPanelLoader
{
    public abstract void loadPanel(PanelData panelData);
    void setLoadedQeastion(QuestionData data);
}
