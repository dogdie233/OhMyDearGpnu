namespace OhMyDearGpnu
{
    internal static class Helper
    {
        internal static string ReadPassword()
        {
            var index = 0;
            var passwordCharArray = new char[8];
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Backspace)
                {
                    index--;
                    if (index >= 0)
                        Console.Write("\b \b");
                    else
                        index = 0;
                    continue;
                }
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return new string(passwordCharArray, 0, index);
                }
                if (char.IsControl(key.KeyChar))
                    continue;
                if (index >= passwordCharArray.Length)
                {
                    var newArray = new char[passwordCharArray.Length * 2];
                    Array.Copy(passwordCharArray, 0, newArray, 0, passwordCharArray.Length);
                    passwordCharArray = newArray;
                }
                passwordCharArray[index++] = key.KeyChar;
                Console.Write('*');
            }
        }
    }
}
