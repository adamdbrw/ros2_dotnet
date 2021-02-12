using NUnit.Framework;
using System;
using System.Linq;

namespace rclcs.Test
{
    [TestFixture]
    public class LargeMessageTest
    {
        Context pub_context;
        Context sub_context;
        Node pub_node;
        Node sub_node;

        [SetUp]
        public void SetUp()
        {
            pub_context = new Context();
            sub_context = new Context();

            Rclcs.Init(pub_context);
            Rclcs.Init(sub_context);

            pub_node = new Node("pub_node", pub_context);
            sub_node = new Node("sub_node", sub_context);
        }

        [TearDown]
        public void TearDown()
        {
            pub_node.Dispose();
            sub_node.Dispose();
            Rclcs.Shutdown(pub_context);
            Rclcs.Shutdown(sub_context);
        }

        [Test]
        public void ImagePubSub()
        {
            bool callbackTriggered = false;

            var subscription = sub_node.CreateSubscription<sensor_msgs.msg.Image>(
              "test_topic", (received_msg) =>
              {
                callbackTriggered = true;
                Assert.That(received_msg.Data.Length, Is.EqualTo(10));
              });

            var publisher = pub_node.CreatePublisher<sensor_msgs.msg.Image>("test_topic");
            var msg = new sensor_msgs.msg.Image();
            msg.Data = new byte[10];
            msg.Data[0] = 1;
            publisher.Publish(msg);

            Rclcs.SpinOnce(sub_node, sub_context, 0.5);

            Assert.That(callbackTriggered, Is.True);

            publisher.Dispose();
            subscription.Dispose();
        }

    }
}
