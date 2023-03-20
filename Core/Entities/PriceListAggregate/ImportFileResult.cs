using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.PriceListAggregate
{
    public class ImportFileResult
    {
        public int ProductsTotal { get; set; } = 0;
        public int ProductsNotUpdated { get; set; } = 0; // not changed
        public int ProductsUpdateSuccess { get; set; } = 0;
        public int ProductsUpdateErrors { get; set; } = 0;
        public int ProductsCreateErrors { get; set; } = 0;
        public int ProductsCreated { get; set; } = 0;
        public int ProductsNotFound { get; set; } = 0;
        public List<ImportResult<Result>> ImportResult { get; set; } = new ();
    }

    public class Result : BaseEntity
    {
        public Result()
        {
            TotalFoundInFile = 0;
            Created = 0;
            NotUpdated = 0;
            UpdateSuccess = 0;
            CreateErrors = 0;
            UpdateErrors = 0;
        }
        public int TotalFoundInFile { get; set; }
        public int Created { get; set; }
        public int NotUpdated { get; set; }
        public int UpdateSuccess { get; set; }
        public int CreateErrors { get; set; }
        public int UpdateErrors { get; set; }
        

    }
    public class ImportResult<T> : Result where T : Result
    {
        public ImportResult(string typeName)
        {
            this.TypeName = typeName;
        }

        public string TypeName { get; }
    }

}