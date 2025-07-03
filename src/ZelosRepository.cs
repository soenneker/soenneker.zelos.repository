using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Soenneker.Documents.Document;
using Soenneker.Dtos.IdNamePair;
using Soenneker.Enums.JsonOptions;
using Soenneker.Extensions.Enumerable;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.Json;
using Soenneker.Utils.Method;
using Soenneker.Zelos.Abstract;
using Soenneker.Zelos.Container.Util.Abstract;
using Soenneker.Zelos.Repository.Abstract;

namespace Soenneker.Zelos.Repository;

///<inheritdoc cref="IZelosRepository{TDocument}"/>
public class ZelosRepository<TDocument> : IZelosRepository<TDocument> where TDocument : Document
{
    private readonly IZelosContainerUtil _zelosContainerUtil;

    protected ILogger<ZelosRepository<TDocument>> Logger { get; }

    private readonly bool _log;

    protected string ContainerName { get; set; }

    protected string DatabaseFilePath { get; set; }

    public ZelosRepository(IConfiguration config, ILogger<ZelosRepository<TDocument>> logger, IZelosContainerUtil zelosContainerUtil)
    {
        _zelosContainerUtil = zelosContainerUtil;
        Logger = logger;

        _log = config.GetValue<bool>("Zelos:Log");
    }

    public async ValueTask<IQueryable<T>> BuildQueryable<T>(CancellationToken cancellationToken = default)
    {
        IZelosContainer container = await _zelosContainerUtil.Get(DatabaseFilePath, ContainerName, cancellationToken).NoSync();

        return container.BuildQueryable<T>();
    }

    public List<T> GetItems<T>(IQueryable<T> queryable)
    {
        if (_log)
            Logger.LogDebug("-- ZELOS: {method} ({type})", MethodUtil.Get(), typeof(T).Name);

        // We're just materializing it here because the IQueryable has already been built via the container
        return queryable.ToList();
    }

    public async ValueTask<TDocument?> GetItem(string id, CancellationToken cancellationToken = default)
    {
        if (_log)
            Logger.LogDebug("-- ZELOS: {method} ({type}): {id}", MethodUtil.Get(), typeof(TDocument).Name, id);

        IZelosContainer container = await _zelosContainerUtil.Get(DatabaseFilePath, ContainerName, cancellationToken).NoSync();

        string? item = container.GetItem(id);

        if (item == null)
            return null;

        return JsonUtil.Deserialize<TDocument>(item);
    }

    public async ValueTask<List<TDocument>?> GetAll(CancellationToken cancellationToken = default)
    {
        IZelosContainer container = await _zelosContainerUtil.Get(DatabaseFilePath, ContainerName, cancellationToken).NoSync();

        List<string> items = container.GetAllItems();

        if (items.Empty())
            return null;

        var list = new List<TDocument>(items.Count);

        for (var i = 0; i < items.Count; i++)
        {
            string item = items[i];
            var document = JsonUtil.Deserialize<TDocument>(item);

            if (document != null)
                list.Add(document);
        }

        return list;
    }

    public ValueTask<TDocument?> GetItemByIdNamePair(IdNamePair idNamePair, CancellationToken cancellationToken = default)
    {
        return GetItem(idNamePair.Id, cancellationToken);
    }

    public virtual async ValueTask<string> AddItem(TDocument document, CancellationToken cancellationToken = default)
    {
        if (_log)
        {
            string? serialized = JsonUtil.Serialize(document, JsonOptionType.Pretty);
            Logger.LogDebug("-- ZELOS: {method} ({type}): {document}", MethodUtil.Get(), typeof(TDocument).Name, serialized);
        }

        IZelosContainer container = await _zelosContainerUtil.Get(DatabaseFilePath, ContainerName, cancellationToken).NoSync();

        string? docSerialized = JsonUtil.Serialize(document);

        if (docSerialized == null)
            throw new Exception("Failed to serialize document");

        _ = await container.AddItem(document.Id, docSerialized, cancellationToken).NoSync();

        return document.Id;
    }

    public virtual async ValueTask<List<TDocument>> AddItems(List<TDocument> documents, CancellationToken cancellationToken = default)
    {
        if (_log)
        {
            Logger.LogDebug("-- COSMOS: {method} ({type})", MethodUtil.Get(), typeof(TDocument).Name);
        }

        IZelosContainer container = await _zelosContainerUtil.Get(DatabaseFilePath, ContainerName, cancellationToken).NoSync();

        foreach (TDocument document in documents)
        {
            string? docSerialized = JsonUtil.Serialize(document);

            if (docSerialized == null)
                throw new Exception("Failed to serialize document");

            _ = await container.AddItem(document.Id, docSerialized, cancellationToken).NoSync();
        }

        return documents;
    }

    public virtual async ValueTask<string> UpdateItem(TDocument document, CancellationToken cancellationToken = default)
    {
        if (_log)
        {
            string? serialized = JsonUtil.Serialize(document, JsonOptionType.Pretty);
            Logger.LogDebug("-- ZELOS: {method} ({type}): {document}", MethodUtil.Get(), typeof(TDocument).Name, serialized);
        }

        IZelosContainer container = await _zelosContainerUtil.Get(DatabaseFilePath, ContainerName, cancellationToken).NoSync();

        string? docSerialized = JsonUtil.Serialize(document);

        if (docSerialized == null)
            throw new Exception("Failed to serialize document");

        _ = await container.UpdateItem(document.Id, docSerialized, cancellationToken).NoSync();

        return document.Id;
    }

    public virtual async ValueTask<List<TDocument>> UpdateItems(List<TDocument> documents, CancellationToken cancellationToken = default)
    {
        if (_log)
        {
            Logger.LogDebug("-- COSMOS: {method} ({type})", MethodUtil.Get(), typeof(TDocument).Name);
        }

        IZelosContainer container = await _zelosContainerUtil.Get(DatabaseFilePath, ContainerName, cancellationToken).NoSync();

        for (var i = 0; i < documents.Count; i++)
        {
            TDocument document = documents[i];
            string? docSerialized = JsonUtil.Serialize(document);

            if (docSerialized == null)
                throw new Exception("Failed to serialize document");

            _ = await container.UpdateItem(document.Id, docSerialized, cancellationToken).NoSync();
        }

        return documents;
    }

    public virtual async ValueTask DeleteItem(string id, CancellationToken cancellationToken = default)
    {
        IZelosContainer container = await _zelosContainerUtil.Get(DatabaseFilePath, ContainerName, cancellationToken).NoSync();

        await container.DeleteItem(id, cancellationToken).NoSync();
    }

    public virtual async ValueTask DeleteAll(CancellationToken cancellationToken = default)
    {
        Logger.LogWarning("-- ZELOS: {method} ({type}) ", MethodUtil.Get(), typeof(TDocument).Name);

        IZelosContainer container = await _zelosContainerUtil.Get(DatabaseFilePath, ContainerName, cancellationToken).NoSync();

        await container.DeleteAllItems(cancellationToken).NoSync();
    }
}