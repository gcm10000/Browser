        private static void Client()
        {
            Uri uri = new Uri("http://slider.kz/vk_auth.php?q=2pac");
            ClientWeb client = new ClientWeb();
            client.Request(uri);
            string Header = client.GetResponse().Header;
            string Body = client.GetResponse().Body;
            Console.WriteLine(Body);
            System.IO.File.WriteAllText(@"D:\DecnoBot\DecnoCSharp\DecnoServer\Header.txt", Header);
            System.IO.File.WriteAllText(@"D:\DecnoBot\DecnoCSharp\DecnoServer\Body.txt", Body);

        }