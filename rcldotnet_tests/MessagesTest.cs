using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;

namespace rclcs.Test
{
    [TestFixture()]
    public class MessagesTest
    {
        [Test]
        public void CreateMessage()
        {
            std_msgs.msg.Bool msg = new std_msgs.msg.Bool();
        }

        [Test]
        public void SetBoolData()
        {
            var msg = new std_msgs.msg.Bool();
            var msgCopy = new std_msgs.msg.Bool();

            msg.ReadNativeMessage();
            Assert.That(msg.Data, Is.False);

            msg.Data = true;
            msg.WriteNativeMessage();
            msgCopy.ReadNativeMessage(msg.Handle);
            Assert.That(msgCopy.Data, Is.True);

            msg.Data = false;
            msg.WriteNativeMessage();
            msgCopy.ReadNativeMessage(msg.Handle);
            Assert.That(msgCopy.Data, Is.False);
        }

        [Test]
        public void SetInt64Data()
        {
            var msg = new std_msgs.msg.Int64();
            var msgCopy = new std_msgs.msg.Int64();

            Assert.That(msg.Data, Is.EqualTo(0));
            msg.Data = 12345;
            msg.WriteNativeMessage();
            msgCopy.ReadNativeMessage(msg.Handle);
            Assert.That(msgCopy.Data, Is.EqualTo(12345));
        }

        [Test]
        public void SetDefaults()
        {
            var msg = new test_msgs.msg.Defaults();
            var msgCopy = new test_msgs.msg.Defaults();

            msg.Int32_value = 24;
            msg.WriteNativeMessage();
            msgCopy.ReadNativeMessage(msg.Handle);
            Assert.That(msgCopy.Int32_value, Is.EqualTo(24));

            msg.Float32_value = 3.14F;
            msg.WriteNativeMessage();
            msgCopy.ReadNativeMessage(msg.Handle);
            Assert.That(msgCopy.Float32_value, Is.EqualTo(3.14F));
        }

        [Test]
        public void SetStrings()
        {
            var msg = new test_msgs.msg.Strings();
            var msgCopy = new test_msgs.msg.Strings();

            msg.ReadNativeMessage();
            Assert.That(msg.String_value, Is.EqualTo(""));

            msg.String_value = "Turtles all the way down";
            msg.WriteNativeMessage();
            msgCopy.ReadNativeMessage(msg.Handle);
            Assert.That(msgCopy.String_value, Is.EqualTo("Turtles all the way down"));
        }

        // NOTE(sam): Bool arrays seem to not work yet
        [Test]
        public void SetUnboundedSequences()
        {
            test_msgs.msg.UnboundedSequences msg = new test_msgs.msg.UnboundedSequences();
            test_msgs.msg.UnboundedSequences msgCopy = new test_msgs.msg.UnboundedSequences();

            bool[] setBoolSequence = new bool[2];
            setBoolSequence[0] = true;
            setBoolSequence[1] = false;
            msg.Bool_values = setBoolSequence;

            // msg.WriteNativeMessage();
            // msg.ReadNativeMessage();
            // Assert.That(msg.Bool_values_size, Is.EqualTo(2));
            // msgCopy.ReadNativeMessage(msg.Handle);

            // bool[] getBoolSequence = msg.Bool_values;
            // Assert.That(msg.Bool_values_size, Is.EqualTo(2));
            // Assert.That(getBoolSequence.Length, Is.EqualTo(2));
            // Assert.That(getBoolSequence[0], Is.True);
            // Assert.That(getBoolSequence[1], Is.False);

            int[] setIntSequence = new int[2];
            setIntSequence[0] = 123;
            setIntSequence[1] = 456;
            msg.Int32_values = setIntSequence;
            int[] getIntList = msg.Int32_values;

            Assert.That(getIntList.Length, Is.EqualTo(2));
            Assert.That(getIntList[0], Is.EqualTo(123));
            Assert.That(getIntList[1], Is.EqualTo(456));

            string[] setStringSequence = new string[2];
            setStringSequence[0] = "Hello";
            setStringSequence[1] = "world";
            msg.String_values = setStringSequence;
            string[] getStringSequence = msg.String_values;
            Assert.That(getStringSequence.Length, Is.EqualTo(2));
            Assert.That(getStringSequence[0], Is.EqualTo("Hello"));
            Assert.That(getStringSequence[1], Is.EqualTo("world"));

            Byte[] setUint8Sequence = new Byte[2];
            setUint8Sequence[0] = 1;
            setUint8Sequence[1] = 2;
            msg.Uint8_values = setUint8Sequence;
            Byte[] getUint8Sequence = msg.Uint8_values;
            Assert.That(getUint8Sequence.Length, Is.EqualTo(2));
            Assert.That(getUint8Sequence[0], Is.EqualTo(1));
            Assert.That(getUint8Sequence[1], Is.EqualTo(2));
        }


        [Test]
        public void SetBoundedSequences()
        {
            test_msgs.msg.BoundedSequences msg = new test_msgs.msg.BoundedSequences();
            bool[] setBoolSequence = new bool[2];
            setBoolSequence[0] = true;
            setBoolSequence[1] = false;
            msg.Bool_values = setBoolSequence;

            bool[] getBoolSequence = msg.Bool_values;
            Assert.That(getBoolSequence.Length, Is.EqualTo(2));
            Assert.That(getBoolSequence[0], Is.True);
            Assert.That(getBoolSequence[1], Is.False);

            int[] setIntSequence = new int[2];
            setIntSequence[0] = 123;
            setIntSequence[1] = 456;
            test_msgs.msg.BoundedSequences msg2 = new test_msgs.msg.BoundedSequences();
            msg2.Int32_values = setIntSequence;
            int[] getIntList = msg2.Int32_values;
            Assert.That(getIntList.Length, Is.EqualTo(2));
            Assert.That(getIntList[0], Is.EqualTo(123));
            Assert.That(getIntList[1], Is.EqualTo(456));

            string[] setStringSequence = new string[2];
            setStringSequence[0] = "Hello";
            setStringSequence[1] = "world";
            test_msgs.msg.BoundedSequences msg3 = new test_msgs.msg.BoundedSequences();
            msg3.String_values = setStringSequence;
            string[] getStringSequence = msg3.String_values;
            Assert.That(getStringSequence.Length, Is.EqualTo(2));
            Assert.That(getStringSequence[0], Is.EqualTo("Hello"));
            Assert.That(getStringSequence[1], Is.EqualTo("world"));
        }

        [Test]
        public void SetNested()
        {
            var msg = new test_msgs.msg.Nested();
            var msgCopy = new test_msgs.msg.Nested();
            Assert.That(msg.Basic_types_value.Int32_value, Is.EqualTo(0));
            msg.Basic_types_value.Int32_value = 25;
            msg.WriteNativeMessage();
            msgCopy.ReadNativeMessage(msg.Handle);
            Assert.That(msgCopy.Basic_types_value.Int32_value, Is.EqualTo(25));
        }

        // FIXME(sam): use ReadNativeMessage and WriteNativeMessage
        [Test]
        public void SetMultiNested()
        {
            test_msgs.msg.MultiNested msg = new test_msgs.msg.MultiNested();

            msg.Unbounded_sequence_of_unbounded_sequences = new test_msgs.msg.UnboundedSequences[3];
            var setUnboundedSequences = new test_msgs.msg.UnboundedSequences();
            string[] string_array = new string[2];
            setUnboundedSequences.String_values = string_array;
            setUnboundedSequences.String_values[0] = "hello";

            msg.Unbounded_sequence_of_unbounded_sequences[0] = setUnboundedSequences;
            msg.Unbounded_sequence_of_unbounded_sequences[0].String_values[1] = "world";

            Assert.That(msg.Unbounded_sequence_of_unbounded_sequences.Length, Is.EqualTo(3));

            var getUnboundedOfUnbounded = msg.Unbounded_sequence_of_unbounded_sequences;

            Assert.That(getUnboundedOfUnbounded[0].String_values[0], Is.EqualTo("hello"));
            Assert.That(getUnboundedOfUnbounded[0].String_values[1], Is.EqualTo("world"));
        }

        [Test]
        public void SetImage()
        {
            var msg = new sensor_msgs.msg.Image();
            var msgCopy = new sensor_msgs.msg.Image();

            Assert.That(msg.Data.Length, Is.EqualTo(0));
            msg.Data = new byte[10];
            msg.WriteNativeMessage();
            Assert.That(msg.Data.Length, Is.EqualTo(10));
            msg.Data = new byte[2];
            msg.ReadNativeMessage();
            Assert.That(msg.Data.Length, Is.EqualTo(10));
            msg.Data = new byte[8];
            // NOTE(sam): Writing twice seems not work
            // msg.WriteNativeMessage();
            msg.ReadNativeMessage();
            Assert.That(msg.Data.Length, Is.EqualTo(10));
            msgCopy.ReadNativeMessage(msg.Handle);
            Assert.That(msgCopy.Data.Length, Is.EqualTo(10));
        }

        /*
        // NOTE(samiam): does not work yet
        [Test]
        public void SetStaticArrayPrimitives()
        {
            test_msgs.msg.StaticArrayPrimitives msg = new test_msgs.msg.StaticArrayPrimitives();
            List<bool> setBoolSequence = new List<bool>();
            setBoolSequence.Add(true);
            setBoolSequence.Add(false);
            msg.bool_values = setBoolSequence;
            List<bool> getBoolSequence = msg.bool_values;
            Assert.That(getBoolSequence.Count, Is.EqualTo(3));
            Assert.That(getBoolSequence[0], Is.True);
            Assert.That(getBoolSequence[1], Is.False);

            //List<int> setIntSequence = new List<int>();
            //setIntSequence.Add(123);
            //setIntSequence.Add(456);
            //test_msgs.msg.StaticArrayPrimitives msg2 = new test_msgs.msg.StaticArrayPrimitives();
            //msg2.int32_values = setIntSequence;
            //List<int> getIntList = msg2.int32_values;
            //Assert.That(getIntList.Count, Is.EqualTo(3));
            //Assert.That(getIntList[0], Is.EqualTo(123));

            //List<string> setStringList = new List<string>();
            //setStringList.Add("Hello");
            //setStringList.Add("world");
            //test_msgs.msg.StaticArrayPrimitives msg3 = new test_msgs.msg.StaticArrayPrimitives();
            //msg3.string_values = setStringList;
            //List<string> getStringList = msg3.string_values;
            //Assert.That(getStringList.Count, Is.EqualTo(3));
            //Assert.That(getStringList[0], Is.EqualTo("Hello"));
            //Assert.That(getStringList[1], Is.EqualTo("world"));
        } */
    }
}
