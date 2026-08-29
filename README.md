[![](https://img.shields.io/nuget/v/soenneker.zelos.repository.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.zelos.repository/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.zelos.repository/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.zelos.repository/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.zelos.repository.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.zelos.repository/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.zelos.repository/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.zelos.repository/actions/workflows/codeql.yml)

# Soenneker.Zelos.Repository

A data persistence abstraction layer for Zelos DB.

## Install

```bash
dotnet add package Soenneker.Zelos.Repository
```

## Quick start

```csharp
using Soenneker.Zelos.Repository.Abstract;

IZelosRepository<TDocument> zelosRepository = /* resolve from DI */;
var result = await zelosRepository.BuildQueryable(default);
```

Builds a queryable collection of a specified type.

## What you get

- `IZelosRepository<TDocument>` — A data persistence abstraction layer for Zelos DB.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `IZelosRepository<TDocument>.BuildQueryable(cancellationToken)` | Builds a queryable collection of a specified type. | A queryable collection of type `T`. |
| `IZelosRepository<TDocument>.GetItem(id, cancellationToken)` | Retrieves an item by its ID. | The requested document or null if not found. |
| `IZelosRepository<TDocument>.GetItemByIdNamePair(idNamePair, cancellationToken)` | Retrieves an item using an IdNamePair. | The requested document or null if not found. |
| `IZelosRepository<TDocument>.AddItem(document, cancellationToken)` | Adds a new document to the repository. | The ID of the added document. |
| `IZelosRepository<TDocument>.AddItems(documents, cancellationToken)` | Adds multiple documents to the repository. | The list of added documents. |
| `IZelosRepository<TDocument>.UpdateItem(document, cancellationToken)` | Updates an existing document in the repository. | The ID of the updated document. |
| `IZelosRepository<TDocument>.UpdateItems(documents, cancellationToken)` | Updates multiple documents in the repository. | The list of updated documents. |
| `IZelosRepository<TDocument>.DeleteItem(id, cancellationToken)` | Deletes a document from the repository by its ID. | Completes when the requested deletion has finished. |
| `IZelosRepository<TDocument>.DeleteAll(cancellationToken)` | Deletes all documents from the repository. | Completes when the requested deletion has finished. |

## Practical notes

- Cancellation stops pending work; it does not undo work that has already completed.
