var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddMemoryCache()//ʹ�ñ��ػ���
       .AddSenparcWeixinServices(builder.Configuration)//Senparc.Weixin ע��
       .AddOptions();

await using var app = builder.Build();

//��ȡ����
var senparcSetting = app.Services.GetService<IOptions<SenparcSetting>>().Value;
var senparcWeixinSetting = app.Services.GetService<IOptions<SenparcWeixinSetting>>().Value;

//ע�� CO2NET �� Senparc.Weixin SDK
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
