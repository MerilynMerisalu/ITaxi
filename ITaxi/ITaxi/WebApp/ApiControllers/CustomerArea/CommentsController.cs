#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.CustomerArea;

[ApiController]
[Route("api/v{version:apiVersion}/CustomerArea/[controller]")]
[Authorize(Roles = "Admin, Customer", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Comments.GettingAllOrderedCommentsWithIncludesAsync(userId, roleName);

        return Ok(res);
    }

    // GET: api/Comments/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDTO>> GetComment(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id, userId, roleName);

        if (comment == null) return NotFound();


        return comment;
    }

    // PUT: api/Comments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutComment(Guid id, CommentDTO? comment)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id, userId, roleName);
        if (comment == null)
        {
            return NotFound();
        }

        try
        {
            comment.UpdatedBy = User.Identity!.Name;
            comment.UpdatedAt = DateTime.Now.ToUniversalTime();
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
    public async Task<ActionResult<CommentDTO>> PostComment([FromBody]CommentDTO comment)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        if (roleName != "Admin" || comment.Drive!.Booking!.Customer!.AppUserId != userId )
        {
            return Forbid();
        }

        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }
        comment.Drive.Booking.Customer.AppUserId = userId;
        comment.CreatedBy = User.Identity!.Name;
        comment.CreatedAt = DateTime.Now;
        comment.UpdatedBy = User.Identity.Name;
        comment.UpdatedAt = DateTime.Now;
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
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id, userId, roleName);
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