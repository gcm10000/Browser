        private static void Client()
        {
            Uri uri = new Uri("http://slider.kz/vk_auth.php?q=eminem");
            ClientWeb client = new ClientWeb();
            client.Request(uri);
            var response = client.GetResponse();
            string Header = response.Header;
            string Body = response.Body;
            //Console.WriteLine(Body);
            System.IO.File.WriteAllText(@"D:\DecnoBot\DecnoCSharp\DecnoServer\Header.txt", Header);
            System.IO.File.WriteAllText(@"D:\DecnoBot\DecnoCSharp\DecnoServer\Body.txt", Body);

        }