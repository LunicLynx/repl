static x : Point = Point {
    x: 1,
        y: 2,
        z: 3,
        w: 4,
};

fn main() {
    println!("Hello, world!");

    let p = Point {
        x: 1,
        y: 2,
        z: 3,
        w: 4,
    };
    let p2 = Point {
        x: 1,
        y: 2,
        z: 3,
        w: 4,
    };

    let pr = addr(&p, &p2);
    let pr = addr(&p, &x);
    

    let p4 = addr(&addr(&p, &p2), &addr(&p, &p2));


    let p3 = add(p, p2);

    let s = "Hallo welt!";
    let s2 = s;
    
    println!("{}", s);

    // let y = x;
    // let z = y;
    // let w = x;
}

struct Point {
    x: i32,
    y: i32,
    z: i32,
    w: i32
}

fn addr(a: &Point, b: &Point) -> Point {
    return Point {
        x: a.x + b.x,
        y: a.y + b.y,
        z: a.z + b.z,
        w: a.w + b.w
    };
}

fn add(a: Point, b: Point) -> Point {
    return Point {
        x: a.x + b.x,
        y: a.y + b.y,
        z: a.z + b.z,
        w: a.w + b.w
    };
}
