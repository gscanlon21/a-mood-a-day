﻿using Core.Dtos.User;
using Core.Models.Options;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;

namespace Lib.Services;

/// <summary>
/// User helpers.
/// </summary>
public class UserService
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<SiteSettings> _siteSettings;

    public UserService(IHttpClientFactory httpClientFactory, IOptions<SiteSettings> siteSettings)
    {
        _siteSettings = siteSettings;
        _httpClient = httpClientFactory.CreateClient();
        if (_httpClient.BaseAddress != _siteSettings.Value.ApiUri)
        {
            _httpClient.BaseAddress = _siteSettings.Value.ApiUri;
        }
    }

    public async Task<UserNewsletterDto?> GetUser(string email, string token)
    {
        var response = await _httpClient.GetAsync($"{_siteSettings.Value.ApiUri.AbsolutePath}/User/User?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}");

        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            return default;
        }
        else if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<UserNewsletterDto>();
        }

        return null;
    }

    public async Task<IList<UserDiaryDto>?> GetWorkouts(string email, string token)
    {
        return await _httpClient.GetFromJsonAsync<List<UserDiaryDto>>($"{_siteSettings.Value.ApiUri.AbsolutePath}/User/Workouts?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}");
    }
}

