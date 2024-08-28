﻿using Core.Models.User;
using Data.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Views.User;

namespace Web.Controllers.User;

public partial class UserController
{
    [HttpPost, Route("feast")]
    public async Task<IActionResult> EditFeast(string email, string token, [FromForm] string feastEmail, [FromForm] string? feastToken)
    {
        var user = await _userRepo.GetUser(email, token);
        if (user == null)
        {
            return View("StatusMessage", new StatusMessageViewModel(LinkExpiredMessage));
        }

        if (string.IsNullOrWhiteSpace(feastEmail) || string.IsNullOrWhiteSpace(feastToken))
        {
            TempData[TempData_User.FailureMessage] = "Missing email or token.";
            return RedirectToAction(nameof(Edit), new { email, token, WasUpdated = false });
        }

        var response = await _httpClient.GetAsync($"{_siteSettings.Value.FeastUri}/User/Allergens?weeks={1}&email={Uri.EscapeDataString(feastEmail)}&token={Uri.EscapeDataString(feastToken)}");
        var weeklyFeast = await ApiResult<IDictionary<Allergy, double?>>.FromResponse(response);
        if (!weeklyFeast.IsSuccessStatusCode)
        {
            TempData[TempData_User.FailureMessage] = "Failed to connect to server.";
            return RedirectToAction(nameof(Edit), new { email, token, WasUpdated = false });
        }

        user.FeastEmail = feastEmail;
        user.FeastToken = feastToken;
        await _context.SaveChangesAsync();

        TempData[TempData_User.SuccessMessage] = "Your preferences have been saved.";
        return RedirectToAction(nameof(Edit), new { email, token, WasUpdated = true });
    }

    [HttpPost, Route("allergens")]
    public async Task<IActionResult> ManageAllergens(string email, string token, UserFeastAllergens userMood)
    {
        if (true || ModelState.IsValid)
        {
            var user = await _userRepo.GetUser(email, token, allowDemoUser: true);
            if (user == null || string.IsNullOrWhiteSpace(user.FeastEmail) || string.IsNullOrWhiteSpace(user.FeastToken))
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"{_siteSettings.Value.FeastUri.AbsolutePath}/user/Allergens?weeks={1}&email={Uri.EscapeDataString(user.FeastEmail)}&token={Uri.EscapeDataString(user.FeastToken)}");
            var allergens = await ApiResult<IDictionary<Allergy, double>>.FromResponse(response);
            if (!allergens.HasValue)
            {
                return RedirectToAction(nameof(ManageMood), new { email, token, WasUpdated = false });
            }

            var startOfWeek = DateHelpers.Today.StartOfWeek();
            var todaysMood = await _context.UserFeastAllergens.FirstOrDefaultAsync(p => p.UserId == user.Id && p.Date == startOfWeek);
            if (todaysMood == null)
            {
                userMood.UserId = user.Id;
                userMood.Allergens = allergens.Value;
                _context.Add(userMood);
            }
            else
            {
                todaysMood.Allergens = allergens.Value;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageMood), new { email, token, WasUpdated = true });
        }

        return RedirectToAction(nameof(ManageMood), new { email, token, WasUpdated = false });
    }
}