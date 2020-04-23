#include <iostream>
#include <string>

typedef const char* string;

std::string input;

extern "C" {
    void Main();

    string Input(){
        auto result = new char[1000];
        std::getline(std::cin, input);

        // TODO this is not a good way
        // but it works for now
        return input.c_str();
    }

    void Print(string s) {
        std::cout << s << std::endl;
    }
}

int main() {
    Main();
}