using AssessmentPAS.Data;
using AssessmentPAS.Dto;
using AssessmentPAS.Models;
using AssessmentPAS.Services.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssessmentPAS.Services
{
    public class FormRepository:IFormInterface
    {
        private readonly PASDbContext pasDbContext;
        public FormRepository(PASDbContext pasDbContext)
        {
            this.pasDbContext = pasDbContext;
        }
        public async Task<Form> GetFormById(Guid id)
        {

            var form = await pasDbContext.Forms.FindAsync(id);
            if (form == null)
            {
                return null;
            }
            return form;
        }
        public async Task<IEnumerable<Form>> GetFormByType(string type)
        {
            var forms = await pasDbContext.Forms.Where(x => x.Type == type).ToListAsync();

            return forms != null ? forms : Enumerable.Empty<Form>();
        }
        public async Task<IEnumerable<Form>> GetAllFormsByTableId(Guid TableId)
        {   
            var forms = await pasDbContext.Forms.Include("Table").Where(x => x.TableId == TableId).ToListAsync();
            return forms;
        }
        public async Task<Form> AddFormByTableName(string TableName, Form form)
        {
            await pasDbContext.AddAsync(form);
            await pasDbContext.SaveChangesAsync();
            return form;
        }
        public async Task<Form> UpdateFormByName(string FormName ,EditFormDto editForm)
        {
            var existingForm = await pasDbContext.Forms.FirstOrDefaultAsync(x => x.Name == FormName);
            if (existingForm != null)
            {

                existingForm.Name = editForm.Name ?? existingForm.Name;
                existingForm.Sequence = editForm.Sequence ?? existingForm.Sequence;
                existingForm.Comment = editForm.Comment ?? existingForm.Comment;
                existingForm.TabResourceName = editForm.TabResourceName ?? existingForm.TabResourceName;
                existingForm.SubSequence = editForm.SubSequence ?? existingForm.SubSequence;
                existingForm.MinOccurs = editForm.MinOccurs ?? existingForm.MinOccurs;
                existingForm.MaxOccurs = editForm.MaxOccurs ?? existingForm.MaxOccurs;
                existingForm.BtnCndAdd = editForm.BtnCndAdd ?? existingForm.BtnCndAdd;
                existingForm.BtnCndCopy = editForm.BtnCndCopy ?? existingForm.BtnCndCopy;
                existingForm.BtnCndAdd = editForm.BtnCndAdd ?? existingForm.BtnCndAdd;
                existingForm.Condition = editForm.Condition ?? existingForm.Condition;
                existingForm.Type = editForm.Type ?? existingForm.Type;
                existingForm.AddChangeDeleteFlag = editForm.AddChangeDeleteFlag ?? existingForm.AddChangeDeleteFlag;
                await pasDbContext.SaveChangesAsync();
                return existingForm;
            }
            return null;
        }
        public async Task<Form> DeleteFormById(Guid id)
        {
            var form = await pasDbContext.Forms.FindAsync(id);
            if (form != null)
            {
                pasDbContext.Remove(form);
                await pasDbContext.SaveChangesAsync();
                return form;
            }
            return null;

        }
    }


}
