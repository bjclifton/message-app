using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace server.Models
{
  public class Message
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("conversationId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ConversationId { get; set; }

    [BsonElement("senderId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string SenderId { get; set; }

    [BsonElement("receiverId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ReceiverId { get; set; }

    [BsonElement("content")]
    public string Content { get; set; }

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; }
  }

}

