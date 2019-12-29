using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Corellian.Test
{
    public class NavigationParameterTest
    {
        private readonly INavigationParameter navigationParameter;
        private const string Enum = "Enum";
        private const string String = "String";
        private const string Class = "TestClass";
        private const string Struct = "TestStruct";



        public NavigationParameterTest()
        {
            navigationParameter = new NavigationParameter 
            {
                {Enum, TestEnum.Three },
                {String, String },
                {Class, TestClass.GetClass()},
                {Struct, TestStruct.GetStruct() }
            };
        }

        [Fact]
        public void GetParameter_WithClass_ExpectClass()
        {
            var result = navigationParameter.GetParameter<TestClass>(Class);

            result.Should().BeEquivalentTo(TestClass.GetClass());
        }

        [Fact]
        public void GetParameter_WithClassWrongKey_ExpectNull()
        {
            var result = navigationParameter.GetParameter<TestClass>("Invalid");

            result.Should().BeNull();
        }

        [Fact]
        public void GetParameter_WithClassWrongType_ExpectNull()
        {
            var result = navigationParameter.GetParameter<TestClass>(Struct);

            result.Should().BeNull();
        }

        [Fact]
        public void GetParameter_WithStruct_ExpectStruct()
        {
            var result = navigationParameter.GetParameter<TestStruct>(Struct);

            result.Should().BeEquivalentTo(TestStruct.GetStruct());
        }

        [Fact]
        public void GetParameter_WithEnum_ExpectThree()
        {
            var result = navigationParameter.GetParameter<TestEnum>(Enum);

            result.Should().Be(TestEnum.Three);
        }


        [Fact]
        public void GetRequiredParameter_WithEnum_ExpectThree()
        {
            var result = navigationParameter.GetRequiredParameter<TestEnum>(Enum);

            result.Should().Be(TestEnum.Three);
        }

        [Fact]
        public void GetRequiredParameter_WithWrongKey_ExpectKeyNotFoundException()
        {
            var result = Record.Exception(() => navigationParameter.GetRequiredParameter<TestEnum>("Invalid"));

            result.Should().BeOfType<KeyNotFoundException>();
        }

        [Fact]
        public void GetRequiredParameter_WithWrongType_ExpectKeyNotFoundException()
        {
            var result = Record.Exception(() => navigationParameter.GetRequiredParameter<TestEnum>(String));

            result.Should().BeOfType<InvalidCastException>();
        }

        private enum TestEnum
        {
            One,
            Two,
            Three
        }

        private class TestClass
        {
            public static TestClass GetClass() => new TestClass {MyProperty = 6, TestProperty = -5};
            public int MyProperty { get; set; }
            public double TestProperty { get; set; }
        }

        private struct TestStruct
        {
            public static TestStruct GetStruct() => new TestStruct { MyProperty = 6, TestProperty = -5 };
            public int MyProperty { get; set; }
            public double TestProperty { get; set; }
        }
    }
}
