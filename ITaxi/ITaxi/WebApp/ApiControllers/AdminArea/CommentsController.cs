#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Contracts.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CommentsController : ControllerBase
{
    private readonly IAppBLL _appBLL;

    public CommentsController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: api/Comments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentDTO>>> GetComments()
    {
        return Ok(await _appBLL.Comments.GettingAllOrderedCommentsWithoutIncludesAsync());
    }

    // GET: api/Comments/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDTO>> GetComment(Guid id)
    {
        var comment = await _appBLL.Comments.GettingCommentWithoutIncludesAsync(id);

        if (comment == null) return NotFound();

        return comment;
    }

    // PUT: api/Comments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutComment(Guid id, CommentDTO comment)
    {
        if (id != comment.Id) return BadRequest();


        try
        {
            _appBLL.Comments.Update(comment);
            await _appBLL.SaveChangesAsync();
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
    public async Task<ActionResult<CommentDTO>> PostComment([FromBody] CommentDTO comment)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }
        _appBLL.Comments.Add(comment);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetComment", new
        {
            id = comment.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString() ,
        }, comment);
    }

    // DELETE: api/Comments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        var comment = await _appBLL.Comments.GettingCommentWithoutIncludesAsync(id);
        if (comment == null) return NotFound();

        _appBLL.Comments.Remove(comment);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool CommentExists(Guid id)
    {
        return _appBLL.Comments.Exists(id);
    }
}