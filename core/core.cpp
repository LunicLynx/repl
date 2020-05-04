#include <iostream>
#include <string>
#include <iomanip>

struct string
{
    char* buffer;
    long long length;
};


std::string input;

void print(const void* data, int length)
{
    for (size_t i = 0; i < length; i++)
    {
        auto c = ((unsigned char*)data)[i];
        std::cout << std::setw(2) << std::setfill('0') << std::hex << (int) c;
    }
}

extern "C" {
    void Main();

    // extern Input(): string;

    void Input(string* p){

        // print(p, sizeof(string));
        //auto result = new char[1000];
        std::getline(std::cin, input);

        // TODO this is not a good way
        // but it works for now
        //return input.c_str();

        p->buffer = (char*)malloc(input.length());
        memcpy(p->buffer, input.c_str(), input.length());
        p->length = input.length();
        print(p, sizeof(string));
        std::cout << std::endl;
    }

    void Print(string* s) {
        print(s, sizeof(string));
        auto str = std::string(s->buffer, s->length);
        std::cout << " " << str << std::endl;
    }
}



int main() {
    Main();
}