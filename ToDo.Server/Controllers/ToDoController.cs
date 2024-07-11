using Microsoft.AspNetCore.Mvc;
using ToDo.Server.Models.Dtos;

namespace ToDo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : Controller
    {

        public ToDoController()
        {

        }

        [HttpGet]
        public IActionResult GetToDoList()
        {
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateItem(ItemDto itemDto)
        {

            return CreatedAtAction();
        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteItem(Guid Id)
        {

            return NoContent();
        }
    }
}
