#if NET
using System.Diagnostics.CodeAnalysis;
#endif
using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization.Metadata;

namespace GameVars;

/// <summary>
/// A collection of variables that can be saved to JSON.
/// </summary>
/// <remarks>
/// Note: Collections are <b>NOT</b> thread-safe.
/// </remarks>
public sealed class GameVarCollection : IReadOnlyCollection<KeyValuePair<string, JsonNode?>> {
    /// <summary>
    /// Invoked whenever a game var in this collection is changed.<br/>
    /// Note: The value may be the same as before.
    /// </summary>
    public event OnGameVarChangedEventHandler? OnGameVarChanged;
    /// <summary>
    /// A handler for <see cref="OnGameVarChanged"/>.
    /// </summary>
    public delegate void OnGameVarChangedEventHandler(string GameVarName);

    private JsonObject GameVars = [];

    /// <summary>
    /// Default <see cref="JsonSerializerOptions"/>.
    /// </summary>
    internal static JsonSerializerOptions DefaultJsonOptions {
#if NET
        [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
        [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
#endif
        get => field ??= new() {
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals | JsonNumberHandling.AllowReadingFromString,
            AllowTrailingCommas = true,
            IncludeFields = true,
            NewLine = "\n",
            ReadCommentHandling = JsonCommentHandling.Skip,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };
    }

    /// <summary>
    /// Gets the number of game vars in the collection.
    /// </summary>
    public int Count => GameVars.Count;

    /// <summary>
    /// Creates a collection of variables which is empty.
    /// </summary>
    public GameVarCollection() {
    }
    /// <summary>
    /// Creates a collection of variables with the given initial variables.
    /// </summary>
    public GameVarCollection(JsonObject GameVars) {
        SetGameVars(GameVars);
    }
    /// <summary>
    /// Returns a copy of the game vars in this collection.
    /// </summary>
    public JsonObject GetGameVars() {
        return (JsonObject)GameVars.DeepClone();
    }
    /// <summary>
    /// Replaces the game vars in this collection with the given game vars.
    /// </summary>
    public void SetGameVars(JsonObject GameVars) {
        HashSet<string> ChangedGameVarNames = [
            .. this.GameVars.Select(GameVar => GameVar.Key),
            .. GameVars.Select(GameVar => GameVar.Key),
        ];

        this.GameVars = (JsonObject)GameVars.DeepClone();

        foreach (string ChangedGameVarName in ChangedGameVarNames) {
            OnGameVarChanged?.Invoke(ChangedGameVarName);
        }
    }
    /// <summary>
    /// Gets the value of the given game var, or a value created from <paramref name="DefaultValueFactory"/>.
    /// </summary>
    public JsonNode? GetGameVar(string GameVarName, Func<JsonNode?> DefaultValueFactory) {
        if (GameVars.TryGetPropertyValue(GameVarName, out JsonNode? Value)) {
            return Value;
        }
        return DefaultValueFactory();
    }
    /// <summary>
    /// Gets the value of the given game var, or a value created from <paramref name="DefaultValueFactory"/>.
    /// </summary>
#if NET
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
#endif
    public T GetGameVar<T>(string GameVarName, Func<T> DefaultValueFactory) {
        if (GameVars.TryGetPropertyValue(GameVarName, out JsonNode? Value)) {
            return JsonSerializer.Deserialize<T>(Value, DefaultJsonOptions)!;
        }
        return DefaultValueFactory();
    }
    /// <summary>
    /// Gets the value of the given game var, or a value created from <paramref name="DefaultValueFactory"/>.
    /// </summary>
    public T GetGameVar<T>(string GameVarName, Func<T> DefaultValueFactory, JsonTypeInfo<T> TypeInfo) {
        if (GameVars.TryGetPropertyValue(GameVarName, out JsonNode? Value)) {
            return JsonSerializer.Deserialize(Value, TypeInfo)!;
        }
        return DefaultValueFactory();
    }
    /// <summary>
    /// Gets the value of the given game var, or <paramref name="DefaultValue"/>.
    /// </summary>
    public JsonNode? GetGameVar(string GameVarName, JsonNode? DefaultValue) {
        if (GameVars.TryGetPropertyValue(GameVarName, out JsonNode? Value)) {
            return Value;
        }
        return DefaultValue;
    }
    /// <summary>
    /// Gets the value of the given game var, or <paramref name="DefaultValue"/>.
    /// </summary>
#if NET
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
#endif
    public T GetGameVar<T>(string GameVarName, T DefaultValue) {
        if (GameVars.TryGetPropertyValue(GameVarName, out JsonNode? Value)) {
            return JsonSerializer.Deserialize<T>(Value, DefaultJsonOptions)!;
        }
        return DefaultValue;
    }
    /// <summary>
    /// Gets the value of the given game var, or <paramref name="DefaultValue"/>.
    /// </summary>
    public T GetGameVar<T>(string GameVarName, T DefaultValue, JsonTypeInfo<T> TypeInfo) {
        if (GameVars.TryGetPropertyValue(GameVarName, out JsonNode? Value)) {
            return JsonSerializer.Deserialize(Value, TypeInfo)!;
        }
        return DefaultValue;
    }
    /// <summary>
    /// Sets the value of the given game var.
    /// </summary>
    public void SetGameVar(string GameVarName, JsonNode? Value) {
        GameVars[GameVarName] = Value;

        OnGameVarChanged?.Invoke(GameVarName);
    }
    /// <summary>
    /// Sets the value of the given game var.
    /// </summary>
#if NET
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
#endif
    public void SetGameVar<T>(string GameVarName, T Value) {
        JsonNode? NodeValue = JsonSerializer.SerializeToNode(Value, DefaultJsonOptions);
        SetGameVar(GameVarName, NodeValue);
    }
    /// <summary>
    /// Sets the value of the given game var.
    /// </summary>
    public void SetGameVar<T>(string GameVarName, T Value, JsonTypeInfo<T> TypeInfo) {
        JsonNode? NodeValue = JsonSerializer.SerializeToNode(Value, TypeInfo);
        SetGameVar(GameVarName, NodeValue);
    }
    /// <summary>
    /// Returns an enumerator that enumerates through the game vars in the collection.
    /// </summary>
    public IEnumerator<KeyValuePair<string, JsonNode?>> GetEnumerator() {
        return GameVars.GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}