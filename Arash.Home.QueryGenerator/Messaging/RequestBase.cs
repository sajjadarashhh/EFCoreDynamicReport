namespace Arash.Home.QueryGenerator.Messaging
{
    public abstract class RequestBase
    {
    }
    public abstract class RequestBase<TEntity> : RequestBase
    {
        public TEntity Entity { get; set; }
    }
}
