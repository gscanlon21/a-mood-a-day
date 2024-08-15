﻿using Data.Models.Newsletter;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Entities.Newsletter;

/// <summary>
/// A day's workout routine.
/// </summary>
[Table("user_diary"), Comment("A day's workout routine")]
public class UserDiary
{
    [Obsolete("Public parameterless constructor required for EF Core .AsSplitQuery()", error: true)]
    public UserDiary() { }

    internal UserDiary(DateOnly date, NewsletterContext context) : this(date, context.User) { }

    public UserDiary(DateOnly date, User.User user)
    {
        Date = date;
        UserId = user.Id;
        Logs = Core.Code.Logs.WriteLogs(user);
    }

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private init; }

    [Required]
    public int UserId { get; private init; }

    /// <summary>
    /// The date the workout is for, using the user's UTC offset date.
    /// </summary>
    [Required]
    public DateOnly Date { get; private init; }

    public string? Logs { get; private init; }

    //[JsonIgnore, InverseProperty(nameof(Entities.User.User.UserFeasts))]
    //public virtual User.User User { get; private init; } = null!;

    [JsonIgnore, InverseProperty(nameof(UserDiaryTask.UserDiary))]
    public virtual ICollection<UserDiaryTask> UserDiaryTasks { get; private init; } = null!;

}