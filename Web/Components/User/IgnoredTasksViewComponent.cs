﻿using Core.Dtos.Newsletter;
using Core.Dtos.User;
using Data;
using Data.Models;
using Data.Query.Builders;
using Data.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Views.Shared.Components.IgnoredTasks;

namespace Web.Components.User;


/// <summary>
/// Renders an alert box summary of when the user's next deload week will occur.
/// </summary>
public class IgnoredTasksViewComponent(CoreContext context, UserRepo userRepo, IServiceScopeFactory serviceScopeFactory) : ViewComponent
{
    /// <summary>
    /// For routing
    /// </summary>
    public const string Name = "IgnoredTasks";

    public async Task<IViewComponentResult> InvokeAsync(Data.Entities.User.User user)
    {
        if (!user.Components.HasFlag(Core.Models.User.Components.Tasks))
        {
            return Content("");
        }

        // Need a user context so the manage link is clickable and the user can un-ignore an exercise/variation.
        var token = await userRepo.AddUserToken(user, durationDays: 1);
        var userNewsletter = new UserNewsletterDto(user.AsType<UserDto, Data.Entities.User.User>()!, token);

        var userTasks = await context.UserTasks
            .Where(r => r.UserId == user.Id)
            .ToListAsync();

        var tasks = await new QueryBuilder()
            // Include disabled tasks.
            .WithUser(user, ignored: true)
            .WithTasks(x =>
            {
                x.AddTasks(userTasks);
            })
            .Build()
            .Query(serviceScopeFactory);

        return View("IgnoredTasks", new IgnoredTasksViewModel()
        {
            UserNewsletter = userNewsletter,
            Tasks = tasks.Select(r => r.AsType<NewsletterTaskDto, QueryResults>()!).ToList(),
        });
    }
}