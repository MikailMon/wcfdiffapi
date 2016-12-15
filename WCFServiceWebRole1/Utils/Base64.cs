using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WCFServiceWebRole1.Utils
{
    /// <summary>
    /// Utility class for general purposes
    /// </summary>
    public static class Base64
    {
        /// <summary>
        /// Decode a base64 string
        /// </summary>
        /// <param name="encodedText">Encoded base64 string</param>
        /// <returns>Decoded string</returns>
        public static string Decode(string encodedText) {            
            byte[] decodedBytes = Convert.FromBase64String(encodedText);
            string decodedText = Encoding.UTF8.GetString(decodedBytes);
            return decodedText;
        }
    }
}