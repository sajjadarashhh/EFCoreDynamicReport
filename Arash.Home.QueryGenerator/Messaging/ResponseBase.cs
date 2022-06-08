namespace Arash.Home.QueryGenerator.Messaging
{
    public abstract class ResponseBase
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
    public abstract class ResponseBase<TEntity> : ResponseBase
    {
        public TEntity Entity { get; set; }
    }
    public abstract class ResponseListBase<TEntity> : ResponseBase
    {
        public List<TEntity> Entities { get; set; }
    }
}
