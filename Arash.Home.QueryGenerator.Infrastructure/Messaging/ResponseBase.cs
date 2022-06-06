namespace Arash.Home.QueryGenerator.Infrastructure.Messaging
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
}
