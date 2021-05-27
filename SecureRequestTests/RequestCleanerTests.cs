using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecureRequest;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecureRequest.Tests
{
    [TestClass()]
    public class RequestCleanerTests
    {
        [TestMethod()]
        public void SecureDataTest()
        {
            //Arrange
            var BookingHttpResult = new HttpResult()
            {
                Url = "http://test.com/users/max/info?pass=123456",
                RequestBody = "http://test.com?user=max&pass=123456",
                ResponseBody = "http://test.com?user=max&pass=123456"
            };
            var bookingRequestCleaner = new RequestCleaner();
            var bookingLogHandler = new HttpLogHandler();
            //Act
            bookingLogHandler.Process(BookingHttpResult.Url, 
                BookingHttpResult.RequestBody, 
                BookingHttpResult.ResponseBody, 
                bookingRequestCleaner);
            //Assert
            Assert.AreEqual("http://test.com/users/XXX/info?pass=XXXXXX", bookingLogHandler.CurrentLog.Url);
            Assert.AreEqual("http://test.com?user=XXX&pass=XXXXXX", bookingLogHandler.CurrentLog.RequestBody);
            Assert.AreEqual("http://test.com?user=XXX&pass=XXXXXX", bookingLogHandler.CurrentLog.ResponseBody);

        }
    }
}