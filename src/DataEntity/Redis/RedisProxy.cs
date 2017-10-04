using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq.Expressions;
using System.Threading;
using Agebull.Common.Base;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.MySql;
using NServiceKit.Redis;

namespace Agebull.Common.DataModel.Redis
{
    /// <summary>
    /// REDIS������
    /// </summary>
    public class RedisProxy : IDisposable
    {
        #region ����

        /// <summary>
        /// ��̬����
        /// </summary>
        static RedisProxy()
        {
            var c = ConfigurationManager.AppSettings["Redis"].Split(',', ':');
            Address = c[0];
            Port = c.Length > 1 ? int.Parse(c[1]) : 6379;
            //PoolSize = Convert.ToInt32(ConfigurationManager.AppSettings["RedisPoolSize"]);
            //if (PoolSize < 100)
            //    PoolSize = 100;
            if (c.Length > 2)
                PassWord = c[2];
        }

        /// <summary>
        /// ��ַ
        /// </summary>
        public static readonly string Address;

        /// <summary>
        /// ����
        /// </summary>
        static readonly string PassWord;
        /// <summary>
        /// �˿�
        /// </summary>
        public static readonly int Port;
        /*// <summary>
        /// ����������
        /// </summary>
        private static readonly int PoolSize;*/

        /// <summary>
        /// ϵͳ����
        /// </summary>
        public const int DbSystem = 15;

        /// <summary>
        /// WEB�˵Ļ���
        /// </summary>
        public const int DbWebCache = 14;

        /// <summary>
        /// Ȩ������
        /// </summary>
        public const int DbAuthority = 13;

        /// <summary>
        /// WEB�˵Ļ���
        /// </summary>
        public const int DbComboCache = 12;


        #endregion

        /// <summary>
        /// ������
        /// </summary>
        private static readonly object LockObj = new object();
        /// <summary>
        /// ʹ���ĸ����ݿ�
        /// </summary>
        private readonly int _db;

        /// <summary>
        /// ��ǰ���ݿ�
        /// </summary>
        public long CurrentDb => Client.Db;

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="db"></param>
        public RedisProxy(int db = 0)
        {
            _db = db;
        }

        /*// <summary>
        /// ʹ���е�
        /// </summary>
        private static readonly Dictionary<int, List<RedisClient>> Used = new Dictionary<int, List<RedisClient>>();
        /// <summary>
        /// ���е�
        /// </summary>
        private static readonly Dictionary<int, List<RedisClient>> Idle = new Dictionary<int, List<RedisClient>>();*/
        /// <summary>
        /// �ͻ�����
        /// </summary>
        private RedisClient _client;
        /// <summary>
        /// �õ�һ�����õ�Redis�ͻ���
        /// </summary>
        public RedisClient Client
        {
            get
            {
                if (_client != null)
                    return _client;
                Monitor.Enter(LockObj);
                try
                {
                    //var dbid = (this._db << 16);
                    return _client = new RedisClient(Address, Port, PassWord, _db)
                    {
                        RetryCount = 50,
                        RetryTimeout = 5000
                    };
                    /*List<RedisClient> used;
                    if (!Used.ContainsKey(dbid))
                    {
                        Used.Add(dbid, used = new List<RedisClient>());
                        Idle.Add(dbid, new List<RedisClient>());

                        _client = new RedisClient(Address, Port, null, _db)
                        {
                            RetryCount = 50,
                            RetryTimeout = 5000
                        };
                        used.Add(_client);
                        return Client;
                    }
                    used = Used[dbid];
                    List<RedisClient> idle = Idle[dbid];
                    while (idle.Count > 0 && idle[0] == null)
                    {
                        idle.RemoveAt(0);
                    }
                    if (idle.Count == 0)
                    {
                        _client = new RedisClient(Address, Port, null, _db)
                        {
                            RetryCount = 50,
                            RetryTimeout = 5000
                        };
                        used.Add(_client);
                        return _client;
                    }
                    _client = idle[0];
                    idle.RemoveAt(0);
#if DEBUG
                    _client.Ping();
#endif
                    used.Add(_client);
                    return _client;*/
                }
                finally
                {
                    Monitor.Exit(LockObj);
                }
            }
        }


        public void Dispose()
        {
            if (_client == null)
                return;
            Monitor.Enter(LockObj);
            try
            {
                //int dbid = (_db << 16);
                //Used[dbid].Remove(_client);
                _client.ResetSendBuffer();
                //if (_client.HadExceptions || Idle[dbid].Count > PoolSize)
                {
                    _client.Quit();
                    _client.Dispose();
                }
                //else
                //{
                //    Idle[dbid].Add(_client);
                //}
            }
            finally
            {
                Monitor.Exit(LockObj);
            }
        }
        /// <summary>
        /// ɾ��KEY
        /// </summary>
        /// <param name="key"></param>
        public void RemoveKey(string key)
        {
            Client.Remove(key);
        }
        /// <summary>
        /// ����ļ�
        /// </summary>
        private static readonly Dictionary<string, List<string>> KeysDictionary = new Dictionary<string, List<string>>();

        /// <summary>
        /// ���ҹ�ɾ��KEY
        /// </summary>
        /// <param name="condition">����</param>
        public void FindAndRemoveKey(string condition)
        {
            if (!KeysDictionary.ContainsKey(condition))
                KeysDictionary[condition] = Client.SearchKeys(condition);
            foreach (var key in KeysDictionary[condition])
                Client.Remove(key);
        }

        /// <summary>
        /// ʹ�ü���Ϸ�ʽ��ȡֵ
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="id">�������</param>
        /// <param name="def">Ĭ��ֵ</param>
        /// <returns></returns>
        public TData GetEntity<TData>(int id, TData def=null) where TData : class, new()
        {
            return id == 0 ? def ?? new TData() : Client.Get<TData>(DataKey<TData>(id)) ?? def ?? new TData() ;
        }

        /// <summary>
        /// ʹ�ü���Ϸ�ʽ��ȡֵ
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="key">���ݼ�</param>
        /// <param name="def">Ĭ��ֵ</param>
        /// <returns></returns>
        public TData GetEntity<TData>(string key,TData def = null) where TData : class, new()
        {
            return Client.Get<TData>(key) ?? def ?? new TData();
        }

        /// <summary>
        /// ʹ�ü���Ϸ�ʽ��ȡֵ
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data">�������</param>
        /// <returns></returns>
        public void SetEntity<TData>(TData data) where TData : class, IIdentityData
        {
            Client.Set(DataKey(data), data);
        }

        /// <summary>
        /// ������Щ����
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="datas">����</param>
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns></returns>
        public void CacheData<TData>(IEnumerable<TData> datas, Func<TData, string> keyFunc = null) where TData : class, IIdentityData, new()
        {
            if (keyFunc == null)
                keyFunc = DataKey;
            var key = keyFunc(new TData());
            Client.Set(key, DateTime.Now);
            foreach (var data in datas)
            {
                Client.Set(keyFunc(data), data);
            }
        }

        /// <summary>
        /// ������Щ����
        /// </summary>
        /// <typeparam name="TDataAccess"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns></returns>
        public void CacheData<TData, TDataAccess>(Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            var access = new TDataAccess();
            var datas = access.All();
            CacheData(datas, keyFunc);
        }

        /// <summary>
        /// ȡ�ı�
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            var bytes = Client.Get(key);
            return bytes?.BytesToString();
        }

        /// <summary>
        /// д�ı�
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetValue(string key, string value)
        {
            if (value == null)
                Client.Remove(key);
            else
                Client.Set(key, value.ToByte());
        }
        /// <summary>
        /// Ĭ�ϵ����ݼ�����������
        /// </summary>
        /// <typeparam name="TData">���ݼ�</typeparam>
        /// <param name="data">����</param>
        /// <returns>���ݼ���</returns>
        public static string DataKey<TData>(TData data) where TData : class, IIdentityData
        {
            return $"data:{typeof(TData).Name.ToLower()}:{data.Id}";
        }

        /// <summary>
        /// Ĭ�ϵ����ݼ�����������
        /// </summary>
        /// <typeparam name="TData">���ݼ�</typeparam>
        /// <param name="id">���ݼ�</param>
        /// <returns>���ݼ���</returns>
        public static string DataKey<TData>(object id)
        {
            return $"data:{typeof(TData).Name.ToLower()}:{id}";
        }
        /// <summary>
        /// Ĭ�ϵ����ݼ�����������
        /// </summary>
        /// <param name="type">����</param>
        /// <param name="id">���ݼ�</param>
        /// <returns>���ݼ���</returns>
        public static string DataKey(string type, int id)
        {
            return $"data:{type.ToLower()}:{id}";
        }
        /// <summary>
        /// ������Щ����
        /// </summary>
        /// <typeparam name="TDataAccess"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns></returns>
        public void TryCacheData<TData, TDataAccess>(Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            var key = DataKey<TData>(0);
            var date = Client.Get<DateTime>(key);
            if (date == DateTime.MinValue)
                CacheData<TData, TDataAccess>(keyFunc);
        }

        /// <summary>
        ///     ���û�л������Щ����,�ͻ�����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns>����</returns>
        public void TryCacheData<TData, TDataAccess>(Expression<Func<TData, bool>> lambda, Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            var key = DataKey<TData>(0);
            var date = Client.Get<DateTime>(key);
            if (date == DateTime.MinValue)
                CacheData<TData, TDataAccess>(lambda, keyFunc);
        }

        /// <summary>
        ///     ������Щ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <param name="keyFunc">���ü��ķ���,��Ϊ��</param>
        /// <returns>����</returns>
        public void CacheData<TData, TDataAccess>(Expression<Func<TData, bool>> lambda, Func<TData, string> keyFunc = null)
            where TDataAccess : MySqlTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            var access = new TDataAccess();
            var datas = access.All(lambda);
            CacheData(datas, keyFunc);
        }

        /// <summary>
        ///     ������Щ����
        /// </summary>
        /// <returns>����</returns>
        public void ClearCache<TData>()
        {
            Client.Delete(Client.SearchKeys(DataKey<TData>("*")));
        }

        /// <summary>
        ///     ������Щ����
        /// </summary>
        /// <returns>����</returns>
        public void RefreshCache<TData, TDataAccess>(int id, Func<TData, bool> lambda = null)
            where TDataAccess : MySqlTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            if (id <= 0)
                return;
            var access = new TDataAccess();
            var data = access.LoadByPrimaryKey(id);
            if (data != null && (lambda == null || lambda(data)))
                Client.Set(DataKey(data), data);
            else
                Client.Remove(DataKey<TData>(id));
        }

        /// <summary>
        ///     ������Щ����
        /// </summary>
        /// <returns>����</returns>
        public void RemoveCache<TData>(int id)
        {
            if (id > 0)
                Client.Remove(DataKey<TData>(id));
        }
    }
    /// <summary>
    /// Redis��Db��Χ
    /// </summary>
    public class RedisDbScope : ScopeBase 
    {
        private readonly RedisProxy _proxy;
        private readonly long _db;

        public static RedisDbScope CreateScope(RedisProxy proxy, long db)
        {
            return new RedisDbScope(proxy, db);
        }

        private RedisDbScope(RedisProxy proxy, long db)
        {
            _proxy = proxy;
            if (db == proxy.CurrentDb)
                _db = -1;
            else
            {
                _db = proxy.CurrentDb;
                _proxy.Client.Db = db;
            }
        }

        /// <summary>������Դ</summary>
        protected override void OnDispose()
        {
            if (_db >= 0)
            {
                _proxy.Client.Db = _db;
            }
        }
    }
}