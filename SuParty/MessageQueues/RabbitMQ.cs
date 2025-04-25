//using Huede.FMC2.Aspects;
//using RabbitMQ.Client;
//using System;

//namespace Huede.FMC2.Communications.MessageQueues
//{
//    /// <summary>
//    /// 透過RabbitMQ進行佇列訊息交換
//    /// </summary>
//    /// <typeparam name="T">訊息類型</typeparam>
//    [GeneralAspect]
//    public class RabbitMQ<T> : IMessageQueue<T>
//    {
//        #region Constants (偽)

//        /// <summary>
//        /// 實際與RabbitMQ通訊的客戶單元
//        /// </summary>
//        private RabbitMQUnit<T>[] UNITS;

//        #endregion

//        #region Events

//        /// <summary>
//        /// 當<see cref="RabbitMQ"/>收到可解析為<see cref="T"/>的訊息時，觸發此事件
//        /// </summary>
//        public event EventHandler<EventArgs<T>> MessageReceived;

//        /// <summary>
//        /// 觸發<see cref="MessageReceived"/>事件
//        /// </summary>
//        /// <param name="msg">收到的訊息</param>
//        protected void OnMessageReceived(EventArgs<T> e)
//        {
//            MessageReceived?.Invoke(this, e);
//        }

//        #endregion

//        #region Initialize & Finalize

//        /// <summary>
//        /// 根據組態初始化Rabbit MQ
//        /// </summary>
//        /// <param name="config">訊息佇列的組態</param>
//        public bool Initialize(Configs.MessageQueueConfig config)
//        {
//            //在本地儲存常用的組態值
//            IConnectionFactory factory = new ConnectionFactory { HostName = config.Host, Port = 5672, UserName = config.User, Password = config.Password };
//            string queue = config.Input.Queue;
//            string exchange = config.Output.Exchange;
//            string key = config.Output.Key;

//            //嘗試取出AMQP的 QOS 與 Client數量
//            ushort qos = 10000;
//            int count = 10;
//            if (config.Others != null)
//            {
//                if (config.Others.ContainsKey("QOS"))
//                    ushort.TryParse(config.Others["QOS"], out qos);
//                if (config.Others.ContainsKey("Count"))
//                    int.TryParse(config.Others["Count"], out count);
//            }
//            if (count < 1) count = 1;

//            //建立units
//            UNITS = new RabbitMQUnit<T>[count];
//            for (int i = 0; i < count; i++)
//            {
//                //注意: 用thread pool快速開連線很容易被MQ拒連?
//                UNITS[i] = new RabbitMQUnit<T>(factory, queue, exchange, key, qos);
//                UNITS[i].MessageReceived += (s, e) => OnMessageReceived(e);
//            }
//            return true;
//        }

//        /// <summary>
//        /// 終止通訊並棄置MQ
//        /// </summary>
//        public void Dispose()
//        {
//            if (UNITS == null) return;
//            foreach (RabbitMQUnit<T> unit in UNITS)
//                unit.Dispose();
//            UNITS = null;
//        }

//        /// <summary>
//        /// 發送訊息至MQ
//        /// </summary>
//        /// <param name="message">訊息</param>
//        /// <returns>成功與否</returns>
//        public bool SendMessage(T message)
//        {
//            return UNITS[0].SendMessage(message);
//        }

//        #endregion
//    }
//}
