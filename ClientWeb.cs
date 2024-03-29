﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DecnoBrowser
{
    public static class Helper
    {
        public static IEnumerable<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    break;
                yield return index;
            }
        }

    }

    public abstract class Header
    {
        public string UserAgent { get; set; } = @"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.142 Safari/537.36";
        public string Method { get; set; } = "GET";
        protected Uri Uri;
        public Dictionary<string, string> AddHeader = new Dictionary<string, string>();

        public Header()
        {

        }

        public override string ToString()
        {
            string strRequest = $"{Method} {Uri.PathAndQuery} HTTP/1.1" + Environment.NewLine;
            strRequest += $"Host: {Uri.Authority}" + Environment.NewLine;
            strRequest += "Connection: keep-alive" + Environment.NewLine;
            strRequest += "Upgrade-Insecure-Requests: 1" + Environment.NewLine;
            strRequest += "User-Agent: " + UserAgent + Environment.NewLine;
            strRequest += @"Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3" + Environment.NewLine;
            //strRequest += @"Accept-Encoding: gzip, deflate, br" + Environment.NewLine;
            strRequest += @"Accept-Language: pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7" + Environment.NewLine;
            strRequest += @"Cookie: SESSIONID=00000000" + Environment.NewLine;
            if (AddHeader.Count > 0)
                foreach (var item in AddHeader)
                {
                    strRequest += $"{item.Key}: {item.Value}" + Environment.NewLine;
                }
            strRequest += Environment.NewLine;

            return strRequest;
        }
    }
    public class ClientWeb : Header
    {
        AsyncClient client;
        int TimeOut { get => client.TimeOut; set => client.TimeOut = value; }

        public class Document
        {
            public string Header { get; set; }
            public string Body { get; set; }
            public Document(string Header, string Body)
            {
                this.Header = Header;
                this.Body = Body;
            }
        }

        public ClientWeb() : base()
        {
            client = new AsyncClient();
        }

        public void Request(Uri Uri)
        {
            base.Uri = Uri;
            client.Connect(Uri.Host, Uri.Port);
            client.SendHeader(this);
        }

        public Document GetResponse()
        {
            string result = client.Receive();
            int separateHeader = result.IndexOf("\r\n\r\n");
            int endBody = result.LastIndexOf("\r\n0\r\n\r\n");

            string Header = result.Substring(0, separateHeader);

            string Body = result.Substring(separateHeader + 4);
            int NumberUnknown = Body.IndexOf("\r\n\r\n");

            if (endBody > -1)
                Body = result.Substring(separateHeader + 4, endBody - (separateHeader + 4));

            if (Int32.TryParse(Body.Substring(0, NumberUnknown), System.Globalization.NumberStyles.HexNumber, null, out _))
                Body = Body.Substring(NumberUnknown + 2);

            Document document = new Document(Header, Body);
            return document;
        }

        public void Close()
        {
            client.Close();
        }

    }

}
