using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageContexts;
using Senparc.Weixin.MP.MessageHandlers;

namespace WeixinSample
{
    public class CustomMessageHandler : MessageHandler<DefaultMpMessageContext>
    {
        private string appId = Senparc.Weixin.Config.SenparcWeixinSetting.MpSetting.WeixinAppId;
        public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0, IServiceProvider serviceProvider = null)
            : base(inputStream, postModel, maxRecordCount, false, null, serviceProvider)
        {
        }

        /// <summary>
        /// 回复以文字形式发送的信息（可选）
        /// </summary>
        public override async Task<IResponseMessageBase> OnTextOrEventRequestAsync(RequestMessageText requestMessage)
        {
            //获取用户信息
            var userInfo = await Senparc.Weixin.MP.AdvancedAPIs.UserApi.InfoAsync(appId, OpenId);
            //发送消息
            await Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendTextAsync(
                appId, OpenId,
                $"欢迎{userInfo.nickname}(这是一条来自 Ms Build 2021 现场的异步的客服消息) - {SystemTime.Now}");
            //返回消息
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = $"你发送了文字：{requestMessage.Content}\r\n\r\n你的OpenId：{OpenId}";//以文字类型消息回复
            return responseMessage;
        }

        public override async Task<IResponseMessageBase> OnImageRequestAsync(RequestMessageImage requestMessage)
        {
            await Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendTextAsync(
                appId, OpenId, $"您发送了图片：");
            var responseMessage = base.CreateResponseMessage<ResponseMessageImage>();
            responseMessage.Image = new Image() { MediaId = requestMessage.MediaId };
            return responseMessage;
        }

        /// <summary>
        /// 默认消息
        /// </summary>
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = $"欢迎关注微软 Build 2021！当前时间：{SystemTime.Now}";//没有自定义的消息统一回复固定消息
            return responseMessage;
        }
    }
}
