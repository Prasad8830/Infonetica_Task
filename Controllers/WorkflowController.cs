using Microsoft.AspNetCore.Mvc;
using InfoneticaTask.Models;
using InfoneticaTask.Services;

namespace InfoneticaTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkflowController : ControllerBase
    {
        private readonly WorkflowService _service;

        public WorkflowController(WorkflowService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult CreateWorkflow([FromBody] WorkflowDefinition definition)
        {
            if (definition == null)
                return BadRequest("Invalid workflow definition");

            var result = _service.CreateWorkflow(definition);
            return result.IsSuccess
                ? Ok(new { message = result.Message, workflow = definition })
                : BadRequest(result.Message);
        }

        [HttpGet]
        public IActionResult GetAllWorkflows() => Ok(_service.GetAllWorkflows());

        [HttpGet("{id}")]
        public IActionResult GetWorkflow(string id)
        {
            var wf = _service.GetWorkflow(id);
            return wf != null ? Ok(wf) : NotFound("Workflow not found");
        }
    }
}
