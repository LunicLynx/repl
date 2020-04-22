#include <iostream>

typedef const char* string;

extern "C" {
    void Main();

    string Input(){
        auto result = new char[1000];
        std::cin >> result;
        return result;
    }

    void Print(string s) {
        std::cout << s << std::endl;
    }
}

int main() {
    Main();
}