// See https://aka.ms/new-console-template for more information
using Quartz;
using Quartz.Impl;
using QuartzNetConsole;

//Console.WriteLine("Quartz.NET 初體驗!");
RunProgram().GetAwaiter().GetResult();

static async Task RunProgram()
{
	try
	{
		// 建立 scheduler
		StdSchedulerFactory factory = new StdSchedulerFactory();
		IScheduler scheduler = await factory.GetScheduler();

		// 建立 Job
		IJobDetail job = JobBuilder.Create<ShowDataTimeJob>()
			.WithIdentity("trigger1", "group1")
			.Build();

		// 建立 Trigger，每秒跑一次
		ITrigger trigger = TriggerBuilder.Create()
			.WithIdentity("trigger1", "group1")
			.StartNow()
			.WithSimpleSchedule(x => x
			.WithIntervalInSeconds(1)
			.RepeatForever())
			.Build();

		// 加入 ScheduleJob 中
		await scheduler.ScheduleJob(job, trigger);

		// 啟動
		await scheduler.Start();

		// 執行 10 秒
		await Task.Delay(TimeSpan.FromSeconds(10));

        // say goodbye
		await scheduler.Shutdown();
    }
    catch (Exception ex)
	{
        await Console.Error.WriteLineAsync(ex.ToString());
    }
}