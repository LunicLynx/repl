//#include <string>

//using namespace std;

typedef long long i64;

class Empty {
public:
    Empty() {

    }
};

class Single {
private: 
    i64 value;

public:
    Single() {

    }

    void Act() {
        Act();
    }

    Single Create(const Single& arg){
        return Single();
    }

    static void StaticAct() {

    }
};

struct i64_2{
    i64 a;
    i64 b;
};

void print() {}

void print1(i64 number){

}

void print2(i64& number){
    
}

void print3(i64* number){
    
}

void print4(i64_2 strct){

}

void print5(i64_2& strct){
    
}

void print5_1(const i64_2& strct){
    
}

void print5_2(i64_2&& strct){
    
}

void print6(i64_2* strct){
    
}

i64 print7() {
    return 0LL;
}

i64* print8() {
    i64 x = 0LL;
    return &x;
}

i64& print9() {
    i64 x = 0LL;
    return x;
}

i64_2 print10(){
    return i64_2();
}

Single print10_2(){
    return Single();
}

i64_2& print11(){
    auto x = i64_2();
    return x;
}

i64_2* print12(){
    auto x = i64_2();
    return &x;
}



i64 add1(i64 a, i64 b)
{
    return a + b;
}

i64 add2(i64 &a, i64 &b)
{
    auto x = a + b;
    return x;
}

i64 add3(i64 *a, i64 *b)
{
    return *a + *b;
}

class Point
{
public:
    i64 x;
    i64 y;
    i64 z;
    i64 w;
};

Point padd1(Point a, Point b)
{
    auto p = Point();
    p.x = a.x + b.x;
    p.y = a.y + b.y;
    p.z = a.z + b.z;
    p.w = a.w + b.w;
    return p;
}

Point padd2(Point& a, Point& b)
{
    auto p = Point();
    p.x = a.x + b.x;
    p.y = a.y + b.y;
    p.z = a.z + b.z;
    p.w = a.w + b.w;
    return p;
}

Point padd3(Point* a, Point* b)
{
    auto p = Point();
    p.x = a->x + b->x;
    p.y = a->y + b->y;
    p.z = a->z + b->z;
    p.w = a->w + b->w;
    return p;
}

Point ReturnTest1(){
    auto a = Point();
    return a;
}

Point ReturnTest2(){
    auto a = Point();
    auto b = Point();
    if(a.x > 5)
      return b;
    return a;
}

int main()
{
    auto a = 4LL, b = 7LL;
    add1(5LL, 6LL);
    auto x1 = add1(a, b);
    auto x2 = add2(a, b);
    auto x3 = add3(&a, &b);

    auto ps = Single();
    auto px = Point();
    auto pa = Point { 1LL, 5LL, 8LL, 3LL };
    auto pb = Point { 6LL, 3LL, 2LL, 4LL };
    auto px1 = padd1(pa, pb);
    auto px2 = padd2(pa, pb);
    auto px3 = padd3(&pa, &pb);

    auto e = Empty();
    auto sa = i64_2();

    print();
    print1(0LL);
    print2(a);
    print3(&a);
    print4(i64_2());
    print4(sa);
    // print5(i64_2());
    print5_1(i64_2());
    print5_1(sa);
    print5_2(i64_2());
    // print5_2(sa);
    print5(sa);
    print6(&sa);
    print7();
    print8();
    print9();
    print10();
    print10_2();
    print11();
    print12();

    ps.Act();
    Single::StaticAct();

    i64_2().a;

    auto y = i64_2();
    y.a = 10;

    auto& z = y;
    z.a = 10;

    //string s1 = "Hallo";
    //string s2 = "Welt";
    //auto abc = s1 + " " + s2;

    auto bc = Single();
    auto dc = bc.Create(Single());

    auto ac = bc;

    ReturnTest1();
    ReturnTest2();
}