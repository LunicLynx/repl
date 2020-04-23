
// things that have to be done before this are
// 1. alloc and destroy
// 2. enums
// 3. interfaces and impl
// 4. iterators
// 5. generics
// ?. error handling without exceptions

extern GetCurrenctDirectory(): Path;
extern RunProcess(command: string);

extern ExistsDirectory(path: string);
extern CreateDirectory(path: string);

// immutable path object
// a path can actualy be a directory, a file or nothing
// we don't really want to deal with paths as strings
// that is just tedious
// a path can be absolute / or relative
// but when it is relative we don't know if it is a directory or file
object Path {
    _path: string;

    Name: string {
        get {
            var parts = _path.Split('/');
            return parts[parts.Length - 1];
        }
    }

    ctor(path: string) {
        _path = path;
    }

    Append(s: string) : Path {
        return Path(_path + "/" + s);
    }

    EnsureDirectory(s: name): Path {
        // this must exist either as absolute path
        // or combined with relative path
        // otherwise this should not work
        
        let p = Append(name);

        if !p.Exists {
            p.CreateDirectory();
        }

        // TODO if p.Exists == true but is a file -> error

        return p;
    }

    GetFiles(searchStyle: SearchStyle, string): List<Path> {

    }
}

object List<T>
{

}

impl Iterator<T> for List<T> {
    // TODO
}

enum SearchStyle
{
    None,
    Recursive
}

Main() {
    let currentDirectory = GetCurrentDirectory();

    let appName = currentDirectory.Name;

    let srcDirectory = currentDirectory.Append("src");
    let binDirectory = currentDirectory.Append("bin");

    let files = sourceDirectory.GetFiles(Recursive, "*.e");

    var filesStr = "";
    for file in files {
        filesStr = filesStr + file.Path + " ";
    }

    let outputDir = currentDirectory.EnsureDirectory("bin");

    let outputFile = outputDir.Append(appName + ".exe");

    RunProcess("eagle "+ filesStr + " -o " + outputFile);
}