typedef long long i64;

class empty {
public:
    empty() {

    }
};

struct i64_2{
    i64 a;
    i64 b;
};

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

void print6(i64_2* strct){
    
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

int main()
{
    auto a = 4LL, b = 7LL;
    add1(5LL, 6LL);
    auto x1 = add1(a, b);
    auto x2 = add2(a, b);
    auto x3 = add3(&a, &b);

    auto pa = Point { 1LL, 5LL, 8LL, 3LL };
    auto pb = Point { 6LL, 3LL, 2LL, 4LL };
    auto px1 = padd1(pa, pb);
    auto px2 = padd2(pa, pb);
    auto px3 = padd3(&pa, &pb);

    auto e = empty();
}