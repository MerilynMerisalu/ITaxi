/*#nullable enable
using App.Contracts.DAL;
using App.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

[Route("api/AdminArea/[controller]")]
[ApiController]
[Authorize(Roles = nameof(Admin), AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CommentsController : ControllerBase
{
    private readonly IAppUnitOfWork _uow;

    public CommentsController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: api/Comments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
    {
        return Ok(await _uow.Comments.GettingAllOrderedCommentsWithoutIncludesAsync());
    }

    // GET: api/Comments/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Comment>> GetComment(Guid id)
    {
        var comment = await _uow.Comments.GettingCommentWithoutIncludesAsync(id);

        if (comment == null) return NotFound();

        return comment;
    }

    // PUT: api/Comments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutComment(Guid id, Comment comment)
    {
        if (id != comment.Id) return BadRequest();


        try
        {
            _uow.Comments.Update(comment);
            await _uow.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CommentExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Comments
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Comment>> PostComment(Comment comment)
    {
        _uow.Comments.Add(comment);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetComment", new {id = comment.Id}, comment);
    }

    // DELETE: api/Comments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        var comment = await _uow.Comments.GettingCommentWithoutIncludesAsync(id);
        if (comment == null) return NotFound();

        _uow.Comments.Remove(comment);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private bool CommentExists(Guid id)
    {
        return _uow.Comments.Exists(id);
    }
}*/