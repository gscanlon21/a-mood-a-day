﻿using Data.Entities.Footnote;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Entities.User;


/// <summary>
/// User's progression level of an exercise.
/// </summary>
[Table("user_medicine"), Comment("User variation weight log")]
public class UserMedicine
{
    public UserMedicine() { }

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private init; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public DateOnly Date { get; init; } = DateHelpers.Today;

    [JsonIgnore, InverseProperty(nameof(Entities.Footnote.UserCustom.UserMedicines))]
    public virtual List<UserCustom> UserCustoms { get; init; } = null!;

    [JsonIgnore, InverseProperty(nameof(Entities.User.User.UserMedicines))]
    public virtual User User { get; init; } = null!;
}
