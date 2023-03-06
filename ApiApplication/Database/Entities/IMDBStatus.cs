using System;

namespace ApiApplication.Database.Entities
{
    public sealed class IMDBStatus
    {
        public bool Up { get; set; }
        public DateTime LastCall { get; set; }

        private static readonly object _lock = new object();
        private static IMDBStatus _instance = null;
        public static IMDBStatus Instance
        {
            get
            {
                lock (_lock)
                {
                    _instance ??= new IMDBStatus();
                }

                return _instance;
            }
        }
    }
}
