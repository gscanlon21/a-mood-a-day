﻿using Core.Consts;
using Data.Entities.User;
using System.ComponentModel.DataAnnotations;
using Web.ViewModels;

namespace Web.Views.User;


/// <summary>
/// For CRUD actions
/// </summary>
public class UserManageMoodViewModel
{
    public record TheParameters(string Email, string Token);

    [Obsolete("Public parameterless constructor for model binding.", error: true)]
    public UserManageMoodViewModel() { }

    public UserManageMoodViewModel(IList<UserMood>? userWeights, int? currentWeight)
    {
        Mood = currentWeight.GetValueOrDefault();
        if (userWeights != null && currentWeight.HasValue)
        {
            // Skip today, start at 1, because we append the current weight onto the end regardless.
            Xys = Enumerable.Range(1, UserConsts.ChartTimeFrameDays).Select(i =>
            {
                var date = DateHelpers.Today.AddDays(-i);
                return new Xy(date, (int?)userWeights.FirstOrDefault(uw => uw.Date == date)?.Mood);
            }).Where(xy => xy.Y.HasValue).Reverse().Append(new Xy(DateHelpers.Today, currentWeight)).ToList();
        }
    }

    public required TheParameters Parameters { get; init; }

    public required Data.Entities.User.User User { get; init; }

    public bool? WasUpdated { get; init; }

    [Range(1, 5)]
    [Display(Name = "How was your day?")]
    public int? Mood { get; init; }

    internal IList<Xy> Xys { get; init; } = [];
}
