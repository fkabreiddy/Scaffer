# Scaffer

**Universal Scaffolding Generator CLI.** Scaffer is a command-line tool designed to generate project structures and files quickly and efficiently.

> **Note:** Currently, official support and testing are focused on **Windows**. macOS and Linux versions are considered experimental.

---

## Installation

The recommended way to install Scaffer on Windows is via **Scoop**.

### Via Scoop (Recommended)

If you already have [Scoop]() installed, simply run these two commands in PowerShell:

```powershell
# 1. Add the official bucket
scoop bucket add Scaffer https://github.com/fkabreiddy/scaffer-scoop-bucket

# 2. Install the tool
scoop install scaffer

# Test the installation
scaffer --help

# Update Scaffer
scoop update # to update the buckets
scoop update scaffer

```

### Manual Installation

If you prefer not to use package managers:

1. Go to the Releases section.
2. Download the `.zip` file for Windows (`scaffer-win-x64...`).
3. Unzip it into a safe folder.
4. (Optional) Rename `Scaffer.CLI.exe` to `scaffer.exe` so you can call `scaffer -b ...` instead of `Scaffer.CLI.exe -b ...`.
5. Add that folder to your Environment Variables (PATH) to use the `scaffer` command from any terminal.

---

## Syntax

### Define the parameters

```text
@params(
  name??
)

```

Params are by default optional, but you can add a fallback (default value) to them using `??`.

```text
@params(
  name??John
)

```

### Use your parameter values in the template directive

Enclose your template logic between `<<temp` and `tempend>>`.

```text
<<temp
 <p>{{value:name}}</p>
tempend>>

```

---

## `.scaff` file example

Create a file named `test.scaff`:

```csharp
@params(
    name??ExampleService
    type??string
    initial??"Hello World"
    log??
)

<<temp
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
tempend>>

```

### Build command

Make sure to be in the same directory as the `.scaff` file, and specify the output name and extension using the `-o` flag:

```bash
scaffer -b test \
    -p name=UserService \
    -p type=string \
    -p initial="John Doe" \
    -p log="Data" \
    -o UserService.cs

```

### The output

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

---

## Commands and Options

Scaffer uses a standard short and long flag syntax.

```text
Usage:
  scaffer [command] [options]

Commands:
  -b, --build <template>       Builds a .scaff template
  -l, --list                   Lists all available .scaff files
  -h, --help                   Shows this help message

Options:
  -r, --route <path>           Output directory
  -o, --out <name>             Output file name
  -p, --param <key=value>      Template parameter (repeatable)

```

### Examples

**Basic build:**

```bash
scaffer -b feature

```

**Build with parameters:**

```bash
scaffer -b feature -p name=User

```

**Advanced build with multiple flags:**

```bash
scaffer -b page \
        -p title=Home \
        -p content="Hello World" \
        -r ./pages \
        -o index.html

```

**Long syntax equivalent:**

```bash
scaffer --build page \
        --param title=Home \
        --route ./output \
        --out index.html

```
