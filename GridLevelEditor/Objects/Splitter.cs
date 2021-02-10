namespace GridLevelEditor.Objects
{
    public static partial class FileIO
    {
        static class Splitter
        {
            public static string Global { get => "<\b|\b>"; }
            public static string Level { get => "<\blvl\b>"; }
            public static string Column { get => "<\blvldatacol\b>"; }
            public static string Row { get => "<\blvldatarow\b>"; }
            public static string Void { get => "<\bvoid\b/>"; }
        }
    }
}
