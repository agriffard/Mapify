# Mapify

IQueryable extensions.

[![Nuget](https://img.shields.io/nuget/dt/Mapify)](https://www.nuget.org/packages/Mapify)

## Examples

``` csharp
Context.Entity.FindListAsync<EntityModel>(filter, orderBy);
Context.Entity.FindListAsync<EntityModel, int>(x => x.Property == value, x => x.Id);
Context.Entity.FindOneAsync<EntityModel>(filter, orderBy);
Context.Entity.FindOneAsync<EntityModel>(x => x.Property == value);
(List<T> results, int totalCount) Context.Entity.GetPagedListAsync<EntityModel>(pageIndex, pageSize, filter, orderBy);
(List<T> results, int totalCount) Context.Entity.GetPagedListAsync<EntityModel, int>(pageIndex, pageSize, x => x.Property == value, x => x.Id);
```

## Credits

https://github.com/alirezanet/Gridify  
https://github.com/MapsterMapper/Mapster
