using System.Text.RegularExpressions;

namespace ConstructorRemoval
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var CSharpFiles = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.cs", SearchOption.AllDirectories);
            Parallel.ForEach(CSharpFiles, (file) =>
            {
                string typeName = Path.GetFileNameWithoutExtension(file);
                string pattern = @"public\s+" + typeName + @"\s*\(";
                string findStuct = @"struct\s+" + typeName;

                // Find the constructor and then remove throw null from it

                // Read file line by line
                string[] lines = File.ReadAllLines(file);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (Regex.IsMatch(lines[i], findStuct) && lines[i + 1].Contains('{'))
                    {
                        break;
                    }

                    if (Regex.IsMatch(lines[i], pattern))
                    {
                        if (lines[i + 2].Contains("throw null;"))
                        {
                            lines[i + 2] = string.Empty;
                            Console.WriteLine($"Removed constructor null throw from {file}");
                        }
                    }
                }
                File.WriteAllLines(file, lines);
            });
        }
    }
}
