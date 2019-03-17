using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WebCrawler
{
    public static class LinkService
    {
        public static bool IsFullPath(string url)
        {
            var pattern = @"^(https?:\/\/)";
            var match = Regex.Match(url, pattern);

            return match.Success;
        }

        public static bool IsValidLink(string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                return false;
            }
            return true;
        }

        public static bool IsValidLinkFromUser(string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                return false;
            }

            string pattern = @"^https?:\/\/(www\.)?[a-zA-Z0-9\-]+\.[a-zA-Z0-9\-]+";
            var match = Regex.Match(url, pattern);
            if (match.Success)
            {
                return true;
            }
            return false;
        }

        public static bool IsInternalLink(string url, string parentUrl)
        {
            if (String.Equals(GetDomain(url), GetDomain(parentUrl)))
            {
                return true;
            }
            return false;
        }

        public static string GetEalierPath(string parentUrl)
        {
            var sb = new StringBuilder();

            //1 eg. http://site.example.com/page1/
            if (String.Equals(parentUrl[parentUrl.Length - 1], '/'))
            {
                return parentUrl;
            }

            //2 eg. http://site.example.com/page1.html
            var match3 = Regex.Match(parentUrl, @"([\s\S]+[a-z0-9\-]+\.[a-z0-9\-]+\/)([a-z0-9\-]+\.[a-z0-9\-]+)$", RegexOptions.IgnoreCase);
            if (match3.Success)
            {
                return match3.Groups[1].ToString();
            }

            //3 eg. http://site.example.com/page1
            var match2 = Regex.Match(parentUrl, @"(\/[a-z0-9\-]+)$", RegexOptions.IgnoreCase);
            if (match2.Success)
            {
                sb.Append(parentUrl);
                sb.Append("/");
                return sb.ToString();
            }

            //4 eg. http://site.example.com
            var match1 = Regex.Match(parentUrl, @"(([a-z0-9\-]+\.)*[a-z0-9\-]+\.[a-z]+)\b", RegexOptions.IgnoreCase);
            if (match1.Success)
            {
                if (String.Equals(match1.Groups[0].ToString(), GetDomainOrSubdomain(parentUrl)))
                {
                    sb.Append(parentUrl);
                    sb.Append("/");
                    return sb.ToString();
                }
            }

            return "";
        }

        public static string PrepareUrl(string url)
        {
            if(String.Equals(url[0], '/'))
            {
                var match = Regex.Match(url, @"^\/([\s\S]+)", RegexOptions.IgnoreCase);
                return match.Groups[1].ToString();
            }
            return url;
        }

        private static bool IsSubdomain(string url)
        {
            var domainOrSubdomain = GetDomainOrSubdomain(url);
            var match = Regex.Match(domainOrSubdomain, @"[a-z0-9\-]+\.[a-z0-9\-]+\.", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return true;
            }
            return false;
        }

        public static string GetDomain(string url)
        {
            var match1 = Regex.Match(url, @"(www\.)?([a-z0-9\-]+\.)*([a-z0-9\-]+\.(edu|gov)\.[a-z]+)", RegexOptions.IgnoreCase);
            if (match1.Success)
            {
                return match1.Groups[3].ToString();
            }

            var match2 = Regex.Match(url, @"([a-z0-9\-]+\.)*([a-z0-9\-]+\.[a-z]+)", RegexOptions.IgnoreCase);
            if (match2.Success)
            {
                return match2.Groups[2].ToString();
            }
            return "unknown";
        }

        public static string GetDomainOrSubdomain(string url)
        {
            var match = Regex.Match(url, @"(www\.)?(([a-z0-9\-]+\.)*[a-z0-9\-]+\.[a-z]+)", RegexOptions.IgnoreCase);
            return match.Groups[2].ToString();
        }
    }
}
