using System;

namespace Agebull.Common.DataModel
{
    /// <summary>
    /// �¼�����
    /// </summary>
    /// <typeparam name="TArgument"></typeparam>
    public class EventArgs<TArgument> : EventArgs
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="arg"></param>
        public EventArgs(TArgument arg)
        {
            Argument = arg;
        }
        /// <summary>
        /// ����
        /// </summary>
        public TArgument Argument { get; }
    }
}