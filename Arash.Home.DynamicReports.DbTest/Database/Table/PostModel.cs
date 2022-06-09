using Arash.Home.DynamicReports.DbTest.Database.Table;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arash.Home.QueryGenerator.ConsoleTest.DataBase.Table
{
    public class PostModel
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public CategoryModel Category { get; set; }
    }
}
