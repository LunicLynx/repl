# Eagle

The eagle programming language tries to combine features from different languages into one language.
It syntax and style is heavily influence by C# while borrowing ideas from rust and its ecosystem.

The biggest differences to C# are, that eagle is compiled ahead of time and does not have a garbage collector.

Some aspects are comparable to C# while following a different philosophy.
For example prefix operations are changed to suffix ones to reduce the need of using parenthesis.

While C# and C++ have concepts of classes and structs, eagle only has objects for object oriented constructs.

# build system


-- mission statement:



From the command line change into a directory where a project is located. 
Run a cli like <command> "run" to execute the current project.
Run a cli line <command> "build" to build the current project.

A possible directory structure could be:

root
 - src 
   - here goes the source
 - bin
   - here goes the build output
 - docs
   - here goes documentation
- .eoptions
    file that contains options for the build etc.

-- source

root (src)
  Main.e - Entry point of the application (contains the Main function)



# nest cli

init -> create base structur
  - .gitignore etc.

run -> build the project and run it

build -> build the project


# things that have to be done before this are


## variables

// readonly variables
let a = 5;
a = 6; // error

// mutable variables

var a = 5;
a = 6; // ok

// redefinition
let a = 5;
var a = 6; // ok

// references

var x = 5;
var &y = x; 
y = 6; 
Assert(x, 6);

// parameters

Print(&str: string)
{

}

let s = string("Hello");
Print(s);


// copy
works for all the simple type



// 1. alloc and destroy
// 2. enums
// 3. interfaces and impl
// 4. iterators
// 5. generics
// ?. error handling without exceptions
