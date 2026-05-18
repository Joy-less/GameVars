namespace GameVars;

/// <summary>
/// A saveable collection of variables.
/// </summary>
/// <remarks>
/// Note: Collections are <b>NOT</b> thread-safe.
/// </remarks>
public sealed class GameVarCollection {
    /// <summary>
    /// Invoked whenever a game var in this collection is changed.<br/>
    /// Note: The value may be the same as before.
    /// </summary>
    public event OnGameVarChangedEventHandler? OnGameVarChanged;
    /// <summary>
    /// A handler for <see cref="OnGameVarChanged"/>.
    /// </summary>
    public delegate void OnGameVarChangedEventHandler(string GameVarName);

    private Dictionary<string, object?> GameVars = [];

    /// <summary>
    /// Creates a new saveable collection of variables which is empty.
    /// </summary>
    public GameVarCollection() {
    }
    /// <summary>
    /// Creates a new saveable collection of variables with the given initial variables.
    /// </summary>
    public GameVarCollection(IReadOnlyDictionary<string, object?> GameVars) {
        SetGameVars(GameVars);
    }
    /// <summary>
    /// Returns a read-only interface over the game vars in this collection.
    /// </summary>
    public IReadOnlyDictionary<string, object?> GetGameVars() {
        return GameVars;
    }
    /// <summary>
    /// Replaces the game vars in this collection with the given game vars.
    /// </summary>
    public void SetGameVars(IReadOnlyDictionary<string, object?> GameVars) {
        HashSet<string> ChangedGameVarNames = [
            .. this.GameVars.Select(GameVar => GameVar.Key),
            .. GameVars.Select(GameVar => GameVar.Key),
        ];

#if NET
        this.GameVars = GameVars.ToDictionary();
#else
        this.GameVars = new Dictionary<string, object?>(GameVars.Count);
        foreach (KeyValuePair<string, object?> KeyValuePair in GameVars) {
            this.GameVars[KeyValuePair.Key] = KeyValuePair.Value;
        }
#endif

        foreach (string ChangedGameVarName in ChangedGameVarNames) {
            OnGameVarChanged?.Invoke(ChangedGameVarName);
        }
    }
    /// <summary>
    /// Gets the value of the given game var, or a value created from <paramref name="DefaultValueFactory"/>.
    /// </summary>
    public T GetGameVar<T>(string GameVarName, Func<T> DefaultValueFactory) {
        if (GameVars.TryGetValue(GameVarName, out object? Value)) {
            return (T)Value!;
        }
        return DefaultValueFactory();
    }
    /// <summary>
    /// Gets the value of the given game var, or <paramref name="DefaultValue"/>.
    /// </summary>
    public T GetGameVar<T>(string GameVarName, T DefaultValue) {
        if (GameVars.TryGetValue(GameVarName, out object? Value)) {
            return (T)Value!;
        }
        return DefaultValue;
    }
    /// <summary>
    /// Sets the value of the given game var.
    /// </summary>
    public void SetGameVar(string GameVarName, object? Value) {
        GameVars[GameVarName] = Value;

        OnGameVarChanged?.Invoke(GameVarName);
    }
}