﻿using Data;
using Data.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Views.Shared.Components.Medicine;

namespace Web.Components.User;

/// <summary>
/// Renders an alert box summary of when the user's next deload week will occur.
/// </summary>
public class MedicineViewComponent(CoreContext context, UserRepo userRepo) : ViewComponent
{
    /// <summary>
    /// For routing
    /// </summary>
    public const string Name = "Medicine";

    public async Task<IViewComponentResult> InvokeAsync(Data.Entities.User.User user)
    {
        var token = await userRepo.AddUserToken(user, durationDays: 1);
        var userMood = await context.UserMedicines.OrderByDescending(d => d.Date).FirstOrDefaultAsync(ud => ud.UserId == user.Id);
        var userMoods = (await context.UserMedicines
            .Include(ud => ud.UserCustoms)
            .Where(ud => ud.UserId == user.Id)
            .ToListAsync());
        var userCustoms = await context.UserCustoms
            .Where(c => c.Type == Core.Models.Footnote.CustomType.Medicine)
            .Where(c => c.UserId == null || c.UserId == user.Id)
            .ToListAsync();
        var viewModel = new MedicineViewModel(userMoods, userCustoms)
        {
            Token = await userRepo.AddUserToken(user, durationDays: 1),
            User = user,
            PreviousMood = userMood,
            UserMood = new Data.Entities.User.UserMedicine()
            {
                UserId = user.Id,
                User = user
            },
        };

        return View("Medicine", viewModel);
    }
}
