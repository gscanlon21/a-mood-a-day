﻿using Data;
using Quartz;

namespace Api.Jobs.Delete;

public class DeleteOldLogs(ILogger<DeleteOldMoods> logger, CoreContext coreContext) : IJob, IScheduled
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {

        }
        catch (Exception e)
        {
            logger.Log(LogLevel.Error, e, "");
        }
    }

    public static JobKey JobKey => new(nameof(DeleteOldLogs) + "Job", GroupName);
    public static TriggerKey TriggerKey => new(nameof(DeleteOldLogs) + "Trigger", GroupName);
    public static string GroupName => "Delete";

    public static async Task Schedule(IScheduler scheduler)
    {
        var job = JobBuilder.Create<DeleteOldLogs>()
            .WithIdentity(JobKey)
            .Build();

        // Trigger the job every day
        var trigger = TriggerBuilder.Create()
            .WithIdentity(TriggerKey)
            .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(0, 0))
            .Build();

        if (await scheduler.GetTrigger(trigger.Key) != null)
        {
            // Update
            await scheduler.RescheduleJob(trigger.Key, trigger);
        }
        else
        {
            // Create
            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
