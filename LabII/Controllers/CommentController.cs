using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab6.Viewmodels;
using Lab6.Models;
using Lab6.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private ICommentService commentsService;
        private IUsersService usersService;


        public CommentController(ICommentService commentsService, IUsersService usersService)
        {
            this.commentsService = commentsService;
            this.usersService = usersService;
        }

        ///<remarks>
        ///{
        ///"id": 1,
        ///"text": "What?! 777.55",
        ///"important": true,
        ///"expenseId": 7
        ///}
        ///</remarks>
        /// <summary>
        ///  Gets all comments filtered by a string
        /// </summary>
        /// <param name="filter">Opyional, the keyword used to filter comments</param>
        /// <param name="page"></param>
        /// <returns>List of comments</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize(Roles = "Regular, Admin")] -nu e necesar!
        // GET: api/Comments
        [HttpGet]
        
        public PaginatedListModel<CommentGetModel> GetAll([FromQuery]String filter, [FromQuery]int page = 1)
        {
            
            return commentsService.GetAll(filter, page);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Regular")]
        public IActionResult Get(int id)
        {
            var found = commentsService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }

            return Ok(found);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Regular")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public void Post([FromBody] CommentPostModel comment)
        {
            User addedBy = usersService.GetCurrentUser(HttpContext);
            commentsService.Create(comment, addedBy);
        }

        [Authorize(Roles = "Admin,Regular")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Comment comment)
        {
            var result = commentsService.Upsert(id, comment);
            return Ok(result);
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Regular")]
        public IActionResult Delete(int id)
        {
            var result = commentsService.Delete(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

    }
}