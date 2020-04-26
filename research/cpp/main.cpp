int add1(int a, int b)
{
    return a + b;
}

int add2(int &a, int &b)
{
    auto x = a + b;
    return x;
}

int add3(int *a, int *b)
{
    return *a + *b;
}

class Point
{
public:
    int x;
    int y;
    int z;
    int w;
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
    auto a = 4, b = 7;
    auto x1 = add1(a, b);
    auto x2 = add2(a, b);
    auto x3 = add3(&a, &b);

    auto pa = Point { 1, 5 };
    auto pb = Point { 6, 3 };
    auto px1 = padd1(pa, pb);
    auto px2 = padd2(pa, pb);
    auto px3 = padd3(&pa, &pb);
}