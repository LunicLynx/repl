extern StringLength(str: string): int

struct String {
    Length:int => StringLength(this)
}

struct Int32 {
}

struct Int64 {
}

struct UInt32 {
}

struct UInt64 {
}

struct Int16 {
}

struct UInt16 {
}

struct Int8 {
}

struct UInt8 {
}

struct Void {
}

struct Boolean {
}

extern Print(s: string);
extern Read(): string;

var min = 0;
            var max = 100;
            Print($"Think of a number between {min} and {max}.")

            while (min != max)
            {
                let guess = (min + max) / 2

                Print("Is your number greater than " + guess + "?")

                var greater = false
                while (true)
                {
                    Print("Enter y, n:")
                    var a = Read()
                    if (a == "y")
                    {
                        greater = true;
                        break
                    }
                    else if (a == "n")
                    {
                        break
                    }
                }

                if (greater)
                {
                    min = guess + 1
                }
                else
                {
                    max = guess
                }
            }

            Print("Your number is "+ min)