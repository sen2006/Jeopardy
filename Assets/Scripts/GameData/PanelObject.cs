using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PanelObject : ISaveSerialization<PanelObjectSaveData> {
    protected float x;
    protected float y;
    protected float xScale;
    protected float yScale;

    public abstract GameObject LoadToScene(GameObject parent);
    public abstract void LoadToEditorScene(GameObject parent);

    public void SetXY(float x, float y) {
        this.x = x;
        this.y = y;
    }

    internal void SetScale(float x, float y) {
        xScale = x;
        yScale = y;
    }

    public abstract PanelObjectSaveData Save();


    public abstract void Load(PanelObjectSaveData saveData);
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

[Serializable]
public struct PanelObjectSaveData {
    public float x;
    public float y;
    public float xScale;
    public float yScale;
    public List<string> aditionalStringData;
    public List<float> aditionalFloatData;
}