#nullable enable
using App.Contracts.DAL;
using App.Domain;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.CustomerArea;
[Authorize(Roles = "Admin, Customer", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/CustomerArea/[controller]")]
[ApiController]
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
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        return Ok(await _uow.Comments.GettingAllOrderedCommentsWithIncludesAsync(userId, roleName));
    }

    // GET: api/Comments/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Comment>> GetComment(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var comment = await _uow.Comments.GettingTheFirstCommentAsync(id, userId, roleName);

        if (comment == null) return NotFound();

        return comment;
    }

    // PUT: api/Comments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutComment(Guid id, Comment? comment)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        comment = await _uow.Comments.GettingTheFirstCommentAsync(id, userId, roleName);
        if (comment == null)
        {
            return NotFound();
        }
        try
        {
            comment.UpdatedBy = User.Identity!.Name;
            comment.UpdatedAt = DateTime.Now.ToUniversalTime();
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
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        if (roleName != "Admin" || comment.Drive!.Booking!.Customer!.AppUserId != userId )
        {
            return Forbid();
        }

        comment.Drive.Booking.Customer.AppUserId = userId;
        comment.CreatedBy = User.Identity!.Name;
        comment.CreatedAt = DateTime.Now.ToUniversalTime();
        comment.UpdatedBy = User.Identity.Name;
        comment.UpdatedAt = DateTime.Now.ToUniversalTime();
        _uow.Comments.Add(comment);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetComment", new {id = comment.Id}, comment);
    }

    // DELETE: api/Comments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var comment = await _uow.Comments.GettingTheFirstCommentAsync(id, userId, roleName);
        if (comment == null) return NotFound();

        _uow.Comments.Remove(comment);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private bool CommentExists(Guid id)
    {
        return _uow.Comments.Exists(id);
    }
}