extern Print(s: string);
extern Input(): string;

object String {
    [index: int] : char {
        get {
            return this.(char*)[index];
        }
    }

    Length :int {
        get {
            var l = 0;
            let self = this.(char*);
            while self[l] != '\0'
                l++;
            return l;
        }
    }
}

Concat(a: string, b: string) : string {

    var length = a.Length + b.Length

    var result  = new char[length + 1];

    // move a's content into the result
    for i = 0 to a.Length {
        result[i] = a[i];
    }

    // move b's content into the result
    for i = 0 to b.Length {
        result[a.Length + i] = b[i];
    }

    result[length] = '\0';

    return result;
}