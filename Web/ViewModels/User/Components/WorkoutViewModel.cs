﻿namespace Web.ViewModels.User.Components;

public class WorkoutViewModel
{
    public required Data.Entities.User.User User { get; init; } = null!;

    public required string Token { get; init; } = null!;
}
