namespace OrdersService.Core.Persistence.Entities;

/// <summary>
/// Message saved in Outbox Table.
/// </summary>
public class OutboxMessage
{
    /// <summary>
    /// Initializes a new instance of the OutboxMessage class.
    /// </summary>
    /// <param name="payload"></param>
    /// <param name="key"></param>
    /// <param name="topic"></param>
    /// <param name="type"></param>
    public static OutboxMessage CreateNew(byte[] payload, byte[] key, string topic, string type) =>
        new()
        {
            MessageId = Guid.NewGuid(),
            Payload = payload,
            Key = key,
            Topic = topic,
            State = MessageState.New,
            Type = type
        };

    /// <summary>
    /// For EF
    /// </summary>
    private OutboxMessage() { }

    /// <summary>
    /// Message Id
    /// </summary>
    public required Guid MessageId { get; init; }

    /// <summary>
    /// The topic to send the message to
    /// </summary>
    public required string Topic { get; init; }

    /// <summary>
    /// The key for the message.
    /// </summary>
    public required byte[] Key { get; init; }

    /// <summary>
    /// Gets the message payload.
    /// </summary>
    public required byte[] Payload { get; init; }

    /// <summary>
    /// Message State
    /// </summary>
    public required MessageState State { get; set; }

    /// <summary>
    /// The date and time when the message was processed, if applicable.
    /// </summary>
    public DateTime? ProcessedOnUtc { get; set; }

    /// <summary>
    /// Type of Domain Event
    /// </summary>
    public required string Type { get; init; }
}