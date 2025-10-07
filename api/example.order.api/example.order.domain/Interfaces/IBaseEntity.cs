namespace example.domain.Interfaces
{
    public interface IBaseEntity<TKey>
    {
        TKey Id { get; set; }
    }

    public interface IDeleteEntity
    {
        bool IsDeleted { get; set; }
    }

    public interface IDeleteEntity<TKey> : IDeleteEntity, IBaseEntity<TKey>
    {
    }

    public interface IAuditEntity
    {
        DateTime? CreatedAt { get; set; }
        string? CreatedBy { get; set; }
        DateTime? UpdatedAt { get; set; }
        string? UpdatedBy { get; set; }
    }

    public interface IAuditEntity<TKey> : IAuditEntity, IBaseEntity<TKey>
    {
    }
}
