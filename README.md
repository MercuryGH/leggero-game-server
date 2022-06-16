# Leggero Game Server

## Environment

* Ubuntu 20.04 (x86-64)
* .NET 6.0.201

## Build the project

First add dependency of MySQL Connector in the simplest way:

```
dotnet add package MySql.Data 
```

Then you can Build debug:
```
dotnet build
```

or Build release:

```
dotnet build --configuration Release
```
