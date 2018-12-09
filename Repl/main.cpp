#include <iostream>

//extern "C"
//{
//    int sum(int, int);
//}

//extern "C"
//{
//    void mainlang();
//}

extern "C"
{
    int __anon_expr();
}

int main()
{
    /*int x = 4, y = 5;
    std::cout << "sum of " << x << " and " << y << " is " << sum(x, y) << std::endl;*/
    //mainlang();

    std::cout << __anon_expr() << std::endl;
}