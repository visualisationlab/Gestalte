using AIDirector.SensorOperations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Editor
{
    [TestFixture]
    public class EvaluationFunctionTest
    {
    
        private SensorAddOperation _addFunction;
        private SensorMultiplyOperation _multiplyFunction;
    
        [SetUp]
        public void SetUp()
        {
            _addFunction = ScriptableObject.CreateInstance<SensorAddOperation>();
            _multiplyFunction = ScriptableObject.CreateInstance<SensorMultiplyOperation>();
        }
    
        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_addFunction);
            Object.DestroyImmediate(_multiplyFunction);
        }
    
        [Test]
        public void EvaluateAddition_ExpectedResult()
        {
            var addition = _addFunction.Evaluate(new[] { 10f, 20f, 20f });
            Assert.AreEqual(50, addition);
        }
    
        [Test]
        public void EvaluateAddition_NullError()
        {
            LogAssert.Expect(LogType.Error, "AddFunction received null or not enough inputs.");
            var addition = _addFunction.Evaluate(new[] { 10f });
            Assert.AreEqual(float.NaN, addition);
        }
    
        [Test]
        public void EvaluateMultiplication_ExpectedResult()
        {
            var addition = _multiplyFunction.Evaluate(new[] { 2f, 2f, 10f });
            Assert.AreEqual(40, addition);
        }
    
        [Test]
        public void EvaluateMultiplication_NullError()
        {
            LogAssert.Expect(LogType.Error, "MultiplyFunction received null or not enough inputs.");
            var addition = _multiplyFunction.Evaluate(new[] { 1f });
            Assert.AreEqual(float.NaN, addition);
        }
    }
}