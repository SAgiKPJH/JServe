using System.Text.RegularExpressions;

namespace CodeTest
{
    public class EmptyStringLiteralTest
    {
        private readonly string _sourceDirectory = Path.GetFullPath(
            Path.Combine(Directory.GetCurrentDirectory(),"..", "..", "..", "..", "..")
        );

        [Fact]
        public void ShouldNotContainEmptyStringLiterals()
        {
            var test = Directory.GetFiles(_sourceDirectory, "*.cs", SearchOption.AllDirectories);
            var csFiles = Directory.GetFiles(_sourceDirectory, "*.cs", SearchOption.AllDirectories)
                .Where(path => !path.Contains("\\obj\\") && !path.Contains("\\bin\\") && !path.Contains("EmptyStringLiteralTest.cs")).ToList();

            Assert.NotEmpty(csFiles);

            var violations = new List<string>();
            var emptyStringPattern = new Regex(@"(?<!\/\/.*)""(?<!\\)""(?!\s*\/\/)");

            foreach (var file in csFiles)
            {
                var lines = File.ReadAllLines(file);
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];

                    if (emptyStringPattern.IsMatch(line))
                    {
                        var relativePath = Path.GetRelativePath(_sourceDirectory, file);
                        violations.Add($"{relativePath}:{i + 1} - {line.Trim()}");
                    }
                }
            }

            Assert.True(violations.Count == 0, $"빈 문자열 리터럴(\"\")이 발견되었습니다:\n{string.Join("\n", violations)}");
        }
    }
}
