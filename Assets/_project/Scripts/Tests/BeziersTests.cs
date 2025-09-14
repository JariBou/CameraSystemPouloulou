using NUnit.Framework;
using UnityEngine;

namespace CameraSystem._project.Scripts.Tests
{
    [TestFixture]
    public class BeziersTests
    {
        
        
        [Test]
        public void LinearTests()
        {
            var testA = new Vector3(1, 8, 3.5f);
            var testB = new Vector3(5, -7, 5);
            var t = .6f;
            
            Assert.AreEqual(MathUtils.LinearBeziers(testA, testB, t), MathUtils.VariadicBeziers(t, testA, testB));
            t = .1f;
            Assert.AreEqual(MathUtils.LinearBeziers(testA, testB, t), MathUtils.VariadicBeziers(t, testA, testB));
            t = 1f;
            Assert.AreEqual(MathUtils.LinearBeziers(testA, testB, t), MathUtils.VariadicBeziers(t, testA, testB));
        }
        
        [Test]
        public void QuadraticTests()
        {
            var testA = new Vector3(1, 8, 3.5f);
            var testB = new Vector3(5, -7, 5);
            var testC = new Vector3(-36, -5, 2);
            var t = .6f;
            
            Assert.AreEqual(MathUtils.QuadraticBeziers(testA, testB, testC, t), MathUtils.VariadicBeziers(t, testA, testB, testC));
            t = .1f;
            Assert.AreEqual(MathUtils.QuadraticBeziers(testA, testB, testC, t), MathUtils.VariadicBeziers(t, testA, testB, testC));
            t = 1f;
            Assert.AreEqual(MathUtils.QuadraticBeziers(testA, testB, testC, t), MathUtils.VariadicBeziers(t, testA, testB, testC));
        }
        
        [Test]
        public void CubicTests()
        {
            var testA = new Vector3(1, 8, 3.5f);
            var testB = new Vector3(5, -7, 5);
            var testC = new Vector3(-36, -5, 2);
            var testD = new Vector3(-3.4f, 7, 3);
            var t = .6f;
            
            Assert.AreEqual(MathUtils.CubicBeziers(testA, testB, testC, testD, t), MathUtils.VariadicBeziers(t, testA, testB, testC, testD));
            t = .1f;
            Assert.AreEqual(MathUtils.CubicBeziers(testA, testB, testC, testD, t), MathUtils.VariadicBeziers(t, testA, testB, testC, testD));
            t = 1f;
            Assert.AreEqual(MathUtils.CubicBeziers(testA, testB, testC, testD, t), MathUtils.VariadicBeziers(t, testA, testB, testC, testD));
        }
        
        
    }
}