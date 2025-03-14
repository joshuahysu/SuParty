using System;

namespace Huede.FMC2.Communications.MessageQueues
{
    /// <summary>
    /// 定義基於AMQP的訊息佇列的通用介面
    /// </summary>
    /// <typeparam name="T">訊息類型</typeparam>
    public interface IMessageQueue<T> : IDisposable
    {
        /// <summary>
        /// 當從佇列中取得新訊息時，觸發此事件
        /// </summary>
        event EventHandler<EventArgs<T>> MessageReceived;

        /// <summary>
        /// 初始化訊息佇列
        /// </summary>
        /// <param name="config">訊息佇列組態</param>
        /// <returns>成功與否</returns>
        bool Initialize(Configs.MessageQueueConfig config);

        /// <summary>
        /// 發送訊息至佇列
        /// </summary>
        /// <param name="message">訊息內容</param>
        /// <returns>成功與否</returns>
        bool SendMessage(T message);
    }
}
