
typedef const char* string;

int length(string* self)
{
    auto l = 0;
    while ((*self)[l] != '\0')
        l++;
    return l;
}

char item(string* self, int index)
{
    return (*self)[index];
}

string concat(string a, string b)
{
    auto len = length(&a) + length(&b);
    auto result = new char[len + 1];

    for (auto i = 0; i < length(&a); i++)
    {
        result[i] = item(&a, i);
    }

    for (auto i = 0; i < length(&b); i++)
    {
        result[length(&a) + i] = item(&b, i);
    }

    result[len] = '\0';

    return result;
}

extern "C" {
    void Print(string s);
    string Input();
}

int main()
{
    Print(concat("Hello ", concat("Flo", " !")));
}