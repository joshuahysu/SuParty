//using Huede.FMC2.Aspects;
//using Newtonsoft.Json;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;
//using RabbitMQ.Client.Exceptions;
//using System;
//using System.Text;
//using System.Timers;

//namespace Huede.FMC2.Communications.MessageQueues
//{
//    /// <summary>
//    /// RabbitMQ的最小執行單元
//    /// </summary>
//    /// <typeparam name="T">訊息類型</typeparam>
//    [Aspects.GeneralAspect]
//    internal class RabbitMQUnit<T>
//    {
//        #region Constants

//        /// <summary>
//        /// 定義UTF-8編碼
//        /// </summary>
//        private static readonly Encoding UTF8 = Encoding.UTF8;

//        /// <summary>
//        /// 定義永遠產生新物件的Json序列化設定
//        /// </summary>
//        private static readonly JsonSerializerSettings CREATE_NEW = new JsonSerializerSettings() { ObjectCreationHandling = ObjectCreationHandling.Replace };

//        /// <summary>
//        /// 定義訊息佇列的發送確認逾時為5秒
//        /// </summary>
//        private static readonly TimeSpan TIMEOUT = new TimeSpan(0, 0, 5);

//        /// <summary>
//        /// 定義自動重連的計時器
//        /// </summary>
//        private readonly Timer RECONNECTOR;

//        /// <summary>
//        /// 定義RabbitMQ的連結產生器
//        /// </summary>
//        private readonly IConnectionFactory FACTORY;

//        /// <summary>
//        /// 定義輸入佇列名稱
//        /// </summary>
//        private readonly string INPUT_QUEUE;

//        /// <summary>
//        /// 定義輸出通道名稱
//        /// </summary>
//        private readonly string OUTPUT_EXCHANGE;

//        /// <summary>
//        /// 定義輸出通道金鑰
//        /// </summary>
//        private readonly string OUTPUT_KEY;

//        /// <summary>
//        /// 定義佇列的服務品質 (實際上為尚未ack的訊息的數量上限)
//        /// </summary>
//        private readonly ushort QOS;

//        #endregion

//        #region Event

//        /// <summary>
//        /// 當<see cref="RabbitMQ"/>收到可解析為<see cref="T"/>的訊息時，觸發此事件
//        /// </summary>
//        public event EventHandler<EventArgs<T>> MessageReceived;

//        /// <summary>
//        /// 觸發<see cref="MessageReceived"/>事件
//        /// </summary>
//        /// <param name="msg">收到的訊息</param>
//        protected void OnMessageReceived(T msg)
//        {
//            MessageReceived?.Invoke(this, new EventArgs<T>(msg));
//        }

//        /// <summary>
//        /// 重連事件
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void OnConnectionShutdown(object sender, ShutdownEventArgs e)
//        {
//            RECONNECTOR.Start();
//        }
//        #endregion

//        #region Initialize & Finalize

//        /// <summary>
//        /// 建立基於RabbitMQ(AMQP)的訊息佇列
//        /// </summary>
//        public RabbitMQUnit(IConnectionFactory factory, string queue, string exchange, string key, ushort qos)
//        {
//            //建立自動重連用的timer
//            RECONNECTOR = new Timer(5000) { AutoReset = true };
//            RECONNECTOR.Elapsed += RECONNECTOR_Elapsed;

//            //在本地儲存常用的組態值
//            FACTORY = factory;
//            INPUT_QUEUE = queue;
//            OUTPUT_EXCHANGE = exchange;
//            OUTPUT_KEY = key;
//            QOS = qos;

//            //建立長連結
//            if (!Connect())
//                RECONNECTOR.Start();
//        }

//        /// <summary>
//        /// 終止通訊並棄置MQ
//        /// </summary>
//        public void Dispose()
//        {
//            if (_channel != null && _handler != null)
//            {
//                _handler.Received -= Consumer_Received;
//                _channel.BasicCancel(_handler.ConsumerTag);
//                _channel.Close();
//                _channel = null;
//                _handler = null;
//            }
//            if (_connection != null)
//            {
//                _connection.Close();
//                _connection = null;
//            }
//        }

//        #endregion

//        #region Connection

//        /// <summary>
//        /// MQ連線元件
//        /// </summary>
//        private IConnection _connection;

//        /// <summary>
//        /// MQ通訊管道
//        /// </summary>
//        private IModel _channel;

//        /// <summary>
//        /// 輸入佇列的接收資料處理器
//        /// </summary>
//        private EventingBasicConsumer _handler;

//        /// <summary>
//        /// 預設的佇列資料屬性
//        /// </summary>
//        private IBasicProperties _property;

//        /// <summary>
//        /// 嘗試連線至RabbitMQ Host
//        /// </summary>
//        /// <returns>成功與否</returns>
//        private bool Connect()
//        {
//            if (_connection != null && _connection.IsOpen) return true;    //已連線，直接回傳

//            //0. 註銷之前的事件處理程序
//            if (_handler != null)
//            {
//                _handler.Received -= Consumer_Received;
//                try
//                {
//                    _channel?.BasicCancel(_handler.ConsumerTag);
//                }
//                catch (AlreadyClosedException ace)
//                {
//                    Trace($"{nameof(_channel.BasicCancel)}失敗：{ace.Message}");
//                }
//                catch (Exception ex)
//                {
//                    Warn($"{nameof(_channel.BasicCancel)}失敗：{ex.ToString()}");
//                }
//            }

//            //1. 建立連線、通道、預設屬性
//            _connection = FACTORY.CreateConnection();
//            if (!_connection.IsOpen) return false;
//            if (_channel != null&& _channel.IsOpen) {
//                _channel.Close();
//            }
//            _channel = _connection.CreateModel();
//            _channel.BasicQos(0, QOS, true);        //只要還有超過qos個訊息un-ack就不繼續收資料
//            _channel.ConfirmSelect();
//            _property = _channel.CreateBasicProperties();
//            _property.Persistent = true;

//            //2. 建立自動取出資料 & 處理程序
//            _handler = new EventingBasicConsumer(_channel);
//            _handler.Received += Consumer_Received;
//            _channel.BasicConsume(INPUT_QUEUE, false, _handler);

//            //3. 當connection被催毀時自動安排重連
//            _connection.ConnectionShutdown -= OnConnectionShutdown;
//            _connection.ConnectionShutdown += OnConnectionShutdown;
//            return true;
//        }

//        /// <summary>
//        /// 重複嘗試重新連線至RabbitMQ Host
//        /// </summary>
//        private void RECONNECTOR_Elapsed(object sender, ElapsedEventArgs e)
//        {
//            //1. 無論如何先停止Timer，以防止在重連工作完成前，又再次觸發Elapsed
//            RECONNECTOR.Stop();

//            bool success = false;
//            try
//            {
//                //2. 關閉後嘗試重連
//                _connection.Close();                
//                success = Connect();
//            }
//            catch (Exception ex)
//            {
//                Trace($"MQ重連失敗：{ex.Message}");
//            }
//            finally
//            {
//                //3. 若重連未成功，則再次啟動Timer，進行下一輪重試
//                if (!success)
//                    RECONNECTOR.Start();
//            }
//        }

//        #endregion

//        #region Read & Write

//        /// <summary>
//        /// 發送訊息至MQ
//        /// </summary>
//        /// <param name="message">訊息</param>
//        /// <returns>成功與否</returns>
//        public bool SendMessage(T message)
//        {
//            _channel.BasicPublish(OUTPUT_EXCHANGE, OUTPUT_KEY, _property, UTF8.GetBytes(JsonConvert.SerializeObject(message)));
//            bool ack = _channel.WaitForConfirms(TIMEOUT, out bool timeout);
//            return ack && !timeout;
//        }

//        /// <summary>
//        /// 處理從MQ收到的訊息
//        /// </summary>
//        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
//        {
//            try
//            {
//                T msg = JsonConvert.DeserializeObject<T>(UTF8.GetString(e.Body), CREATE_NEW);
//                if (msg == null) return;
//                OnMessageReceived(msg);
//            }
//            finally
//            {
//                try
//                {
//                    if (_channel != null && _channel.IsOpen)
//                        _channel.BasicAck(e.DeliveryTag, false);
//                }
//                //ack失敗，因為別的thread已經呼叫Dispose，不予處理
//                catch (NullReferenceException ex)
//                {
//                    Warn(ex.ToString());
//                }
//                //ack失敗，通常是在關閉連線，不予處理
//                catch (AlreadyClosedException ex)
//                {
//                    Warn(ex.ToString());
//                }
//                catch (Exception ex) {
//                    Warn(ex.ToString());
//                }
//            }
//        }

//        #endregion

//        /// <summary>
//        /// 透過FMCLog寫入追蹤日誌
//        /// </summary>
//        /// <param name="log">日誌內容</param>
//        private static void Trace(string log) => Logs.Logger.Trace("Communication", nameof(RabbitMQUnit<T>), log);

//        private static void Warn(string log) => Logs.Logger.Warn("Communication", nameof(RabbitMQUnit<T>), log);
//    }
//}
