using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class JsonDataObject
{
    public string name { get; set; }
}

public class JsonDataObjectContainer<DataObject>
{
    public List<DataObject> dataObjects { get; set; }
}

public class JsonDataLookup<D> where D : JsonDataObject
{
    private Dictionary<string, D> _table;

    public JsonDataLookup(string jsonDataPath)
    {
        _table = new Dictionary<string, D>();
        string jsonString = File.ReadAllText(jsonDataPath);
        JsonDataObjectContainer<D> jsonData = JsonConvert.DeserializeObject<JsonDataObjectContainer<D>>(jsonString);
        foreach (D dataObject in jsonData.dataObjects)
        {
            _table[dataObject.name] = dataObject;
        }
    }

    private D GetDataObject(string objName)
    {
        if (_table.ContainsKey(objName)) {
            return _table[objName];
        }
        else return null;
    }
    public D this[string objName]
    {
        get => GetDataObject(objName);
    }

    public Dictionary<string, D> GetTable()
    {
        return _table;
    }
}
