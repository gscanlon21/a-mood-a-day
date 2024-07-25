using Api.Code;
using Api.Controllers;
using Api.Jobs.Create;
using Api.Jobs.Delete;
using Api.Jobs.Update;
using Api.Mail.Azure;
using Api.Services;
using Core.Code;
using Core.Models.Options;
using Data;
using Data.Repos;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCustomEnvironmentVariables();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<CoreContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CoreContext") ?? throw new InvalidOperationException("Connection string 'CoreContext' not found."), options =>
    {
        options.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
    }));

builder.Services.AddTransient<UserController>();
builder.Services.AddTransient<NewsletterController>();

builder.Services.AddTransient<UserRepo>();
builder.Services.AddTransient<NewsletterRepo>();

builder.Services.AddSingleton<IMailSender, AzureMailSender>();
builder.Services.AddHostedService<EmailSenderService>();

builder.Services.Configure<SiteSettings>(
    builder.Configuration.GetSection("SiteSettings")
);

builder.Services.Configure<DigitalOceanSettings>(
    builder.Configuration.GetSection("DigitalOceanSettings")
);

builder.Services.Configure<AzureSettings>(
    builder.Configuration.GetSection("AzureSettings")
);

builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("SmtpSettings")
);

builder.Services.Configure<FeatureSettings>(
    builder.Configuration.GetSection("FeatureSettings")
);

builder.Services.Configure<QuartzOptions>(options =>
{
    options.Scheduling.IgnoreDuplicates = true;
    options.Scheduling.OverWriteExistingData = true;
});
builder.Services.AddQuartz(q =>
{
    q.UsePersistentStore(x =>
    {
        x.UseProperties = true;
        x.UsePostgres(builder.Configuration.GetConnectionString("QuartzContext") ?? throw new InvalidOperationException("Connection string 'CoreContext' not found."));
        x.UseSerializer<Quartz.Simpl.JsonObjectSerializer>();
    });
});
builder.Services.AddQuartzHostedService(opt =>
{
    opt.WaitForJobsToComplete = true;
});

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var schedulerFactory = app.Services.GetRequiredService<ISchedulerFactory>();
var scheduler = await schedulerFactory.GetScheduler();

await CreateEmails.Schedule(scheduler);
await CreateMoods.Schedule(scheduler);
await DisableInactiveUsers.Schedule(scheduler);
await DisableErroredUsers.Schedule(scheduler);
await DeleteOldMoods.Schedule(scheduler);
await DeleteOldEmails.Schedule(scheduler);
await DeleteOldLogs.Schedule(scheduler);
await DeleteInactiveUsers.Schedule(scheduler);
await DeleteOldTokens.Schedule(scheduler);

app.Run();
