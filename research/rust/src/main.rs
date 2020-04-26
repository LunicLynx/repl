fn main() {
    //println!("Hello, world!");
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

    let p3 = add(p, p2);
}

struct Point {
    x: i32,
    y: i32,
    z: i32,
    w: i32
}

fn add(a: Point, b: Point) -> Point {
    return a;
}
