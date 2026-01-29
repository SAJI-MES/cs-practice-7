namespace App;

/// <summary>
/// Считывает и валидирует ввод пользователя.
/// </summary>
public static class Input
{
    /// <summary>
    /// Считывает от пользователя URL файлов из интернета.
    /// </summary>
    public static string[] GetUris()
    {
        string[] result = [];
        var valid = false;
        while (valid is false)
        {
            Console.Write("Введите нужные URL через пробел: ");
            result = Console.ReadLine()!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            valid = result.All(x => IsValidUri(x));
            if (valid is false)
            {
                Console.WriteLine("Вы ввели некорректный URL");
            }
        }
        return result;
    }
    private static bool IsValidUri(string uri)
    {
        return Uri.TryCreate(uri, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    /// <summary>
    /// Считывает от пользователя путь до файла с результатом.
    /// </summary>
    public static FileInfo GetOutputFile()
    {
        while (true)
        {
            Console.Write("Введите путь до файла с результатом");
            try
            {
                var file = new FileInfo(Console.ReadLine()!);
                if (file.Exists)
                {
                    Console.Write("Файл уже существует. Хотите его перезаписать? да или нет?");
                    var answer = Console.ReadLine()!.ToLower();
                    if (answer != "да")
                    {
                        continue;
                    }
                }
                return file;
            }
            catch
            {
                Console.WriteLine("Ошибка. Вы ввели некорректный путь. Попробуйте ещё раз");
            }
        }
    }
}