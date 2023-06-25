namespace JSON;

public class JsonValue
{
}

public class JsonObject : JsonValue
{
    public IDictionary<JsonString, JsonValue> Pairs { get; }
}

public class JsonArray : JsonValue
{
    public IEnumerable<JsonValue> Values { get; }
}

public class JsonBoolean : JsonValue
{
    public bool TruthValue { get; }
}

public class JsonNumber : JsonValue
{
    public int Number { get; }
}

public class JsonString : JsonValue
{
    public string CharSequence { get; }
}

public class JsonNull : JsonValue
{
}