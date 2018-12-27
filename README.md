# NullFight

## Overview
Fights the good fight against NullReferenceException in .NET using an *Option* struct and a *Result* struct. Although very not needed in F#, useful for c#, and maybe vb (haven't looked into how the api would look here).



## Result

This struct is ***roughly modeled*** after the ideas that the Rust languages Result has.

Result is used to specify a value or an error returned from a Service method. Something may have gone wrong during that process and rather than throw the error at that point, we can instead, throw that in our API/Application layer in a more friendly way or ignore the error if it makes sense. Services can also more consistenly handle the errors of other services returning these result objects.

### Result Examples
***The following code and comments also exist as a unit test in the [ResultTests.cs](/NullFight.Tests/ResultsTests.cs) file.***

Use the static methods to create results
```csharp
    using NullFight.Exceptions;
    using static NullFight.FunctionalExtensions;

    Result<string> stringResult = ResultValue("My String");
```


 You can get the inner value by calling Expect with a message to throw in the
 exception if instead the result had an error in it. In this case it has a string.

```csharp
    Assert.AreEqual("My String", stringResult.Expect("Could not get the string out"));
```

You can also retrieve the inner value by calling Unwrap. THIS THROWS AN ERROR IF THERE
WAS AN EXCEPTION IN IT. You should check first using HasValue or use expect to give a better error message.

```csharp
    Assert.AreEqual("My String", stringResult.Unwrap());
```

Proper way to use Unwrap() making sure we check with HasValue first

```csharp
    if (stringResult.HasValue)
    {
        // ok to do this here because we have checked
        string theString3 = stringResult.Unwrap();
    }
    else
    {
        // won't hit this code since our result has a value.
        throw stringResult.UnwrapException();
    }

    Result<object> errorResult = ResultError(new Exception("Something Failed"));
```

Unwrap throws an exception here since this result object has an error.

```csharp
    Assert.ThrowsException<ResultException>(() => errorResult.Unwrap());
```

Expect throws an exception here since this result object has an error.

```csharp
    Assert.ThrowsException<FriendlyResultException>(() => errorResult.Expect("My More Detailed Error"));
```
A result can be mapped to another type using the MapValue function. If it is a ResultError the MapValue function is not run.

```csharp
    Result<int> intResult = stringResult.MapValue(x => x.Length);
    Assert.AreEqual(intResult.Unwrap(), "My String".Length);
```

This can also be done on an error result and it will pass through the error to the new result

```csharp
    Result<int> intResultWithError = errorResult.MapValue(x => default(int));
    Assert.ThrowsException<ResultException>(() => intResultWithError.Unwrap());
```

Let's go over some of the extension methods if you are returning Task<Result<T>>

```csharp
    Task<Result<string>> stringResultTask = Task.FromResult(ResultValue("My String"));
```

There are extension methods that operate on a task allowing the value returned after an await
to be the inner value instead of the result. Allowing this to occur on one line.

```csharp
    string theString4 = await stringResultTask.Expect("My More Detailed Error");
    Assert.AreEqual(theString4, "My String");
```

Doing this without the extension method

```csharp
    Result<string> stringResult2 = (await stringResultTask);
    string theString5 = stringResult2.Expect("My More Detailed Error");
    Assert.AreEqual(theString5, "My String");
```

There are many tools that improve writing methods that return a Result Take a look at the method code below

```csharp
    Result<int> intResult1 = MethodReturningResult();
```

We can also use c#7 type matching with a result by calling GetValueOrError which returns either item cast to an object for
pattern matching.

```csharp
    switch (intResult1.GetValueOrError())
    {
        case int myInt:
            // Shouldn't hit this
            Assert.IsTrue(false);
            break;
        case Exception ex:
            Assert.IsTrue(true);
            break;
    }
```
