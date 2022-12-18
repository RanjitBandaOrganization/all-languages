var TestClass = /** @class */ (function () {
    function TestClass(a, b) {
        x = a;
        y = b;
    }
    TestClass.prototype.display = function () {
        console.log(x);
        console.log(y);
    };
    return TestClass;
}());
var testing = new TestClass("This is a", "This is b");
