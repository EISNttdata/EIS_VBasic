﻿namespace EaiConverter.Test.Builder
{
    using EaiConverter.Builder;
    using EaiConverter.Test.Utils;

    using NUnit.Framework;

    public class SubscriberBuilderTest
    {
        private SubscriberInterfaceBuilder subscriberBuilder;

        [SetUp]
        public void SetUp()
        {
            this.subscriberBuilder = new SubscriberInterfaceBuilder();
        }

        [Test]
        public void Should_generate_SubscriberInterface ()
        {
            var expected = @"namespace MyApp.Tools.EventSourcing
{
    using System;
    
    
    public interface ISubscriber
    {
        
        System.Int32 WaitingTimeLimit { get; }
        System.Boolean IsStarted { get; }
        event EventHandler ResponseReceived;
        
        void Start();
        
        void Stop();
        
        void Confirm();
    }
}
";
            var generatedCode = TestCodeGeneratorUtils.GenerateCode(this.subscriberBuilder.GenerateClasses()[0]);
			Assert.AreEqual(expected.RemoveWindowsReturnLineChar(), generatedCode);
        }
    }
}