# Eagle

The eagle programming language tries to combine features from different languages into one language.
It syntax and style is heavily influence by C# while borrowing ideas from rust and its ecosystem.

The biggest differences to C# are, that eagle is compiled ahead of time and does not have a garbage collector.


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

