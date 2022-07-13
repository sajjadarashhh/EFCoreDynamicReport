namespace Arash.Home.ExcelGenerator.ExcelGenerator.AdapterOptions
{
    public abstract class AdapterBase
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string Execute(string value);
    }
}
