

using System;
using System.Collections.Generic;

public interface ISaveSerialization<T> {

    public T Save();

    public void Load(T saveData);

    public static List<T> ConvertListToSave<D>(List<D> list) where D : ISaveSerialization<T>{
        List<T> result = new();

        foreach (D item in list) {
            result.Add(item.Save());
        }
        return result;
    }

    public static List<D> ConvertListFromSave<D>(List<T> list, Type classType) where D : ISaveSerialization<T> {
        List<D> result = new();
        foreach (T item in list) {
            D obj = (D)Activator.CreateInstance(classType);
            obj.Load(item);
            result.Add(obj);
        }
        return result;
    }
}

