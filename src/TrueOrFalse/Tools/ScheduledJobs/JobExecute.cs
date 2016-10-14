﻿using System;
using System.Diagnostics;
using System.Threading;
using Autofac;
using NHibernate;
using RollbarSharp;

public class JobExecute
{
    [ThreadStatic] public static bool CodeIsRunningInsideAJob;

    public static void Run(Action<ILifetimeScope> action, string jobName, bool writeLog = true)
    {
        try
        {
            using (var scope = ServiceLocator.GetContainer().BeginLifetimeScope(jobName))
            {
                try
                {
                    ServiceLocator.AddScopeForCurrentThread(scope);

                    CodeIsRunningInsideAJob = true;
                    Settings.UseWebConfig = true;

                    if (IsJobRunning(jobName, scope))
                        return;

                    try
                    {
                        var stopwatch = Stopwatch.StartNew();
                         
                        var threadId = Thread.CurrentThread.ManagedThreadId;
                        var appDomainName = AppDomain.CurrentDomain.FriendlyName;

                        if (writeLog)
                            Logg.r()
                            	.Information("JOB START: {Job}, AppDomain(Hash): {AppDomain}, Thread: {ThreadId}",
                                jobName,
                                appDomainName.GetHashCode().ToString("x"),
                                threadId);
                               
                        action(scope);

                        if (writeLog)
                            Logg.r()
                                .Information("JOB END: {Job}, AppDomain(Hash): {AppDomain}, Thread: {ThreadId} {timeNeeded}", 
                                    jobName,
                                    appDomainName.GetHashCode().ToString("x"),
                                    threadId,
                                    stopwatch.Elapsed);

                        stopwatch.Stop();
                    }
                    finally
                    {
                        CLoseJob(jobName, scope);
                    }
                    }
                    finally
                {
                    ServiceLocator.RemoveScopeForCurrentThread();
                }
            }
        }
        catch (Exception e)
        {
            Logg.r().Error(e, "Job error on " + jobName);

            if (!String.IsNullOrEmpty(Settings.RollbarAccessToken))
                new RollbarClient().SendException(e);
        }
    }

    private static bool IsJobRunning(string jobName, ILifetimeScope scope)
    {
        using (new MutexX(5000, "IsRunning"))
        {
            using (var session = scope.R<ISessionFactory>().OpenSession())
            {
                var runningJobRepo = new RunningJobRepo(session);
                if (runningJobRepo.IsJobRunning(jobName))
                {
                    Logg.r().Information("Job is already running: {jobName}", jobName);
                    return true;
                }

                runningJobRepo.Add(jobName);
            }
            return false;
        }
    }

    private static void CLoseJob(string jobName, ILifetimeScope scope)
    {
        using (var session = scope.R<ISessionFactory>().OpenSession())
        {
            new RunningJobRepo(session).Remove(jobName);
        }
    }
}