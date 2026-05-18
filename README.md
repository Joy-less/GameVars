# GameVars

[![NuGet](https://img.shields.io/nuget/v/GameVars.svg)](https://www.nuget.org/packages/GameVars)

Collections of variables that can be saved to JSON.

## Usage

```cs
GameVarCollection Collection = new();
Collection.SetGameVar("cat", "meow");
Collection.SetGameVar("dog", "woof");
Collection.SetGameVar("animals", 2);
JsonSerializer.Serialize(Collection.GetGameVars()).ShouldBe(JsonSerializer.Serialize(new JsonObject() {
    ["cat"] = "meow",
    ["dog"] = "woof",
    ["animals"] = 2,
}));

GameVar<string> Cat = new(Collection, "cat", () => "");
Cat.Get().ShouldBe("meow");
Cat.Set("nyan");
Cat.Get().ShouldBe("nyan");

GameVarSlim<int> Animals = new(Collection, "animals", () => 0);
Animals.Get().ShouldBe(2);

GameVar<string> Bird = new(Collection, "bird", () => "");
Bird.Set("chirp");
Collection.GetGameVar("bird", () => "").ShouldBe("chirp");
Animals.Increment();
Animals.Get().ShouldBe(3);
```