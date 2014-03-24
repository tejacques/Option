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
2. The representation of FSharpOption<T>.None is actually `null`. The F# language has built in compiler constructs to handle this, but in C# whatever interop library you're using must take care of it.

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

### Using the value ###

```csharp
public void Example1()
{
    Option<int> option = 1;
    
    // Most Performant
    int value;
    if (option.TryGetValue(out value))
    {
        // Do something with the value
        Console.WriteLine(value);
    }
    else
    {
        // Do something else
    }
}
```

```csharp
public void Example2()
{
    Option<int> option = 1;
    
    // Second Most performant
    if (option.HasValue)
    {
        // Do something with the value
        Console.WriteLine(option.Value);
    }
    else
    {
        // Do something else
    }
}
```

```csharp
public void FunctionalExample1()
{
    Option<int> option = 1;
    
    // Third most performant
    option.Match(
        None: () => { /* Do nothing */ }
        Some: value =>
        {
            //Do something with the value
            Console.WriteLine(value);
        });
}
```

``` csharp
public void FunctionalExample2()
{
    Option<int> option = 1;
    
    // Least performant but can match on values
    option.Match()
        .None(() => Console.WriteLine("No value"))
        .Some(1, () => Console.WriteLine("One"))
        .Some((value) => Console.WriteLine(value))
        .Result();
}
```

### Getting the value ###

```csharp
public void GetExample()
{
    Option<int> option = 1;
    
    // Getting the value out
    var v = option.ValueOrDefault;
    var v2 = option.ValueOr(0);
    var v3 = option.ValueOr(() => 0);
}
```

```csharp
public void FunctionalGetExample1()
{
    Option<int> option = 1;
    
    // More performant, but can't match on exact values
    var match1 = option.Match(
        None: () => "No value"
        Some: (value) => value.ToString());
}
```

```csharp
public void FunctionalGetExample2()
{
    // Less performant but can match on values
    var match2 = option.Match<string>()
        .None(() => "No value")
        .Some(1, () => "One")
        .Some((value) => value.ToString())
        .Result();
}
```

Road Map
--------

* Interop support with FSharpOption<T>
