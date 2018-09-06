namespace SmartEP.Service.Core.Cached
{
    using System;
    using System.Net.Sockets;
    using System.Text;

    public class NetworkStreamIgnoreSeek : NetworkStream
    {
        #region Constructors

        public NetworkStreamIgnoreSeek(Socket socket, System.IO.FileAccess access, bool ownsSocket)
            : base(socket, access, ownsSocket)
        {
        }

        public NetworkStreamIgnoreSeek(Socket socket, System.IO.FileAccess access)
            : base(socket, access)
        {
        }

        public NetworkStreamIgnoreSeek(Socket socket, bool ownsSocket)
            : base(socket, ownsSocket)
        {
        }

        public NetworkStreamIgnoreSeek(Socket socket)
            : base(socket)
        {
        }

        #endregion Constructors

        #region Methods

        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            //ignore this.  If we wrap this stream with a
            //BufferedStream this should prevent the underlying
            //NetworkStream from throwing an exception when Flush()
            //is called
            return 0;
        }

        #endregion Methods
    }
}