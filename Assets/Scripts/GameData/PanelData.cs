

using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PanelData : ISaveSerialization<PanelSaveData> {
   
    List<PanelObject> objects = new();

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

    public void AddObject(PanelObject panelObject) {
        objects.Add(panelObject);
    }

    public int ObjectCount() {
        return objects.Count;
    }

    public PanelSaveData Save() {
        PanelSaveData save = new();
        save.objects = new List<PanelObjectWrapper>();

        foreach (PanelObject obj in objects) {

            PanelObjectWrapper wrapper = new PanelObjectWrapper();

            if (obj is TextPanelObject textObj) {
                wrapper.type = "text";
                wrapper.data = textObj.Save();
            } else if (obj is ImagePanelObject imgObj) {
                wrapper.type = "image";
                wrapper.data = imgObj.Save();
            } else if (obj is URLPanelObject urlObj) {
                wrapper.type = "url";
                wrapper.data = urlObj.Save();
            } else throw new Exception("PanelObject saving is unhandled: " + wrapper.type);

            save.objects.Add(wrapper);
        }
        return save;
    }



        public void Load(PanelSaveData saveData) {
        objects = new List<PanelObject>();

        foreach (var wrapper in saveData.objects) {

            switch (wrapper.type) {

                case "text":
                    TextPanelObject textObj = new();
                    textObj.Load(wrapper.data);
                    objects.Add(textObj);
                    break;
                case "image":
                    ImagePanelObject imgObj = new();
                    imgObj.Load(wrapper.data);
                    objects.Add(imgObj);
                    break;
                case "url":
                    URLPanelObject urlObj = new();
                    urlObj.Load(wrapper.data);
                    objects.Add(urlObj);
                    break;
                default: throw new Exception("PanelObject loading is unhandled: " + wrapper.type);
            }
        }
        


    }
}

[Serializable]
public struct PanelSaveData {
    public List<PanelObjectWrapper> objects;
}

[Serializable]
public struct PanelObjectWrapper {
    public string type;
    public PanelObjectSaveData data;
}
