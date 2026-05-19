# GameVars

[![NuGet](https://img.shields.io/nuget/v/GameVars.svg)](https://www.nuget.org/packages/GameVars)

Collections of variables that can be saved to JSON.

## Usage

```cs
GameVarCollection Collection = new();
Collection.Set("cat", "meow");
Collection.Set("dog", "woof");
Collection.Set("animals", 2);
JsonSerializer.Serialize(Collection.GetAll()).ShouldBe(JsonSerializer.Serialize(new JsonObject() {
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
Collection.Get("bird", () => "").ShouldBe("chirp");
Animals.Increment();
Animals.Get().ShouldBe(3);

GameVar<HashSet<int>> Numbers = new(Collection, "numbers", () => [1, 2, 3]);
Numbers.Get().ShouldBe([1, 2, 3]);
Numbers.Set([1]);
Numbers.Get().ShouldBe([1]);
```