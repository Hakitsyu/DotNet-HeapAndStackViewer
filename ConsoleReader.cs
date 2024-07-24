namespace DotNet_HeapAndStackViewer
{
    internal static class ConsoleReader
    {
        internal static string? GetText(string text)
        {
            Console.Write(text);
            return Console.ReadLine();
        }

        internal static string GetRequiredText(string text, string? retryMessage = null)
        {
            Console.Write(text);
            return ForceReadLine(retryMessage);
        }

        internal static string GetOption(string text, 
            IEnumerable<string> options,
            string? retryMessage = null)
        {
            if (!text.EndsWith(' '))
                text = text + ' ';

            Console.Write(text);

            while (true)
            {
                var option = ForceReadLine(retryMessage).Trim();
                if (options.Contains(option))
                    return option;

                if (!string.IsNullOrWhiteSpace(retryMessage))
                    Console.WriteLine(retryMessage);
            }
        }

        internal static string ForceReadLine(string? retryMessage = null)
        {
            string? content = null;

            do
            {
                content = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(retryMessage) && string.IsNullOrWhiteSpace(content))
                {
                    Console.WriteLine(retryMessage);
                }

            } while (string.IsNullOrWhiteSpace(content));

            return content;
        }
    }
}
