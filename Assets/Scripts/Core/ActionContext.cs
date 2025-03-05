using System.Collections.Generic;

public class ActionContext
{
    public int ActionPlayer { get; set; }
    public int ActionInteractable { get; set; }
    public int UsedItem { get; set; }
    public Dictionary<string, object> Parameters { get; private set; } = new Dictionary<string, object>();

    public T GetParameter<T>(string key, T defaultValue = default)
    {
        if (Parameters.TryGetValue(key, out object value) && value is T typedValue)
        {
            return typedValue;
        }
        return defaultValue;
    }

    public void SetParameter<T>(string key, T value)
    {
        Parameters[key] = value;
    }
}