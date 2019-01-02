// single line comment
/* mutli line comment */

// function
Function1() {}
Function2():i32 {0}
Function3(a:i32):i32 {a}
Function4(a:i32, b:i32):i32 {a+b}

// struct
struct Struct1 {
    publicField1: i32
    publicField2: i32 = 0
    publicField3 = 0

	_privateField1: i32

	PublicProperty1: i32 { get set }
	PublicProperty11: i32 { get set } = 0
	PublicProperty12 { get set } = 0

	PublicProperty2: i32 { get }
	PublicProperty3: i32 { get } = 0
	PublicProperty4 { get } = 0
	
	PublicProperty41 { set { _privateField1 = value } }
	PublicProperty42 { set => _privateField1 = value }
		
	PublicProperty5: i32 => 0 // get expr
	PublicProperty6 => 0 // get expr

	PublicProperty7: i32 { 
		get { 0 } 
		set { _privateField1 = value }
	}

	PublicProperty7 { 
		get { 0 } // sets the type
		set { _privateField1 = value }
	}

	PublicProperty8: i32 { 
		get => 0 
		set => _privateField1 = value
	}
	
	PrivateProperty1: i32 { get; set; }

	Method1() {}
	Method2():i32 {0}
	Method3(a:i32) {a}
	Method4()
}

// variable declaration
let a = 0;
var a = 1; // redeclaration replaces the previous one

let b1 = true
var b2 = false
let i1 = 0
var i2 = 1

// expressions
b1 && b2
b1 || b2
i1 == i2
i2 = i1
b1 != b2
i1 < i2
i1 <= i2
+i1
-i1
!b1
~i1
i1 & i2
i1 | i2

// Call and block statement
Example()
{
}

// function declaration
func Example()
{
}

// alias
alias Number = int

// casts
let n = (int)3
let n = (int)-3
let n = (Number)3
let n = (Number)-3

