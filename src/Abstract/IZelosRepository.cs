using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Documents.Document;
using Soenneker.Dtos.IdNamePair;

namespace Soenneker.Zelos.Repository.Abstract;

/// <summary>
/// A data persistence abstraction layer for Zelos DB
/// </summary>
public interface IZelosRepository<TDocument> where TDocument : Document
{
    ValueTask<IQueryable<T>> BuildQueryable<T>(CancellationToken cancellationToken = default);

    ValueTask<TDocument?> GetItem(string id, CancellationToken cancellationToken = default);

    ValueTask<TDocument?> GetItemByIdNamePair(IdNamePair idNamePair, CancellationToken cancellationToken = default);
}