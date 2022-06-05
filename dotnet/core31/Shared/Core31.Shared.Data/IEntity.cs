namespace Core31.Shared.Data
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
