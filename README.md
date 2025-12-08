# Scaffer CLI

Universal cross-language scaffolding generator — simple, fast, and framework-agnostic.



---

## ✨ What is Scaffer?

**Scaffer** is a universal scaffolding generator that uses simple `.scaff` template files  
to create files for **any language or framework** (C#, HTML, JS, React, Python, etc.).

It includes:

- 🛠 A powerful CLI (`scaff`)
- 📦 Simple scaffold templates
- 🔧 Parameter injection
- 🧩 Optional parameters with fallback
- 🚀 Cross-platform support (Windows, Linux, macOS)

---

## Installation

Install globally from NuGet:

```bash
dotnet tool install -g Scaffer
 ```
## Update

```bash
dotnet tool update -g Scaffer
```

## Uninstall

```bash
dotnet tool uninstall -g Scaffer
```

## Syntax


Define metadata

```bash
@Meta(

  Extension: ".html"
  Route: "./"
  Name: "index"
)
```

`Route`, and `Name` are optional values and they can be set in while generating the file with the `--scaff-route` and `--scaff-out` respectively.
By default `./` is set to the route and a random `GUID` for the `Name`.

Define the params

```bash

@Params(

  name??
)

```

Params are by default optional but you can add a fallback to them.

```bash
....
 name??Jhon
 ....
```

Use your params values in the `@Template` directive

```bash
@Template(
 <p>{{value:name}}</p>
)
```


## 📦 `.scaff` file example

`test.scaff`

```csharp

    
@Meta(
    Extension: ".cs"
)

@Params(
    name??ExampleService
    type??string
    initial??"Hello World"
    log??
)

@Template(`
using System;

namespace MyApp.Services
{
    public class {{value:name}}
    {
        public {{value:type}} Data { get; set; } = {{value:initial}};

        public void Log()
        {
            Console.WriteLine({{value:log}});
        }
    }
}
`)

```



build command (make sure to be on the `.scaff` file directory)

```bash
scaff --scaff-build test 
    --scaff-param name=UserService 
    --scaff-param type=string 
    --scaff-param initial="John Doe"
    --scaff-param log="Data"
```

The output

```csharp
using System;

namespace MyApp.Services
{
    public class UserService
    {
        public string Data { get; set; } = "John Doe";
        
        public void Log()
        {
           Console.WriteLine(Data);
        }
    }
}
```

## Show available `.scaff` files

```bash
scaff --scaff-list
```

## Show all commands

```bash
scaff --scaff-help
```

