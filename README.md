# Scaffer

![GitHub release (latest by date)](https://img.shields.io/github/v/release/fkabreiddy/Scaffer)
![Platform](https://img.shields.io/badge/platform-win-blue)

**Universal Scaffolding Generator CLI.** Scaffer is a command-line tool designed to generate project structures and files quickly and efficiently.

> **Note:** Currently, official support and testing are focused on **Windows**. macOS and Linux versions are considered experimental.

---

## 📦 Installation

The recommended way to install Scaffer on Windows is via **Scoop**.

### 🪟 Via Scoop (Recommended)

If you already have [Scoop](https://scoop.sh/) installed, simply run these two commands in PowerShell:

```powershell
# 1. Add the official bucket
scoop bucket add Scaffer https://github.com/fkabreiddy/scaffer-scoop-bucket

# 2. Install the tool
scoop install scaffer

#test

scaffer --scaff-help

#update
scoop update // to update the buckets
scoop update scaffer
 ```


## 🛠 Manual Installation
If you prefer not to use package managers:

Go to the Releases section.

Download the .zip file for Windows (scaffer-win-x64...).

Unzip it into a safe folder.

(optional) rename the Scaffer.CLI.exe into scaffer.exe so you can call `scaffer --scaff-build ...` instead of `Scaffer.CLI.exe --scaff-build...`

Add that folder to your Environment Variables (PATH) to use the scaffer command from any terminal.




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
<<temp
 <p>{{value:name}}</p>
tempend>>
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
scaffer --scaff-build test 
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
scaffer --scaff-list
```

## Show all commands

```bash
scaffer --scaff-help
```

