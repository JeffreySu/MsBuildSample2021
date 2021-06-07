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
    app.UseSenparcGlobal(app.Environment, senparcSetting, _ => { }, true)
        .UseSenparcWeixin(senparcWeixinSetting, weixinRegister => weixinRegister.RegisterMpAccount(senparcWeixinSetting));

app.MapGet("/", (Func<string>)(() => "Welcome to use Senparc.Weixin SDK!"));

app.UseMessageHandlerForMp("/WeixinAsync",
(stream, postModel, maxRecordCount, serviceProvider) => new CustomMessageHandler(stream, postModel, maxRecordCount, serviceProvider),
        options =>
        {
            options.AccountSettingFunc = context => senparcWeixinSetting;
        });

await app.RunAsync();
