using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Documents.Document;
using Soenneker.Dtos.IdNamePair;

namespace Soenneker.Zelos.Repository.Abstract;

/// <summary>
/// A data persistence abstraction layer for Zelos DB
/// </summary>
/// <typeparam name="TDocument">The type of document being handled.</typeparam>
public interface IZelosRepository<TDocument> where TDocument : Document
{
    /// <summary>
    /// Builds a queryable collection of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of elements in the queryable collection.</typeparam>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A queryable collection of type <typeparamref name="T"/>.</returns>
    ValueTask<IQueryable<T>> BuildQueryable<T>(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an item by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the item.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>The requested document or null if not found.</returns>
    ValueTask<TDocument?> GetItem(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an item using an IdNamePair.
    /// </summary>
    /// <param name="idNamePair">The IdNamePair containing the item's ID.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>The requested document or null if not found.</returns>
    ValueTask<TDocument?> GetItemByIdNamePair(IdNamePair idNamePair, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new document to the repository.
    /// </summary>
    /// <param name="document">The document to add.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>The ID of the added document.</returns>
    ValueTask<string> AddItem(TDocument document, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds multiple documents to the repository.
    /// </summary>
    /// <param name="documents">The list of documents to add.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>The list of added documents.</returns>
    ValueTask<List<TDocument>> AddItems(List<TDocument> documents, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing document in the repository.
    /// </summary>
    /// <param name="document">The document to update.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>The ID of the updated document.</returns>
    ValueTask<string> UpdateItem(TDocument document, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates multiple documents in the repository.
    /// </summary>
    /// <param name="documents">The list of documents to update.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>The list of updated documents.</returns>
    ValueTask<List<TDocument>> UpdateItems(List<TDocument> documents, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a document from the repository by its ID.
    /// </summary>
    /// <param name="id">The ID of the document to delete.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    ValueTask DeleteItem(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes all documents from the repository.
    /// </summary>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    ValueTask DeleteAll(CancellationToken cancellationToken = default);
}