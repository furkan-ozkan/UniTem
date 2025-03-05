using UnityEngine;
using SimpleJSON;

public static class ActionContextUtility
{
    /// <summary>
    /// ActionContext nesnesini JSON string'e çevirir.
    /// </summary>
    public static string ConvertContextToJson(ActionContext context)
    {
        if (context == null) return "{}";

        JSONNode json = new JSONObject();
        json["ActionPlayer"] = context.ActionPlayer != null ? context.ActionPlayer.ToString() : "";
        json["ActionInteractable"] = context.ActionInteractable != null ? context.ActionInteractable.ToString() : "";
        json["UsedItem"] = context.UsedItem != null ? context.UsedItem.ToString() : "";

        // Parametreleri JSON objesine ekle
        JSONNode parametersNode = new JSONObject();
        foreach (var param in context.Parameters)
        {
            parametersNode[param.Key] = param.Value.ToString();
        }
        json["Parameters"] = parametersNode;

        return json.ToString();
    }

    public static ActionContext ConvertJsonToContext(string jsonString)
    {
        if (string.IsNullOrEmpty(jsonString))
        {
            Debug.LogWarning("ConvertJsonToContext: Received an empty or null JSON string.");
            return new ActionContext();
        }

        JSONNode json = JSON.Parse(jsonString);
        ActionContext context = new ActionContext();

        // JSON verisinden primitive türleri al ve dönüştür
        context.ActionPlayer = json.HasKey("ActionPlayer") ? json["ActionPlayer"].AsInt : -1;
        context.ActionInteractable = json.HasKey("ActionInteractable") ? json["ActionInteractable"].AsInt : -1;
        context.UsedItem = json.HasKey("UsedItem") ? json["UsedItem"].AsInt : -1;

        // Parametreleri deserialize et
        if (json.HasKey("Parameters"))
        {
            JSONNode parametersNode = json["Parameters"];
            foreach (var key in parametersNode.Keys)
            {
                context.SetParameter(key, parametersNode[key].Value);
            }
        }

        return context;
    }

}