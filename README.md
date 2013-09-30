Option
======

What is it?
-----------

An Option type for C#. Options are a well studied Functional paradigm that allows for the representation of not having a value (None) as well as having a value (Some). This allows for a clear differentiation without the need for null values.

How can I get it?
-----------------

Option is available as a NuGet package: https://www.nuget.org/packages/Option

```
PM> Install-Package Option
```

Why was it made?
----------------

Option types are missing as a built in for C#, and while present and excellent in F# using F# options in C# is cumbersome. There are decent solutions which use extension methods or wrapper classes, but these solutions are not ideal because:

1. You need to include the FSharp.Core runtime dll in your project
2. The representation of FSharpOption<T>.None is actually `null`. The F# language has build in compiler constructs to handle this, but in C# whatever interop library you're using must take care of it.

### Decompiled Source for FSharpOption<T>.None ###

```csharp
/// <summary>
/// Create an option value that is a 'None' value.
/// </summary>
[CompilerGenerated]
[DebuggerNonUserCode]
[DebuggerBrowsable(DebuggerBrowsableState.Never)]
public static FSharpOption<T> None
{
    [CompilationMapping(SourceConstructFlags.UnionCase, 0)] get
    {
        return (FSharpOption<T>) null;
    }
}
```

Example Usage
-------------

```csharp
public void Example()
{
    Option<int> option = 1;
    
    if (option.HasValue)
    {
        // Do something with the value
        Console.WriteLine(option.Value);
    }
    
    var s = matchOptionExample(1); // s = "One"
}

public string matchOptionExample(Option<int> option)
{
    return option.Match<string>()
        .None(() => "No value")
        .Some(1, () => "One")
        .Some((value) => value.ToString())
        .Result();
}
```

Road Map
--------

* Interop support with FSharpOption<T>
