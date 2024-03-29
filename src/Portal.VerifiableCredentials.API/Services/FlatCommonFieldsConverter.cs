using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Portal.VerifiableCredentials.API.Models;

namespace Portal.VerifiableCredentials.API.Services;

internal class FlatCommonFieldsConverter<THighLevelContract> : JsonConverter where THighLevelContract : WithClaims
{
    [ThreadStatic]
    static bool disabled;

    // Disables the converter in a thread-safe manner.
    bool Disabled { get { return disabled; } set { disabled = value; } }

    public override bool CanWrite { get { return !Disabled; } }

    public override bool CanRead { get { return !Disabled; } }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override bool CanConvert(Type objectType)
    {
        return typeof(THighLevelContract).IsAssignableFrom(objectType);
    }
    
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }
        using (new PushJsonComposedObjectAsValue<bool>(true, () => Disabled, val => Disabled = val))  // Prevent infinite recursion of converters
        {
            var hasCommon = (THighLevelContract)value;
            var commonEvent = hasCommon.Claims;
            if (commonEvent == null)
            {
                throw new Exception("the expected object field (composition)");
            }
            var commonObjEvent = JObject.FromObject(commonEvent, serializer);

            var commonData = hasCommon.Claims;
            if (commonData == null)
            {
                throw new Exception("the expected object field (composition)");
            }
            
            var obj = JObject.FromObject(hasCommon, serializer);
            commonObjEvent.Merge(obj);

            commonObjEvent.WriteTo(writer);
        }
    }
}