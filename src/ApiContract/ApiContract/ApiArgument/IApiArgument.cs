﻿namespace Yizuan.Service.Api
{
    /// <summary>
    /// 表示API请求参数
    /// </summary>
    public interface IApiArgument
    {
        /// <summary>
        /// 转为Form的文本
        /// </summary>
        /// <returns></returns>
        string ToFormString();

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="message">返回的消息</param>
        /// <returns>成功则返回真</returns>
        bool Validate(out string message);
    }
}
