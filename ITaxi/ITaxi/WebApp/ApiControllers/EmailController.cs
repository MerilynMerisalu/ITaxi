using Microsoft.AspNetCore.Mvc;
using WebApp.Helpers;

namespace WebApp.ApiControllers;

/// <summary>
/// Email controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MailController : ControllerBase
{
    private readonly IMailService _mailService;

    /// <summary>
    /// Email controller constructor
    /// </summary>
    /// <param name="mailService">Email service</param>
    public MailController(IMailService mailService)
    {
        _mailService = mailService;
    }

    /// <summary>
    /// Email controller send email
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Status 200OK response</returns>
    [HttpPost("send")]
    public async Task<IActionResult> SendMail([FromForm] MailRequest request)
    {
        await _mailService.SendEmailAsync(request);
        return Ok();
    }
}