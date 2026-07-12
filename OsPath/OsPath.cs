using System;
using System.IO;

namespace OsPath
{
#if NET10_0_OR_GREATER
    public record struct OsPath
#else
    public struct OsPath
#endif
    {
#if NET10_0_OR_GREATER
        public readonly bool IsEmpty => string.IsNullOrEmpty(_path);
        public readonly bool IsAbsolute => Path.IsPathRooted(_path);
        public readonly string? Extension => Path.GetExtension(_path);
        public readonly string FileName => Path.GetFileName(_path);
#else        
        public bool IsEmpty => string.IsNullOrEmpty(_path);
        public bool IsAbsolute => Path.IsPathRooted(_path);
        public string Extension => Path.GetExtension(_path);
        public string FileName => Path.GetFileName(_path);
#endif

        private readonly string _path;

#if NET10_0_OR_GREATER
        public OsPath()
        {
            this._path = string.Empty;
        }
#endif

        public OsPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));
#if NET10_0_OR_GREATER
            this._path = Path.TrimEndingDirectorySeparator(path);
#else
            this._path = path.TrimEnd(Path.DirectorySeparatorChar);
#endif
        }

        public OsPath(OsPathSpec pathSpec)
        {
#if NET10_0_OR_GREATER
            this._path = pathSpec switch
            {
                OsPathSpec.Current => ".",
                OsPathSpec.Parent => "..",
                _ => throw new ArgumentException()
            };
#else
            if (pathSpec == OsPathSpec.Current) this._path = ".";
            else if (pathSpec == OsPathSpec.Parent) this._path = "..";
            else throw new ArgumentException();
#endif
        }

        public OsPath Combine(OsPath other)
        {
            return new OsPath(Path.Combine(this._path, other._path));
        }

        public OsPath Combine(string other)
        {
            if (string.IsNullOrWhiteSpace(other)) throw new ArgumentNullException(nameof(other));
            return new OsPath(Path.Combine(this._path, other));
        }

        public OsPath Absolute()
        {
            if (string.IsNullOrEmpty(this._path)) return this;
            return new OsPath(Path.GetFullPath(this._path));
        }

        public OsPath Parent()
        {
            if (string.IsNullOrEmpty(this._path)) return this;
            string parent = Path.GetDirectoryName(this._path);
            return parent is null
                ? default
                : new OsPath(parent);
        }

        public string[] Split()
        {
            if (string.IsNullOrEmpty(this._path)) return Array.Empty<string>();
            return this._path.Split(Path.DirectorySeparatorChar);
        }

        public static OsPath operator /(OsPath left, OsPath right)
        {
            return left.Combine(right);
        }

        public static OsPath operator /(OsPath left, string right)
        {
            return left.Combine(right);
        }

        public static OsPath operator /(string left, OsPath right)
        {
            return new OsPath(Path.Combine(left, right._path));
        }

        public override string ToString()
        {
            return _path;
        }
    }

    public enum OsPathSpec
    {
        Current,
        Parent
    }
}
