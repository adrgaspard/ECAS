﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Volo.Abp.Uow;

namespace ECAS.IdentityService;

/* All test classes are derived from this class, directly or indirectly. */
public abstract class IdentityServiceTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    protected virtual Task WithUnitOfWorkAsync(Func<Task> func)
    {
        return WithUnitOfWorkAsync(new AbpUnitOfWorkOptions(), func);
    }

    protected virtual async Task WithUnitOfWorkAsync(AbpUnitOfWorkOptions options, Func<Task> action)
    {
        using IServiceScope scope = ServiceProvider.CreateScope();
        IUnitOfWorkManager uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

        using IUnitOfWork uow = uowManager.Begin(options);
        await action();

        await uow.CompleteAsync();
    }

    protected virtual Task<TResult> WithUnitOfWorkAsync<TResult>(Func<Task<TResult>> func)
    {
        return WithUnitOfWorkAsync(new AbpUnitOfWorkOptions(), func);
    }

    protected virtual async Task<TResult> WithUnitOfWorkAsync<TResult>(AbpUnitOfWorkOptions options, Func<Task<TResult>> func)
    {
        using IServiceScope scope = ServiceProvider.CreateScope();
        IUnitOfWorkManager uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

        using IUnitOfWork uow = uowManager.Begin(options);
        TResult result = await func();
        await uow.CompleteAsync();
        return result;
    }
}
