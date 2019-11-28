using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWT;
using JWT.Builder;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Gov.Pssg.Css.Public.Utility
{
    public static class JwtUtility
    {
        public static JObject TryDecode(string token, string secret, ILogger logger)
        {
            try
            {
                string payloadString = Decode(token, secret);
                var payload = JObject.Parse(payloadString);

                return payload;
            }
            catch (TokenExpiredException ex)
            {
                logger.LogWarning(ex, "Token expired at {Expiry}", ex.Expiration);
                throw;
            }
            catch (SignatureVerificationException ex)
            {
                logger.LogWarning(ex, "Failed to verify token using provided secret");
                throw;
            }
            catch (JsonReaderException ex)
            {
                logger.LogWarning(ex, "Failed to parse token payload");
                throw;
            }
        }

        private static string Decode(string token, string secret)
        {
            return new JwtBuilder()
                .WithSecret(secret)
                .MustVerifySignature()
                .Decode(token);
        }
    }
}
