using AssessmentPAS.Dto;
using AssessmentPAS.Models;

    namespace AssessmentPAS.Services.Interfaces
{
    public interface IFormInterface
    {
        Task<Form> AddFormByTableName(string TableName, Form form);
        Task<Form> DeleteFormById(Guid id);
        Task<IEnumerable<Form>> GetAllFormsByTableId(Guid TableId);
        Task<Form> GetFormById(Guid id);
        Task<IEnumerable<Form>> GetFormByType(string type);
        Task<Form> UpdateFormByName(string FormName, EditFormDto editForm);
    }
}