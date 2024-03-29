﻿using System;
using System.Collections.Generic;
using System.Linq;
using SharedKernel.Domain.Common;
using Xunit;

namespace SharedKernel.UnitTests.Domain.Common;

public class ValueObjectTests
{
    public ValueObjectTests()
    { }

    [Theory]
    [MemberData(nameof(EqualValueObjects))]
    public void equals_equal_value_objects_returns_true(ValueObject instanceA, ValueObject instanceB, string reason)
    {
        // Act
        var result = EqualityComparer<ValueObject>.Default.Equals(instanceA, instanceB);

        // Assert
        Assert.True(result, reason);
    }

    [Theory]
    [MemberData(nameof(NonEqualValueObjects))]
    public void equals_non_equal_value_objects_returns_false(ValueObject instanceA, ValueObject instanceB, string reason)
    {
        // Act
        var result = EqualityComparer<ValueObject>.Default.Equals(instanceA, instanceB);

        // Assert
        Assert.False(result, reason);
    }

    private static readonly ValueObject? NullValueObject = null;
    private static readonly ValueObject? APrettyValueObject = new ValueObjectA(1, "2", Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), new ComplexObject(2, "3"));

    public static readonly TheoryData<ValueObject?, ValueObject?, string> EqualValueObjects = new TheoryData<ValueObject?, ValueObject?, string>
    {
        {
            NullValueObject,
            NullValueObject,
            "they should be equal because they are both null"
        },
        {
            APrettyValueObject,
            APrettyValueObject,
            "they should be equal because they are the same object"
        },
        {
            new ValueObjectA(1, "2", Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), new ComplexObject(2, "3")),
            new ValueObjectA(1, "2", Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), new ComplexObject(2, "3")),
            "they should be equal because they have equal members"
        },
        {
            new ValueObjectA(1, "2", Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), new ComplexObject(2, "3"), notAnEqualityComponent: "xpto"),
            new ValueObjectA(1, "2", Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), new ComplexObject(2, "3"), notAnEqualityComponent: "xpto2"),
            "they should be equal because all equality components are equal, even though an additional member was set"
        },
        {
            new ValueObjectB(1, "2",  1, 2, 3 ),
            new ValueObjectB(1, "2",  1, 2, 3 ),
            "they should be equal because all equality components are equal, including the 'C' list"
        }
    };

    public static readonly TheoryData<ValueObject, ValueObject, string> NonEqualValueObjects = new TheoryData<ValueObject, ValueObject, string>
    {
        {
            new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(2, "3")),
            new ValueObjectA(a: 2, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(2, "3")),
            "they should not be equal because the 'A' member on ValueObjectA is different among them"
        },
        {
            new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(2, "3")),
            new ValueObjectA(a: 1, b: "", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(2, "3")),
            "they should not be equal because the 'B' member on ValueObjectA is different among them"
        },
        {
            new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(a: 2, b: "3")),
            new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(a: 3, b: "3")),
            "they should not be equal because the 'A' member on ValueObjectA's 'D' member is different among them"
        },
        {
            new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(a: 2, b: "3")),
            new ValueObjectB(a: 1, b: "2"),
            "they should not be equal because they are not of the same type"
        },
        {
            new ValueObjectB(1, "2",  1, 2, 3 ),
            new ValueObjectB(1, "2",  1, 2, 3, 4 ),
            "they should be not be equal because the 'C' list contains one additional value"
        },
        {
            new ValueObjectB(1, "2",  1, 2, 3, 5 ),
            new ValueObjectB(1, "2",  1, 2, 3 ),
            "they should be not be equal because the 'C' list contains one additional value"
        },
        {
            new ValueObjectB(1, "2",  1, 2, 3, 5 ),
            new ValueObjectB(1, "2",  1, 2, 3, 4 ),
            "they should be not be equal because the 'C' lists are not equal"
        }

    };

    private class ValueObjectA : ValueObject
    {
        public ValueObjectA(int a, string b, Guid c, ComplexObject d, string notAnEqualityComponent = "")
        {
            A = a;
            B = b;
            C = c;
            D = d;
            NotAnEqualityComponent = notAnEqualityComponent;
        }

        private int A { get; }
        private string B { get; }
        private Guid C { get; }
        private ComplexObject D { get; }
        private string NotAnEqualityComponent { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return A;
            yield return B;
            yield return C;
            yield return D;
        }
    }

    private class ValueObjectB : ValueObject
    {
        public ValueObjectB(int a, string b, params int[] c)
        {
            A = a;
            B = b;
            C = c.ToList();
        }

        private int A { get; }
        private string B { get; }

        private List<int> C { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return A;
            yield return B;

            foreach (var c in C)
            {
                yield return c;
            }
        }
    }

    private class ComplexObject : IEquatable<ComplexObject>
    {
        public ComplexObject(int a, string b)
        {
            A = a;
            B = b;
        }

        private int A { get; set; }

        private string B { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ComplexObject);
        }

        public bool Equals(ComplexObject? other)
        {
            return other != null &&
                    A == other.A &&
                    B == other.B;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(A, B);
        }
    }
}
