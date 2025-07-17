using Microsoft.AspNetCore.Mvc;
using InfoneticaTask.Services;

namespace InfoneticaTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstanceController : ControllerBase
    {
        private readonly WorkflowService _service;

        public InstanceController(WorkflowService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllInstances() => Ok(_service.GetAllInstances());

        [HttpGet("{instanceId}")]
        public IActionResult GetInstance(string instanceId)
        {
            var instance = _service.GetInstance(instanceId);
            return instance != null ? Ok(instance) : NotFound("Instance not found");
        }

        [HttpPost("{workflowId}")]
        public IActionResult StartWorkflowInstance(string workflowId)
        {
            var instance = _service.StartWorkflowInstance(workflowId);
            return instance != null ? Ok(instance) : BadRequest("Invalid workflow");
        }

        [HttpPost("{instanceId}/actions/{actionId}")]
        public IActionResult ExecuteAction(string instanceId, string actionId)
        {
            var result = _service.ExecuteAction(instanceId, actionId);
            return result.IsSuccess ? Ok(result.Message) : BadRequest(result.Message);
        }
    }
}
