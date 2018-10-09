using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public static partial class HttpExtension
    {
        /// <summary>
        /// Initializes the <see cref="HttpExtension"/> class.
        /// </summary>
        static HttpExtension()
        {
            // ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        #region Uri and Credential

        /// <summary>
        /// The domain credential regex
        /// </summary>
        private static Regex domainCredentialRegex = new Regex(@"^((?<Domain>([0-9a-zA-Z_-]+))\\)?(?<AccountName>([0-9a-zA-Z\._-]+))(:(?<Token>(.+)))?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// The URI credential regex
        /// </summary>
        private static Regex uriCredentialRegex = new Regex(@"^((?<AccountName>([0-9a-zA-Z\._-]+))(:(?<Token>(.+)))?@(?<Domain>([0-9a-zA-Z_\.-]+)))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Gets the primary URI. Get primary part like: http://google.com/, ignore the rest.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>System.String.</returns>
        public static string GetPrimaryUri(this Uri uri)
        {
            if (uri == null)
            {
                return null;
            }
            return uri.IsDefaultPort ? string.Format("{0}://{1}/", uri.Scheme, uri.Host) : string.Format("{0}://{1}:{2}/", uri.Scheme, uri.Host, uri.Port);
        }

        /// <summary>
        /// Parses to access credential. Acceptable string samples: cnsh\rynn.wang:12345, cnsh\rynn.wang, rynn.wang:12345, rynn.wang, rynn.wang@cnsh, rynn.wang:12345@cnsh
        /// </summary>
        /// <param name="accountString">The account string.</param>
        /// <returns>HttpCredential.</returns>
        public static HttpCredential ParseToAccessCredential(this string accountString)
        {
            return string.IsNullOrWhiteSpace(accountString) ?
                    null
                    : (ParseDomainCredentialToAccessCredential(accountString) ?? ParseUrlCredentialToAccessCredential(accountString));
        }

        /// <summary>
        /// Parses the URL credential to access credential. Supported string samples: rynn.wang:12345, rynn.wang, rynn.wang@cnsh, rynn.wang:12345@cnsh
        /// </summary>
        /// <param name="urlCredential">The URL credential.</param>
        /// <returns></returns>
        public static HttpCredential ParseUrlCredentialToAccessCredential(this string urlCredential)
        {
            if (!string.IsNullOrWhiteSpace(urlCredential))
            {
                var match = uriCredentialRegex.Match(urlCredential);
                if (match.Success)
                {
                    return new HttpCredential
                    {
                        Token = match.Result("${Token}").ToUrlDecodedText(),
                        Account = match.Result("${AccountName}"),
                        Domain = match.Result("${Domain}")
                    };
                }
            }

            return null;
        }

        /// <summary>
        /// Parses the domain credential to access credential. Supported string samples: cnsh\rynn.wang:12345, cnsh\rynn.wang, rynn.wang:12345, rynn.wang
        /// </summary>
        /// <param name="urlCredential">The URL credential.</param>
        /// <returns></returns>
        public static HttpCredential ParseDomainCredentialToAccessCredential(this string urlCredential)
        {
            if (!string.IsNullOrWhiteSpace(urlCredential))
            {
                var match = domainCredentialRegex.Match(urlCredential);
                if (match.Success)
                {
                    return new HttpCredential
                    {
                        Token = match.Result("${Token}"),
                        Account = match.Result("${AccountName}"),
                        Domain = match.Result("${Domain}")
                    };
                }
            }

            return null;
        }

        /// <summary>
        /// To the credential string. Format: {domain}\{user}:{password}
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns></returns>
        public static string ToCredentialString(this HttpCredential credential)
        {
            return credential == null ?
                null :
                string.IsNullOrWhiteSpace(credential.Domain) ?
                    ToUserPassword(credential) :
                    string.Format("{0}\\{1}", credential.Domain, ToUserPassword(credential));
        }

        /// <summary>
        /// To the URL. Format: {protocol}{user}:{password}@{domain}
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <param name="protocol">The protocol.</param>
        /// <returns></returns>
        public static string ToUrl(this HttpCredential credential, string protocol = null)
        {
            credential.Domain.CheckEmptyString(nameof(credential.Domain));
            var userPassword = ToUserPassword(credential, EncodingOrSecurityExtension.ToUrlEncodedText);
            return credential == null ?
                null :
                string.IsNullOrWhiteSpace(userPassword) ?
                    string.Format("{0}{1}", protocol.SafeToString(HttpConstants.HttpProtocols.HttpProtocol), credential.Domain) :
                    string.Format("{0}{1}@{2}", protocol.SafeToString(HttpConstants.HttpProtocols.HttpProtocol), userPassword, credential.Domain);
        }

        /// <summary>
        /// To the user password. Format: {user}:{password}
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <param name="tokenPrepareAction">The token prepare action.</param>
        /// <returns></returns>
        private static string ToUserPassword(this HttpCredential credential, Func<string, string> tokenPrepareAction = null)
        {
            string result = null;
            if (credential != null)
            {
                result = string.Format(HttpConstants.UserPasswordFormat, credential.Account, tokenPrepareAction == null ? credential.Token : tokenPrepareAction(credential.Token));
                if (result[0] == ':')
                {
                    result = result.Substring(1);
                }
            }
            return result;
        }

        #endregion Uri and Credential

        /// <summary>
        /// Determines whether this instance is ok.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>
        ///   <c>true</c> if the specified status code is ok; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOK(this HttpStatusCode statusCode)
        {
            var start = (((int)statusCode) / 100);
            return !(start == 4 || start == 5);
        }
    }
}