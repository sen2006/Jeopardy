using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public abstract class PanelObject : IByteSerialization  {
    protected float x;
    protected float y;
    protected float xScale;
    protected float yScale;

    public abstract GameObject LoadToScene(GameObject parent);
    public abstract void LoadToEditorScene(GameObject parent);
    public abstract void Deserialize(Packet pPacket);
    public abstract void Serialize(Packet pPacket);

    public void SetXY(float x, float y) {
        this.x = x;
        this.y = y;
    }

    internal void SetScale(float x, float y) {
        xScale = x;
        yScale = y;
    }
}

public abstract class EditorPanelObject<T> : EditorPanelObject where T : PanelObject {

    public abstract T getData();

    public override PanelObject GetDataObject() {
        return getData();
    }
}

public abstract class EditorPanelObject : MonoBehaviour {
    public abstract PanelObject GetDataObject();
}