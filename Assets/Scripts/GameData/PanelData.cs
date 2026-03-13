

using System;
using System.Collections.Generic;
using UnityEngine;

public class PanelData : IByteSerialization {
   
    List<PanelObject> objects = new List<PanelObject>();

    public PanelData() {
    }

    public GameObject[] loadToScene(GameObject parent) {
        List<GameObject> loadedObjects = new List<GameObject>();
        foreach (PanelObject obj in objects) {
            loadedObjects.Add(obj.LoadToScene(parent));
        }
        return loadedObjects.ToArray();
    }

    public void loadToEditorScene(GameObject parent) {
        foreach (PanelObject obj in objects) {
            obj.LoadToEditorScene(parent);
        }
    }

    public void Deserialize(Packet pPacket) {
        objects = pPacket.ReadList<PanelObject>();
    }

    public void Serialize(Packet pPacket) {
        pPacket.WriteList(objects);
    }

    internal void AddObject(PanelObject panelObject) {
        objects.Add(panelObject);
    }

    internal int ObjectCount() {
        return objects.Count;
    }
}
