using Senparc.NeuChar.App.AppStore;
using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageContexts;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SampleForBuild
{
    /// <summary>
    /// 微信公众号消息处理
    /// </summary>
    public class CustomMessageHandler : MessageHandler<DefaultMpMessageContext>
    {
        public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0, bool onlyAllowEncryptMessage = false, DeveloperInfo developerInfo = null, IServiceProvider serviceProvider = null) : base(inputStream, postModel, maxRecordCount, onlyAllowEncryptMessage, developerInfo, serviceProvider)
        {
        }

        public override async Task<IResponseMessageBase> DefaultResponseMessageAsync(IRequestMessageBase requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = $"(异步)您发送了一条未处理的信息，类型：{requestMessage.MsgType}";


            try
            {
                throw new Exception("测试异常");
            }
            catch (Exception e)
            {
                Senparc.CO2NET.Trace.SenparcTrace.BaseExceptionLog(e);
            }

            var requestMessages = (await base.GetCurrentMessageContext()).RequestMessages;
            if (requestMessages?.Count >=2 && requestMessages[requestMessages.Count - 2] is RequestMessageText lastRequst)
            {
                responseMessage.Content += $"\r\n上一条信息文字内容：{lastRequst.Content}";
            }
            return responseMessage;
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = $"（同步）您发送了一条未处理的信息，类型：{requestMessage.MsgType}";

            Senparc.CO2NET.Trace.SenparcTrace.SendCustomLog("MyDebug", responseMessage.Content);

            var requestMessages = (base.GetCurrentMessageContext().ConfigureAwait(false).GetAwaiter().GetResult()).RequestMessages;

            if (requestMessages?.Count >= 2 && requestMessages[requestMessages.Count - 2] is RequestMessageText lastRequst)
            {
                responseMessage.Content += $"\r\n上一条信息文字内容：{lastRequst.Content}";
            }

            return responseMessage;
        }
    }
}
