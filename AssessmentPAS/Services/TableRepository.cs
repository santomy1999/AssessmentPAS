using AssessmentPAS.Data;
using AssessmentPAS.Models;
using AssessmentPAS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace AssessmentPAS.Services
{
    public class TableRepository:ITableInterface
    {
        public readonly PASDbContext pasDbContext;

        public TableRepository(PASDbContext pasDbContext)
        {
            this.pasDbContext = pasDbContext;
        }

        public async Task<Aotable> GetTableById(Guid id)
        {

            var table = await pasDbContext.Aotables.FindAsync(id);
            if (table== null) { 
            return null;
            }
            return table;
        }
        public async Task<Aotable> GetTableByTableName(string TableName)
        {
            var table = await pasDbContext.Aotables.FirstOrDefaultAsync(x => x.Name == TableName);

            return table; //!= null ? table : null;

        }
        //public async Task<>
    }
}
