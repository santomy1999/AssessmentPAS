using AssessmentPAS.Dto;
using AssessmentPAS.Models;
using AssessmentPAS.Services;
using AssessmentPAS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AssessmentPAS.Controllers
{
    [ApiController]
    [Route("api/")]
    public class PASTableController : ControllerBase
    {
        public IFormInterface FormInterface{ get; }
        public ITableInterface TableInterface { get; }
        //private readonly PASDbContext pasDbContext;
        public PASTableController(IFormInterface  formInterface, ITableInterface tableInterface)
        {
            FormInterface = formInterface;
            TableInterface = tableInterface;
        }
        //Read operations

        //Get form using formid
        [HttpGet("Form/id/{id:guid}")]
        //[Route("Form/id/{id:guid}")]
        public async Task<IActionResult> GetFormById([FromRoute] Guid id)
        {
            try
            {
                var form = await FormInterface.GetFormById(id);
                if (form == null)
                {
                    return NotFound("Item Not Found");
                }
                return Ok(form);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        //Get forms of a type

        [HttpGet("Form/Type/{type}")]
        public async Task<IActionResult> GetFormByType(string type)
        {
            try
            {
                var forms = await FormInterface.GetFormByType(type);
                return forms != null?Ok(forms): NotFound($"No Forms with type '{type}' was found");
                
            }
            catch 
            {
                return StatusCode(500,$"Error Occured While Getting Form of Type '{type}'");
            }
        }
        //Get  forms of a table along with its table name using table id
        [HttpGet("Forms/TableId/{TableId:Guid}")]
        public async Task<ActionResult<List<Aotable>>> GetAllFormsByTableId(Guid TableId)
        {
            try
            {
                var table = await TableInterface.GetTableById(TableId);
                if (table == null)
                {
                    return NotFound($"Table ({TableId}) Not Found!");
                }
                var forms = await FormInterface.GetAllFormsByTableId(TableId);
                if (forms.Any())
                {
                    var result = new
                    {
                        forms,
                        tablename = table.Name
                    };
                    return Ok(result);
                }
                return NotFound($"No Form associated with table({TableId})");
            }
            catch
            {
                return StatusCode(500);
            }

        }
        //Get all records from Form table associated to the TableName (AOTable) passed as parameter
        [HttpGet("Form/TableName/{TableName}")]
        public async Task<IActionResult> GetAllFormsByTableName(string TableName)
        {
            try
            {
                if (string.IsNullOrEmpty(TableName))
                {
                    return BadRequest("The table name cannot be empty.");
                }
                var table = await TableInterface.GetTableByTableName(TableName);
                if(table==null)
                {
                    return NotFound($"Table - '{TableName}' not found");
                }
                var table_id = table.Id;
                var forms = await FormInterface.GetAllFormsByTableId(table.Id);
                if (forms.Any())
                {
                    return Ok(forms);
                }
                return NotFound($"Table - '{TableName}({table.Id})'has no forms");
                
            }
            catch
            {
                return StatusCode(500);
            }
        }
 
        //Add record to form table using table name
        [HttpPost("Form/Add/{TableName}")]
        public async Task<IActionResult> AddFormByTableName(string TableName, [FromBody] Form newForm)
        {

            try
            {
                if (string.IsNullOrEmpty(TableName))
                {
                    return BadRequest("The table name cannot be empty.");
                }
                var table =  await TableInterface.GetTableByTableName(TableName); 
                if (table != null)
                {
                    newForm.Id = Guid.NewGuid();
                    newForm.TableId = table.Id;
                    var result = await FormInterface.AddFormByTableName(TableName, newForm);
                    if(result!=null)
                    {
                        return Ok(result);                        
                    }
                    return BadRequest("Error occured");
                }
                return NotFound($"No Mathcing found for TableName '{TableName}'");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        //Edit a record in Form table by passing Form Name as the parameter
        [HttpPatch("Form/Update/{FormName}")]
        public async Task<IActionResult> UpdateFormByName(string FormName, [FromBody] EditFormDto editForm)
        {
            try
            {
                if(string.IsNullOrEmpty(FormName))
                {
                    return BadRequest("Form name shouldn't be empty");
                }
                var result = await FormInterface.UpdateFormByName(FormName, editForm);
                if (result!=null)
                {
                    return Ok(result);
                }
                return NotFound($"Form {FormName} not found");
            }
            catch
            {
                return StatusCode(500,"Error occured");
            }

        }
        [HttpDelete("Form/Delete/{id:guid}")]
        public async Task<IActionResult> DeleteFormById([FromRoute] Guid id)
        {
            try
            {

                var result = await FormInterface.DeleteFormById(id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("Form Not Found");
            }
            catch
            {
                return StatusCode(500, "Error Occured");
            }
        }


    }
}
