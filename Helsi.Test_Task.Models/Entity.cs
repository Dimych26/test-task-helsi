
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Helsi.Test_Task.Models;
public interface IEntity
{
    [BsonId]
    public ObjectId Id { get; }
}

public abstract class Entity : IEntity
{
    public ObjectId Id { get; private set; }
}
