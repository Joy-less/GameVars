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
public sealed class GameVarCollection : ICollection<KeyValuePair<string, JsonNode?>>, ICollection, IReadOnlyCollection<KeyValuePair<string, JsonNode?>> {
    /// <summary>
    /// Invoked whenever a game var in this collection is changed.<br/>
    /// Note: The value may be the same as before.
    /// </summary>
    public event OnGameVarChangedEventHandler? OnGameVarChanged;
    /// <summary>
    /// A handler for <see cref="OnGameVarChanged"/>.
    /// </summary>
    public delegate void OnGameVarChangedEventHandler(string GameVarName);

    private JsonObject GameVars;

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
        GameVars = [];
    }
    /// <summary>
    /// Creates a collection of variables with the given initial variables.
    /// </summary>
    public GameVarCollection(JsonObject GameVars) {
        this.GameVars = GameVars;
    }
    /// <summary>
    /// Creates a collection of variables with initial variables copied from this collection.
    /// </summary>
    public GameVarCollection Clone() {
        return new GameVarCollection((JsonObject)GameVars.DeepClone());
    }
    /// <summary>
    /// Returns the game vars in this collection.
    /// </summary>
    public JsonObject GetAll() {
        return GameVars;
    }
    /// <summary>
    /// Replaces the game vars in this collection with the given game vars.
    /// </summary>
    public void SetAll(JsonObject GameVars) {
        HashSet<string> ChangedGameVarNames = [
            .. this.GameVars.Select(GameVar => GameVar.Key),
            .. GameVars.Select(GameVar => GameVar.Key),
        ];

        this.GameVars = GameVars;

        foreach (string ChangedGameVarName in ChangedGameVarNames) {
            OnGameVarChanged?.Invoke(ChangedGameVarName);
        }
    }
    /// <summary>
    /// Clears all the game vars in this collection.
    /// </summary>
    public void Clear() {
        HashSet<string> ChangedGameVarNames = [
            .. GameVars.Select(GameVar => GameVar.Key),
        ];

        GameVars.Clear();

        foreach (string ChangedGameVarName in ChangedGameVarNames) {
            OnGameVarChanged?.Invoke(ChangedGameVarName);
        }
    }
    /// <summary>
    /// Gets the value of the given game var, or a value created from <paramref name="DefaultValueFactory"/>.
    /// </summary>
    public JsonNode? Get(string GameVarName, Func<JsonNode?> DefaultValueFactory) {
        if (GameVars.TryGetPropertyValue(GameVarName, out JsonNode? Value)) {
            return Value;
        }
        return DefaultValueFactory();
    }
    /// <summary>
    /// Gets the value of the given game var, or <paramref name="DefaultValue"/>.
    /// </summary>
    public JsonNode? Get(string GameVarName, JsonNode? DefaultValue) {
        if (GameVars.TryGetPropertyValue(GameVarName, out JsonNode? Value)) {
            return Value;
        }
        return DefaultValue;
    }
    /// <summary>
    /// Gets the value of the given game var, or a value created from <paramref name="DefaultValueFactory"/>.
    /// </summary>
#if NET
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
#endif
    public T Get<T>(string GameVarName, Func<T> DefaultValueFactory, JsonSerializerOptions? JsonOptions = null) {
        if (GameVars.TryGetPropertyValue(GameVarName, out JsonNode? Value)) {
            return JsonSerializer.Deserialize<T>(Value, JsonOptions ?? DefaultJsonOptions)!;
        }
        return DefaultValueFactory();
    }
    /// <summary>
    /// Gets the value of the given game var, or <paramref name="DefaultValue"/>.
    /// </summary>
#if NET
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
#endif
    public T Get<T>(string GameVarName, T DefaultValue, JsonSerializerOptions? JsonOptions = null) {
        if (GameVars.TryGetPropertyValue(GameVarName, out JsonNode? Value)) {
            return JsonSerializer.Deserialize<T>(Value, JsonOptions ?? DefaultJsonOptions)!;
        }
        return DefaultValue;
    }
    /// <summary>
    /// Gets the value of the given game var, or a value created from <paramref name="DefaultValueFactory"/>.
    /// </summary>
    public T Get<T>(string GameVarName, Func<T> DefaultValueFactory, JsonTypeInfo<T> TypeInfo) {
        if (GameVars.TryGetPropertyValue(GameVarName, out JsonNode? Value)) {
            return JsonSerializer.Deserialize(Value, TypeInfo)!;
        }
        return DefaultValueFactory();
    }
    /// <summary>
    /// Gets the value of the given game var, or <paramref name="DefaultValue"/>.
    /// </summary>
    public T Get<T>(string GameVarName, T DefaultValue, JsonTypeInfo<T> TypeInfo) {
        if (GameVars.TryGetPropertyValue(GameVarName, out JsonNode? Value)) {
            return JsonSerializer.Deserialize(Value, TypeInfo)!;
        }
        return DefaultValue;
    }
    /// <summary>
    /// Sets the value of the given game var.
    /// </summary>
    public void Set(string GameVarName, JsonNode? Value) {
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
    public void Set<T>(string GameVarName, T Value, JsonSerializerOptions? JsonOptions = null) {
        JsonNode? NodeValue = JsonSerializer.SerializeToNode(Value, JsonOptions ?? DefaultJsonOptions);
        Set(GameVarName, NodeValue);
    }
    /// <summary>
    /// Sets the value of the given game var.
    /// </summary>
    public void Set<T>(string GameVarName, T Value, JsonTypeInfo<T> TypeInfo) {
        JsonNode? NodeValue = JsonSerializer.SerializeToNode(Value, TypeInfo);
        Set(GameVarName, NodeValue);
    }
    /// <summary>
    /// Removes the given game var from the collection.
    /// </summary>
    public void Remove(string GameVarName) {
        GameVars.Remove(GameVarName);

        OnGameVarChanged?.Invoke(GameVarName);
    }
    /// <summary>
    /// Returns whether this collection has a value for the given game var.
    /// </summary>
    public bool Contains(string GameVarName) {
        return GameVars.ContainsKey(GameVarName);
    }
    /// <summary>
    /// Returns an enumerator that enumerates through the game vars in the collection.
    /// </summary>
    public IEnumerator<KeyValuePair<string, JsonNode?>> GetEnumerator() {
        return GameVars.GetEnumerator();
    }
    /// <summary>
    /// Converts the collection of game vars to a string containing a JSON object.
    /// </summary>
#if NET
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
#endif
    public string Serialize(JsonSerializerOptions? JsonOptions = null) {
        return JsonSerializer.Serialize(GameVars, JsonOptions ?? DefaultJsonOptions);
    }
    /// <summary>
    /// Converts the collection of game vars to a string containing a JSON object.
    /// </summary>
    public string Serialize(JsonTypeInfo<JsonObject> TypeInfo) {
        return JsonSerializer.Serialize(GameVars, TypeInfo);
    }

    /// <summary>
    /// Converts a string containing a JSON object to a collection of game vars.
    /// </summary>
#if NET
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
    [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed.")]
#endif
    public static GameVarCollection Deserialize(scoped ReadOnlySpan<char> Input, JsonSerializerOptions? JsonOptions = null) {
        return new GameVarCollection(JsonSerializer.Deserialize<JsonObject>(Input, JsonOptions ?? DefaultJsonOptions) ?? []);
    }
    /// <summary>
    /// Converts a string containing a JSON object to a collection of game vars.
    /// </summary>
    public static GameVarCollection Deserialize(scoped ReadOnlySpan<char> Input, JsonTypeInfo<JsonObject> TypeInfo) {
        return new GameVarCollection(JsonSerializer.Deserialize(Input, TypeInfo) ?? []);
    }

    /// <inheritdoc/>
    bool ICollection<KeyValuePair<string, JsonNode?>>.IsReadOnly => false;
    /// <inheritdoc/>
    bool ICollection.IsSynchronized => false;
    /// <inheritdoc/>
    object ICollection.SyncRoot => this;

    /// <inheritdoc/>
    void ICollection<KeyValuePair<string, JsonNode?>>.Add(KeyValuePair<string, JsonNode?> Item) => Set(Item.Key, Item.Value);
    /// <inheritdoc/>
    void ICollection<KeyValuePair<string, JsonNode?>>.Clear() => Clear();
    /// <inheritdoc/>
    bool ICollection<KeyValuePair<string, JsonNode?>>.Contains(KeyValuePair<string, JsonNode?> Item) => GameVars.Contains(Item);
    /// <inheritdoc/>
    void ICollection<KeyValuePair<string, JsonNode?>>.CopyTo(KeyValuePair<string, JsonNode?>[] Array, int ArrayIndex) => GameVars.ToArray().CopyTo(Array, ArrayIndex);
    /// <inheritdoc/>
    bool ICollection<KeyValuePair<string, JsonNode?>>.Remove(KeyValuePair<string, JsonNode?> Item) {
        if (GameVars.Contains(Item)) {
            Remove(Item.Key);
            return true;
        }
        return false;
    }
    /// <inheritdoc/>
    void ICollection.CopyTo(Array Array, int ArrayIndex) => GameVars.ToArray().CopyTo(Array, ArrayIndex);
    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}