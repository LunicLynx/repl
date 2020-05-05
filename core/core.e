extern Print(s: string&);
extern Input(): string;

extern malloc(size: int): i8*;
extern free(ptr: i8*);

object String {

    _buffer: char*;
    _length: int;

    // private
    ctor() {}

    ctor(str: char*) {
        var l = 0;

        // get length
        while str[l] != '\0'
            l = l + 1;
            
        _length = l;
        _buffer = malloc(_length).(char*);

        for i = 0 to _length {
            _buffer[i] = str[i];
        }     
    }

    dtor() {
        free(_buffer.(i8*));
    }

    [index: int] : char {
        get {
            return _buffer[index];
        }
    }

    // Get_Length(string* s)
    Length :int {
        get {
            return _length;
        }
    }

    // static -> no this pointer
    static Concat(a: string&, b: string&) : string {
        let length = a._length + b._length;

        let buffer = malloc(length).(char*);

        // move a's content into the result
        for i = 0 to a._length {
            buffer[i] = a[i];
        }

        // move b's content into the result
        for i = 0 to b._length {
            buffer[a._length + i] = b[i];
        }

        let result = string();

        result._buffer = buffer;
        result._length = length;

// what happens here?
// memcpy?


// the return type is allocated in the callee
// an a pointer is given as parameter
// we need demo c++ code for this

        return result;
        // result is not owned anymore so we don't call the destructor here
    }
}
