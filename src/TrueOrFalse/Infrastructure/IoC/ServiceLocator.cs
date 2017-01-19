﻿using System.Collections.Concurrent;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using NHibernate;
using System.Threading;

public class Sl
{
    public static T Resolve<T>(){return ServiceLocator.Resolve<T>();}
    public static T R<T>() { return ServiceLocator.Resolve<T>(); }

    public static ISession Session => R<ISession>();
    public static UserRepo UserRepo => R<UserRepo>();

    public static int CurrentUserId => R<SessionUser>().UserId;
}

public static class SlExt
{
    public static T R<T>(this object o) => Sl.R<T>();
}

public class ServiceLocator
{
    private static IContainer _container;
    private static readonly ConcurrentDictionary<int /*managed thread id*/, ILifetimeScope> _liftimeScopes = 
        new ConcurrentDictionary<int, ILifetimeScope>();

    public static void Init(IContainer container)
    {
        _container = container;
    }

    public static void AddScopeForCurrentThread(ILifetimeScope lifetimeScope)
    {
        if(!_liftimeScopes.TryAdd(Thread.CurrentThread.ManagedThreadId, lifetimeScope))
            Logg.r().Error("Could not add lifetime scope");    

    }

    public static void RemoveScopeForCurrentThread()
    {
        ILifetimeScope outLifetimeScope;
        if(!_liftimeScopes.TryRemove(Thread.CurrentThread.ManagedThreadId, out outLifetimeScope))
            Logg.r().Error("Could not remove lifetime scope");
    }

    public static T Resolve<T>()
    {
        var currentThreadId = Thread.CurrentThread.ManagedThreadId;
        if (_liftimeScopes.ContainsKey(currentThreadId))
            return _liftimeScopes[currentThreadId].Resolve<T>();

        if (HttpContext.Current == null)
            return _container.Resolve<T>();

        return ((AutofacDependencyResolver) DependencyResolver.Current).RequestLifetimeScope.Resolve<T>();
    }

    public static IContainer GetContainer()
    {
        return _container;
    }

    public static T R<T>(){
        return Resolve<T>();
    }
}