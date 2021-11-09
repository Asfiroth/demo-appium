using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium.Windows.Enums;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Interactions.Internal;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace iOSTest
{
    [TestClass]
    public class UnitTest1
    {
        private static TestContext _context;
        private static IOSDriver<IOSElement> _driver;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _context = context;

            var capabilities = new AppiumOptions();
            capabilities.AddAdditionalCapability(IOSMobileCapabilityType.BundleId, "com.example.apple-samplecode.SuggestedSearch");
            capabilities.AddAdditionalCapability(MobileCapabilityType.PlatformName, "ios");
            capabilities.AddAdditionalCapability(MobileCapabilityType.DeviceName, "iPhone 11");
            capabilities.AddAdditionalCapability(MobileCapabilityType.AutomationName, "XCUITest");
            capabilities.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "15.0");

            _driver = new IOSDriver<IOSElement>(new Uri("http://127.0.0.1:4723/wd/hub"), capabilities);
        }

        [TestMethod]
        public void GetUIDocument()
        {
            _driver.LaunchApp();
            var document = _driver.PageSource;
            Assert.IsTrue(!string.IsNullOrWhiteSpace(document));
            _context.WriteLine($"this is current page: {document}");
            _driver.CloseApp();
        }
        
        [TestMethod]
        public void ScrollToEndOfListUsingPointerInputDevice()
        {
            _driver.LaunchApp();
            var listView = _driver.FindElement(By.ClassName("ListView"));

            FlickUp(_driver, listView);
            Thread.Sleep(3000);
            FlickUp(_driver, listView);
            Thread.Sleep(3000);
            
            Assert.IsTrue(listView != null);
            _driver.CloseApp();
        }
        
        [TestMethod]
        public void CheckMasterDetailAndBack()
        {
            _driver.LaunchApp();
            Thread.Sleep(3000);
            CreateScreenshot();
            var firstCell = _driver.FindElementByAccessibilityId("Ginger");
            firstCell.Click();
            
            var yearLabel = _driver.FindElementByAccessibilityId("2007");
            Thread.Sleep(3000);
            CreateScreenshot();
            Assert.IsTrue(yearLabel.Text == "2007");

            var backButton = _driver.FindElementByAccessibilityId("Suggested Search");
            backButton.Click();

            var randomCell = _driver.FindElementByAccessibilityId("Poinsettia");
            Assert.IsTrue(randomCell != null);
            Thread.Sleep(3000);
            
            CreateScreenshot();
            _driver.CloseApp();
        }
        
        private void FlickUp(AppiumDriver<IOSElement> driver, IWebElement element)
        {
            var input = new PointerInputDevice(PointerKind.Touch);
            ActionSequence flickUp = new ActionSequence(input);
            flickUp.AddAction(input.CreatePointerMove(element, 0, 0, TimeSpan.Zero));
            flickUp.AddAction(input.CreatePointerDown(MouseButton.Left));
            flickUp.AddAction(input.CreatePointerMove(element, 0, 600, TimeSpan.FromMilliseconds(200)));
            flickUp.AddAction(input.CreatePointerUp(MouseButton.Left));
            driver.PerformActions(new List<ActionSequence>() { flickUp });
        }

        private void CreateScreenshot()
        {
            var screenshot = _driver.GetScreenshot();
            var screenshotName = $"../../../../TestResults/{Guid.NewGuid():D}.png";
            screenshot.SaveAsFile(screenshotName, OpenQA.Selenium.ScreenshotImageFormat.Png);
            _context.AddResultFile(screenshotName);
        }
    
    }    
}

