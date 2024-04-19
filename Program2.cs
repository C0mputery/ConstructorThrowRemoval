using System.Text.RegularExpressions;

namespace ConstructorRemoval
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var CSharpFiles = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.cs", SearchOption.AllDirectories);
            try
            {
                string patternGlobal = $".* class .* : VolumeParameter<.*>"; // Hopefully this is enough to find the class
                Parallel.ForEach(CSharpFiles, (file) =>
                {
                    string typeName = Path.GetFileNameWithoutExtension(file);
                    string pattern = $"class {typeName} : VolumeComponent, IPostProcessComponent";

                    // Find the constructor and then remove throw null from it

                    // Read file line by line
                    string[] lines = File.ReadAllLines(file);
                    bool isSomethingToFix = false;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains(pattern))
                        {
                            isSomethingToFix = true;
                            lines[i] = lines[i].Replace(": VolumeComponent, IPostProcessComponent", "//: VolumeComponent, IPostProcessComponent");
                        }

                        if (isSomethingToFix && (lines[i].Contains("public bool IsActive()") || lines[i].Contains("public bool IsTileCompatible()") || lines[i].Contains("private void Reset()")))
                        {

                            lines[i - 1] = lines[i - 1].Insert(0, "//");
                            lines[i] = lines[i].Insert(0, "//");
                            lines[i + 1] = lines[i + 1].Insert(0, "//");
                            lines[i + 2] = lines[i + 2].Insert(0, "//");
                            lines[i + 3] = lines[i + 3].Insert(0, "//");
                            Console.WriteLine($"Fixed {file}");
                        }

                        if (Regex.IsMatch(lines[i], patternGlobal))
                        {
                            lines[i + 2] = lines[i + 2].Insert(0, "//");
                            lines[i + 3] = lines[i + 3].Insert(0, "//");
                            lines[i + 4] = lines[i + 4].Insert(0, "//");
                            lines[i + 5] = lines[i + 5].Insert(0, "//");
                            lines[i + 6] = lines[i + 6].Insert(0, "//");
                            Console.WriteLine($"Fixed {file} due too global");
                        }
                    }
                    File.WriteAllLines(file, lines);

                });

                // VolumeParameter pass through
                string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.cs", SearchOption.AllDirectories);
                Parallel.ForEach(files, (file) =>
                {

                    // Read file line by line
                    string[] lines = File.ReadAllLines(file);
                    for (int i = 0; i < lines.Length; i++)
                    {
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Done press any key to exit");
            Console.ReadKey();
        }
    }
}