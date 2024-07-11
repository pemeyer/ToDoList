using Microsoft.AspNetCore.Mvc;
using ToDo.Server.Models.Dtos;
using ToDo.Server.Services.Interfaces;

namespace ToDo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _service;
        public ToDoController(IToDoService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetToDoList()
        {
            var ToDoList = _service.GetAll();
            if (ToDoList == null) 
            { 
                return NotFound();
            }
            return Ok(ToDoList);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateItem([FromBody] ItemDto itemDto)
        {
            var createdProduct = await _service.Add(itemDto);

            return CreatedAtAction(nameof(_service.GetItem), new { Id = createdProduct.Id }, createdProduct);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteItem(Guid Id)
        {
            var ToDoItem = _service.GetItem(Id);
            if (ToDoItem == null)
            {
                return NotFound();
            }

            await _service.Delete(Id);

            return NoContent();
        }
    }
}
