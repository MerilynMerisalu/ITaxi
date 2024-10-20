#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Public.DTO.v1.AdminArea;
using AutoMapper;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

/// <summary>
/// Api controller for comments
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CommentsController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor for comments api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.AdminArea.CommentDTO to Public.DTO.v1.AdminArea.Comment</param>
    public CommentsController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/Comments
    /// <summary>
    /// Gets all the comments
    /// </summary>
    /// <returns>List of comments with a statusCode 200OK or statusCode 403 or statusCode 401</returns>
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<Comment>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
    {
        var res = await _appBLL.Comments.GettingAllOrderedCommentsWithoutIncludesAsync();
        return Ok(res.Select(c=> _mapper.Map<Comment>(c)));
    }

    // GET: api/Comments/5
    /// <summary>
    /// Returns comment based on id
    /// </summary>
    /// <param name="id">Comment id, Guid</param>
    /// <returns>Comment (TEntity) with statusCode 200 or statusCode 404 or statusCode 403 or statusCode 401</returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Comment), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Comment>> GetComment(Guid id)
    {
        var comment = await _appBLL.Comments.GettingCommentWithoutIncludesAsync(id);

        if (comment == null) return NotFound();

        return _mapper.Map<Comment>(comment);
    }

    // PUT: api/Comments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Updating an comment
    /// </summary>
    /// <param name="id">An id of the entity which is updated</param>
    /// <param name="comment">DTO which holds the values</param>
    /// <returns>StatusCode 204 or StatusCode 403 or StatusCode 404 or StatusCode 401 or StatusCode 400</returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutComment(Guid id, Comment comment)
    {
        if (id != comment.Id) return BadRequest();

        var commentDTO = await _appBLL.Comments.GettingCommentWithoutIncludesAsync(id);

        if (commentDTO != null)
        {
            if (commentDTO.StarRating >= 0)
            {
                comment.StarRating = commentDTO.StarRating;
            }
            commentDTO.CommentText = comment.CommentText;
            commentDTO.DriveId = comment.DriveId;
            commentDTO.UpdatedBy = User.GettingUserEmail();
            commentDTO.UpdatedAt = DateTime.Now.ToUniversalTime();

            try
            {
                _appBLL.Comments.Update(commentDTO);
                await _appBLL.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                    return NotFound();
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Comments
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Creating a new comment
    /// </summary>
    /// <param name="comment">Comment with properties</param>
    /// <returns>Status201Created with an entity</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Comment), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Comment>> PostComment([FromBody] Comment comment)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }

        var commentDTO = new CommentDTO();
        if (commentDTO.StarRating >= 0)
        {
            comment.StarRating = commentDTO.StarRating;
        }
        commentDTO.CommentText = comment.CommentText;
        commentDTO.DriveId = comment.DriveId;
        commentDTO.CreatedBy = User.GettingUserEmail();
        commentDTO.CreatedAt = DateTime.Now.ToUniversalTime();
        commentDTO.UpdatedBy = User.GettingUserEmail();
        commentDTO.UpdatedAt = DateTime.Now.ToUniversalTime();
        
        _appBLL.Comments.Add(commentDTO);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetComment", new
        {
            id = comment.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString() ,
        }, comment);
    }

    // DELETE: api/Comments/5
    /// <summary>
    /// Deletes an entity
    /// </summary>
    /// <param name="id">Id of an entity</param>
    /// <returns>Status204</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        var comment = await _appBLL.Comments.GettingCommentWithoutIncludesAsync(id);
        if (comment == null) return NotFound();
        if (comment.DriveId != null)
        {
            comment.DriveId = null;
            _appBLL.Comments.Update(comment);
            await _appBLL.SaveChangesAsync();
        }
       await _appBLL.Comments.RemoveAsync(comment.Id);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }
    
    /// <summary>
    /// Return a boolean based on whether or not an entity exists
    /// </summary>
    /// <param name="id">Entity id guid</param>
    /// <returns>boolean value</returns>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]

    private bool CommentExists(Guid id)
    {
        return _appBLL.Comments.Exists(id);
    }
}