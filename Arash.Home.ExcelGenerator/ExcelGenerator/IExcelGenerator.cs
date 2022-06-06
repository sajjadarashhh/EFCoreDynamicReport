using Arash.Home.ExcelGenerator.ExcelGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arash.Home.ExcelGenerator.ExcelGenerator
{
    public interface IExcelGenerator
    {
        void GenerateExcel<TEntity>(ExcelGenerateVm<TEntity> generateVm) where TEntity : class, new();
        void GenerateExcelFromAnonymousType(ExcelGenerateVm generateVm);
    }
}
