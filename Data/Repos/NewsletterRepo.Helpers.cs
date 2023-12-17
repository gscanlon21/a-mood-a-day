﻿using Data.Entities.Newsletter;
using Data.Entities.User;
using Data.Models.Newsletter;

namespace Data.Repos;

public partial class NewsletterRepo
{
    /// <summary>
    /// Common properties surrounding today's workout.
    /// </summary>
    internal async Task<WorkoutContext?> BuildWorkoutContext(User user, string token)
    {
        return new WorkoutContext()
        {
            User = user,
            Token = token,
        };
    }

    /// <summary>
    /// Creates a new instance of the newsletter and saves it.
    /// </summary>
    internal async Task<UserMood> CreateAndAddNewsletterToContext(WorkoutContext context)
    {
        var newsletter = new UserMood(context.User.TodayOffset, context);
        _context.UserMoods.Add(newsletter); // Sets the newsletter.Id after changes are saved.
        await _context.SaveChangesAsync();

        return newsletter;
    }
}
