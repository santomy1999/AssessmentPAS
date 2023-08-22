using AssessmentPAS.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AssessmentPAS.Services.Interfaces
{
    public interface ITableInterface
    {
        Task<Aotable> GetTableById(Guid id);
        Task<Aotable> GetTableByTableName(string TableName);
    }
}