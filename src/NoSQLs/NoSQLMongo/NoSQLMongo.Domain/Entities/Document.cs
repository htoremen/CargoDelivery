using MongoDB.Bson;
using NoSQLMongo.Domain.Interfaces;

namespace NoSQLMongo.Domain.Entities;

public abstract class Document : IDocument
{
    public ObjectId Id { get; set; }

    public DateTime CreatedAt => Id.CreationTime;
}