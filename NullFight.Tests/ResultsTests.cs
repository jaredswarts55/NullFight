using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NullFight.Exceptions;
using static NullFight.FunctionalExtensions;

// ReSharper disable All - we are displaying the explicit type instead of var for documentation purposes
namespace NullFight.Tests
{
    [TestClass]
    public class ResultTests
    {
        [TestMethod]
        public async Task Result_Examples_Work()
        {

            // Use the static method to create a result
            Result<string> stringResult = ResultValue("My String");

            // You can get the inner value by calling Expect with a message to throw in the
            // exception if instead the result had an error in it. In this case it has a string.
            Assert.AreEqual("My String", stringResult.Expect("Could not get the string out"));

            // You can also retrieve the inner value by calling Unwrap. THIS THROWS AN ERROR IF THERE
            // WAS AN EXCEPTION IN IT. You should check first using HasValue or use expect to give a better error message.
            Assert.AreEqual("My String", stringResult.Unwrap());

            // Proper way to use Unwrap() making sure we check with HasValue first
            if (stringResult.HasValue)
            {
                // ok to do this here
                string theString3 = stringResult.Unwrap();
            }
            else
            {
                // won't hit this code since our result has a value.
                throw stringResult.UnwrapException();
            }

            Result<object> errorResult = ResultError(new Exception("Something Failed"));

            // Unwrap throws an exception here since this result object has an error.
            Assert.ThrowsException<ResultException>(() => errorResult.Unwrap());

            // Expect throws an exception here since this result object has an error.
            Assert.ThrowsException<FriendlyResultException>(() => errorResult.Expect("My More Detailed Error"));

            // A result can be mapped to another type using the MapValue function.
            Result<int> intResult = stringResult.MapValue(x => x.Length);
            Assert.AreEqual(intResult.Unwrap(), "My String".Length);

            // This can also be done on an error result and it will pass through the error to the new result
            Result<int> intResultWithError = errorResult.MapValue(x => default(int));
            Assert.ThrowsException<ResultException>(() => intResultWithError.Unwrap());

            // Let's go over some of the extension methods if you are returning Task<Result<T>>
            Task<Result<string>> stringResultTask = Task.FromResult(ResultValue("My String"));

            // There are extension methods that operate on a task allowing the value returned after an await
            // to be the inner value instead of the result. Allowing this to occur on one line.
            string theString4 = await stringResultTask.Expect("My More Detailed Error");
            Assert.AreEqual(theString4, "My String");

            // Doing this without the extension method
            Result<string> stringResult2 = (await stringResultTask);
            string theString5 = stringResult2.Expect("My More Detailed Error");
            Assert.AreEqual(theString5, "My String");

            // There are many tools that improve writing methods that return a Result Take a look at the method code below
            Result<int> intResult1 = MethodReturningResult();

            // We can also use c#7 type matching with a result by calling GetValueOrError which returns either item cast to an object for
            // pattern matching.
            switch (intResult1.GetValueOrError())
            {
                case int myInt:
                    // Shouldn't hit this
                    Assert.IsTrue(false);
                    break;
                case Exception ex:
                    Assert.IsTrue(true);
                    break;
            }

        }

        private Result<int> MethodReturningResult()
        {
            // using this just so Visual Studio / Resharper don't complain about code that won't ever be run.
            var businessLogicGarbage = 99;

            // If a service returns a result of a different type
            Result<string> serviceResult = MockServiceMethodReturningResultValue();

            // We can use Map value to map the service value and keep the error
            if (businessLogicGarbage == 1)
                return serviceResult.MapValue(x => x.Contains("My") ? 1 : 2);

            // Using MatchToResult to handle some cases of the exception.
            if (businessLogicGarbage == 2)
                return serviceResult.MatchToResult(x => x.Contains("My") ? 1 : 2, x =>
                {
                    if (x is ArgumentException)
                        return 1;
                    throw x;
                });

            // Using implicit casting so we don't need to supply a type argument
            if (businessLogicGarbage == 3)
                return ResultError("My Error");

            return ResultError("Some Error");
        }


        [TestMethod]
        public void HasValue_WithResultValue_ReturnsTrue()
        {
            var result = MockServiceMethodReturningResultValue();
            Assert.IsTrue(result.HasValue);
        }

        [TestMethod]
        public void Expect_WithResultValue_ReturnsValue()
        {
            var result = MockServiceMethodReturningResultValue().Expect("Could not get value");
            Assert.AreEqual(result, "Testing");
        }

        [TestMethod]
        public void Unwrap_WithResultValue_ReturnsValue()
        {
            var result = MockServiceMethodReturningResultValue().Unwrap();
            Assert.AreEqual(result, "Testing");
        }

        [TestMethod]
        public void MapValue_WithResultValue_ReturnsNewResultType()
        {
            var result = MockServiceMethodReturningResultValue().MapValue(x => 1);
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(result.Unwrap(), 1);
        }

        [TestMethod]
        public void UnwrapException_WithException_ExceptionObject()
        {
            var result = MockServiceMethodReturningResultError<Exception>();
            Assert.IsFalse(result.HasValue);
            Assert.AreEqual(result.UnwrapException().Message, "Test Error");
        }


        [TestMethod]
        public void UnwrapException_WithResultValue_ThrowsResultException()
        {
            var result = MockServiceMethodReturningResultValue();
            Assert.IsTrue(result.HasValue);
            Assert.ThrowsException<ResultException>(() => result.UnwrapException());
        }

        [TestMethod]
        public void UnwrapExceptionGeneric_WithArgumentNullException_ReturnsTypedException()
        {
            var result = MockServiceMethodReturningResultError(new ArgumentNullException("Test"));
            Assert.IsFalse(result.HasValue);
            Assert.AreEqual(result.UnwrapException<ArgumentNullException>().ParamName, "Test");
        }


        private Result<string> MockServiceMethodReturningResultValue()
        {
            return ResultValue("Testing");
        }

        private Result<string> MockServiceMethodReturningResultError<T>(T ex = null)
            where T : Exception
        {
            // Casts to Result<string> from Result<object> using implicit cast override.
            return ResultError(ex ?? new Exception("Test Error"));
        }
    }
}
