

Main() {
    // creation
    let a = String("Abc");
    let b = String("Def");

    // this will lose a its ownership of the string
    let e = a;

    // concatenation
    let c = a + b;

    // print
    Print(c);

    // destory at end of scope
}