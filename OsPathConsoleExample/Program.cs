namespace OsPathConsoleExample;
using OsPath;

internal class Program
{
    static void Main()
    {
        OsPath root = new(OsPathSpec.Parent);
        Console.WriteLine(root / "test");
    }
}
