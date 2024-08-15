﻿using Core.Models.Newsletter;
using Data.Entities.Task;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Entities.Newsletter;

/// <summary>
/// A day's workout routine.
/// </summary>
[Table("user_diary_task"), Comment("A day's workout routine")]
public class UserDiaryTask
{
    public UserDiaryTask() { }

    public UserDiaryTask(UserDiary newsletter, UserTask recipe)
    {
        UserDiaryId = newsletter.Id;
        UserTaskId = recipe.Id;
    }

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private init; }

    public int UserDiaryId { get; private init; }

    public int UserTaskId { get; private init; }

    /// <summary>
    /// The order of each exercise in each section.
    /// </summary>
    public int Order { get; init; }

    /// <summary>
    /// What section of the newsletter is this?
    /// </summary>
    public Section Section { get; init; }

    [JsonIgnore, InverseProperty(nameof(Task.UserTask.UserDiaryTasks))]
    public virtual UserTask UserTask { get; private init; } = null!;

    [JsonIgnore, InverseProperty(nameof(Newsletter.UserDiary.UserDiaryTasks))]
    public virtual UserDiary UserDiary { get; private init; } = null!;
}