# OsPath

Simple type for OS paths!

## Features

1. Works on .NET Framework 4.8 and .NET 10
2. Immutable
3. Divide operator (/) for easy combined

## Examples

```csharp
OsPath logsDir = new("Logs");
OsPath logFile = logsDir / new OsPath("log1.txt");
Console.WriteLine(logFile.Absolute());
```

Or show project `OsPathConsoleExample`.
