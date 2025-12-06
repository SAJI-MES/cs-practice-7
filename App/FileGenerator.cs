using System.Text;
using Bogus;

namespace App;

/// <summary>
/// Класс который я использовал для генерации файлов, вам его модифицировать не нужно.
/// </summary>
public static class FileGenerator
{
    public static Task GenerateDefault(CancellationToken ct = default) => Task.WhenAll(
        Generate("lorem.txt", 25565, "ru", ct),
        Generate("ipsum.txt", 5432, "en", ct),
        Generate("dolor.txt", 8123, "fr", ct),
        Generate("sit.txt", 27017, "de", ct),
        Generate("amet.txt", 6379, "it", ct));
    
    public static async Task Generate(string filename, int length, string locale, CancellationToken ct = default)
    {
        var faker = new Faker(locale);
        var phrases = Enumerable.Range(0, length).Select(_ => faker.Lorem.Sentence());
        await File.WriteAllLinesAsync(filename, phrases, Encoding.UTF8, ct);
    }
}