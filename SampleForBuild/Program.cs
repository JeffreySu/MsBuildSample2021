using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SampleForBuild;
using Senparc.CO2NET;
using Senparc.CO2NET.AspNet;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.MessageHandlers.Middleware;
using Senparc.Weixin.RegisterServices;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddMemoryCache()//使用本地缓存
       .AddSenparcWeixinServices(builder.Configuration)//Senparc.Weixin 注册
       .AddOptions();

await using var app = builder.Build();

//读取配置
var senparcSetting = app.Services.GetService<IOptions<SenparcSetting>>().Value;
var senparcWeixinSetting = app.Services.GetService<IOptions<SenparcWeixinSetting>>().Value;

//注册 CO2NET 和 Senparc.Weixin SDK
var registerService =
    app.UseSenparcGlobal(app.Environment, senparcSetting, g =>
    {
        //使用配置好的Redis缓存
        Senparc.CO2NET.Cache.CsRedis.Register.SetConfigurationOption(senparcSetting.Cache_Redis_Configuration);
        Senparc.CO2NET.Cache.CsRedis.Register.UseKeyValueRedisNow();
    }, true)
       .UseSenparcWeixin(senparcWeixinSetting, weixinRegister => weixinRegister.RegisterMpAccount(senparcWeixinSetting));

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/", (Func<string>)(() => "Hello Build Workshop & WeChat!"));

app.MapGet("/MyApi", (Func<Task<object>>)(async () =>
{
    //Senparc.CO2NET.Cache.CsRedis.Register.SetConfigurationOption("172.16.13.108:16333");//自定义缓存

    var dt = DateTime.Now.ToString();
    var cache = Senparc.CO2NET.Cache.CacheStrategyFactory.GetObjectCacheStrategyInstance();
    await cache.SetAsync("Time", dt);

    return new
    {
        Time = dt,
        MyTime = "MyTime is " + (await cache.GetAsync<string>("Time"))
    };
}));

//微信公众号消息处理中间件
app.UseMessageHandlerForMp("/WeixinBuild",
    (stream, postModel, maxRecordCount, serviceProvider) => new CustomMessageHandler(stream, postModel, maxRecordCount, serviceProvider: app.Services),
    options =>
    {
        options.AccountSettingFunc = context => senparcWeixinSetting;
    });

await app.RunAsync();
