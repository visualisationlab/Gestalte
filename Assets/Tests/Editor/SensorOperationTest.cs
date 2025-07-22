using NUnit.Framework;

namespace Tests.Editor
{
    [TestFixture]
    public class SensorOperationTest
    {
    
        [SetUp]
        public void SetUp()
        {
            // _multiplyFunction = ScriptableObject.CreateInstance<SensorMultiplyOperation>();
        }
    
        [TearDown]
        public void TearDown()
        {
            // Object.DestroyImmediate(_addFunction);
        }
    
        [Test]
        public void EvaluateAddition_ExpectedResult()
        {
            // var addition = _addFunction.Evaluate(new[] { 10f, 20f, 20f });
            // Assert.AreEqual(50, addition);
        }
    
        [Test]
        public void EvaluateAddition_NullError()
        {
            // LogAssert.Expect(LogType.Error, "AddFunction received null or not enough inputs.");
            // var addition = _addFunction.Evaluate(new[] { 10f });
            // Assert.AreEqual(float.NaN, addition);
        }
    
    }
}