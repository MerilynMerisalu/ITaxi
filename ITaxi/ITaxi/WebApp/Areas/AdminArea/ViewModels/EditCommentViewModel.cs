﻿using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Edit comment view model
/// </summary>
public class EditCommentViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Drive id
    /// </summary>
    [Display(ResourceType = typeof(Comment), Name = nameof(Drive))]
    public Guid? DriveId { get; set; }

    /// <summary>
    /// Drive time and driver
    /// </summary>
    public string? DriveTimeAndDriver { get; set; }

    /// <summary>
    /// Drive
    /// </summary>
    [Display(ResourceType = typeof(Comment), Name = nameof(Drive))]
    public Drive? Drive { get; set; }

    /// <summary>
    /// Comment text
    /// </summary>
    [StringLength(1000)]
    [DataType(DataType.MultilineText)]
    [Display(ResourceType = typeof(Comment), Name = "CommentName")]
    public string CommentText { get; set; } = default!;
}