extern Print(s: string);
extern Input(): string;

extern malloc(size: int): char*;
extern free(ptr: char*);

object String {

    _buffer: char*;
    _length: int;

    // private
    ctor(buffer: char*) {
        _buffer = buffer;
    }

    dtor() {
        free(_buffer);
    }

    [index: int] : char {
        get {
            return _buffer[index];
            // return (*this).(char*)[index];
        }
    }

    // Get_Length(string* s)
    Length :int {
        get {
            return _length;
            // var l = 0;
            // let self = (*this).(char*);
            // while self[l] != '\0'
            //     l = l + 1;
            // return l;
        }
    }

    // static -> no this pointer
    static Concat(a: string&, b: string&) : string {

        var length = a.Length + b.Length

        var result  = malloc(length + 1);

        // move a's content into the result
        for i = 0 to a.Length {
            result[i] = a[i];
        }

        // move b's content into the result
        for i = 0 to b.Length {
            result[a.Length + i] = b[i];
        }

        result[length] = '\0';

        return result.(any).(string);
    }

    static FromCharPtr(str: char*) {
        
    }
}

