﻿using Data.Entities.Task;

namespace Web.Views.User;

/// <summary>
/// For CRUD actions
/// </summary>
public class UserManageTaskViewModel
{
    public record Params(string Email, string Token, int RecipeId);

    public required Params Parameters { get; init; }

    public Data.Entities.User.User User { get; init; } = null!;

    public required UserTask Task { get; init; }

    public bool? WasUpdated { get; init; }
}