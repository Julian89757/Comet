using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comet.Core
{
    // 自定义异常
    public class CometException : Exception
    {
        // Comet客户端已经存在
        public const int CometClientAlreadyExists = 1;
        // Comet客户端不存在
        public const int CometClientDoesNotExist = 2;
        // Comet处理参数无效
        public const int CometHandlerParametersAreInvalid = 3;
    
        public int MessageId { get; }

        public CometException(int messageId, string message, params object [] args) 
            : base(string.Format(message, args))
        {
            MessageId = messageId;
        }

        /// <summary>
        /// Returns an exception initialized with the CometClientAlreadyExists exception
        /// </summary>
        /// <returns></returns>
        public static CometException CometClientAlreadyExistsException()
        {
            return new CometException(CometClientAlreadyExists, "CometClient already exists. Either the Private or Public Token is in use."); 
        }

        /// <summary>
        /// Returns an exception initialized with the CometClientDoesNotExist Exception
        /// </summary>
        /// <returns></returns>
        public static CometException CometClientDoesNotExistException()
        {
            return new CometException(CometClientDoesNotExist, "CometClient does not exist."); 
        }

        /// <summary>
        /// Returns an exception initialized with the CometHandlerParametersAreInvalid Exception
        /// </summary>
        /// <returns></returns>
        internal static CometException CometHandlerParametersAreInvalidException()
        {
            return new CometException(CometHandlerParametersAreInvalid, "Parameters passed to the BeginSubscribe method are invalid.  Please specify lastMessageId (long) and prviateToken (string) in the POST parameters.");
        }
    }
}
