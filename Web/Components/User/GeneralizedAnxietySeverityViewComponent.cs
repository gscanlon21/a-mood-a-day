﻿using Data;
using Data.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.User.Components;

namespace Web.Components.User;

/// <summary>
/// Renders an alert box summary of when the user's next deload week will occur.
/// </summary>
public class GeneralizedAnxietySeverityViewComponent(IServiceScopeFactory serviceScopeFactory, CoreContext context, UserRepo userRepo) : ViewComponent
{
    /// <summary>
    /// For routing
    /// </summary>
    public const string Name = "GeneralizedAnxietySeverity";

    /// <summary>
    /// Today's date in UTC.
    /// </summary>
    private static DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);

    public async Task<IViewComponentResult> InvokeAsync(Data.Entities.User.User user)
    {
        var token = await userRepo.AddUserToken(user, durationDays: 1);
        var userMood = await context.UserGeneralizedAnxietySeverities.OrderByDescending(d => d.Date).FirstOrDefaultAsync(ud => ud.UserId == user.Id);
        var userMoods = await context.UserGeneralizedAnxietySeverities.Where(ud => ud.UserId == user.Id).ToListAsync();
        var viewModel = new GeneralizedAnxietySeverityViewModel(userMoods, userMood?.Score)
        {
            Token = await userRepo.AddUserToken(user, durationDays: 1),
            User = user,
            PreviousMood = userMood,
            UserMood = new Data.Entities.User.UserGeneralizedAnxietySeverity()
            {
                UserId = user.Id,
                User = user
            },
        };

        return View("GeneralizedAnxietySeverity", viewModel);
    }
}
