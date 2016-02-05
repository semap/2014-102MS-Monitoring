using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Ping
{
    public class RawSocketPing
    {
        public Socket _pingSocket;             // Raw socket handle
        public int _timeToLive;                // Time-to-live value to set on ping
        public ushort _pingId;                 // ID value to set in ping packet
        public ushort _pingSequence;           // Current sending sequence number
        public int _payloadSize;      // Size of the payload in ping packet
        public int _pingReceiveTimeout;     // Timeout value to wait for ping response
        public IPEndPoint _responseEndPoint;       // Contains the source address of the ping response
        public EndPoint _castResponseEndPoint;   // Simple cast time used for the responseEndPoint
        private byte[] _pingPacket;             // Byte array of ping packet built
        private byte[] _pingPayload;            // Payload in the ping packet
        private byte[] _receiveBuffer;          // Buffer used to receive ping response
        private IcmpHeader _icmpHeader;             // ICMP header built (for IPv4)
        private DateTime _pingSentTime;       // Timestamp of when ping request was sent

        /// <summary>
        /// Base constructor that initializes the member variables to default values. It also
        /// creates the events used and initializes the async callback function.
        /// </summary>
        public RawSocketPing()
        {
            _pingSocket = null;
            _timeToLive = 8;
            _payloadSize = 8;
            _pingSequence = 0;
            _pingReceiveTimeout = 2000;
            _icmpHeader = null;
        }

        /// <summary>
        /// Constructor that overrides several members of the ping packet such as TTL,
        /// payload length, ping ID, etc.
        /// </summary>
        /// <param name="timeToLive">Time-to-live value to set on ping packet</param>
        /// <param name="payloadSize">Number of bytes in ping payload</param>
        /// <param name="pingId">ID value to put into ping header</param>
        public RawSocketPing(int timeToLive, int payloadSize, ushort pingId)
            : this()
        {
            _timeToLive = timeToLive;
            _payloadSize = payloadSize;
            _pingId = pingId;
        }

        /// <summary>
        /// This routine is called when the calling application is done with the ping class.
        /// This routine closes any open resource such as socket handles.
        /// </summary>
        public void Close()
        {
            try
            {
                if (_pingSocket != null)
                {
                    _pingSocket.Close();
                    _pingSocket = null;
                }
            }
            catch (Exception err)
            {
                //throw;
            }
        }

        /// <summary>
        /// This routine initializes the raw socket, sets the TTL, allocates the receive
        /// buffer, and sets up the endpoint used to receive any ICMP echo responses.
        /// </summary>
        public void InitializeSocket()
        {
            // Create the raw socket
            _pingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);

            // Socket must be bound locally before socket options can be applied
            var localEndPoint = new IPEndPoint(IPAddress.Any, 0);
            _pingSocket.Bind(localEndPoint);

            _pingSocket.SetSocketOption(
                SocketOptionLevel.IP,
                SocketOptionName.IpTimeToLive,
                _timeToLive
                );

            // Allocate the buffer used to receive the response
            _pingSocket.ReceiveTimeout = _pingReceiveTimeout;
            _receiveBuffer = new byte[540];

            _responseEndPoint = new IPEndPoint(IPAddress.Any, 0);
            _castResponseEndPoint = (EndPoint)_responseEndPoint;
        }

        /// <summary>
        /// This routine builds the appropriate ICMP echo packet depending on the
        /// protocol family requested.
        /// </summary>
        public void BuildPingPacket()
        {
            // Initialize the socket if it hasn't already been done
            if (_pingSocket == null)
            {
                InitializeSocket();
            }

            // Create the ICMP header and initialize the members
            _icmpHeader = new IcmpHeader() 
            { 
                Id = _pingId, 
                Sequence = _pingSequence, 
                Type = IcmpHeader.EchoRequestType, 
                Code = IcmpHeader.EchoRequestCode 
            };

            // Build the data payload of the ICMP echo request
            _pingPayload = new byte[_payloadSize];
            for (int i = 0; i < _pingPayload.Length; i++)
            {
                _pingPayload[i] = (byte)'e';
            }
        }

        /// <summary>
        /// This function performs the actual ping. It sends the ping packets created to
        /// the destination 
        /// </summary>
        public bool DoPing(IPAddress destination)
        {
            bool success = false;

            // Send an echo request
            try
            {
                // Increment the sequence count in the ICMP header
                _icmpHeader.Sequence = (ushort)(_icmpHeader.Sequence + (ushort)1);

                // Build the byte array representing the ping packet. This needs to be done
                //    before ever send because we change the sequence number (which will affect
                //    the calculated checksum).
                _pingPacket = _icmpHeader.BuildPacket(_pingId, _pingPayload);

                // Mark the time we sent the packet
                _pingSentTime = DateTime.Now;

                // Send the echo request
                // ICMP does not care about the port, so zero is fine here
                var destinationEndpoint = new IPEndPoint(destination, 0);
                _pingSocket.SendTo(_pingPacket, destinationEndpoint);

                int Brecieved = _pingSocket.ReceiveFrom(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, ref _castResponseEndPoint);

                //if you get to here, then you got a response.
                success = true;
            }
            catch (SocketException err)
            {
                // ping failed
            }

            return success;
        }
    }
}
