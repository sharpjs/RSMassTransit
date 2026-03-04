// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

using System.Text;

namespace RSMassTransit.Client.Internal;

[TestFixture]
public class StringBuilderExtensionsTests
{
    [Test]
    public void AppendDelimitedList_Null()
    {
        new StringBuilder()
            .AppendDelimitedList(null)
            .ToString().ShouldBeEmpty();
    }

    [Test]
    public void AppendDelimitedList_One()
    {
        new StringBuilder()
            .AppendDelimitedList(["a"])
            .ToString().ShouldBe("'a'");
    }

    [Test]
    public void AppendDelimitedList_Two()
    {
        new StringBuilder()
            .AppendDelimitedList(["a", "b"])
            .ToString().ShouldBe("'a', 'b'");
    }

    [Test]
    public void AppendDelimitedList_WithSeparator()
    {
        new StringBuilder()
            .AppendDelimitedList(["a", "b", "c"], separator: "|")
            .ToString().ShouldBe("'a'|'b'|'c'");
    }

    [Test]
    public void AppendDelimitedList_NullSeparator()
    {
        new StringBuilder()
            .AppendDelimitedList(["a", "b", "c"], separator: null)
            .ToString().ShouldBe("'a''b''c'");
    }

    [Test]
    public void AppendDelimitedList_WithDelimiter()
    {
        new StringBuilder()
            .AppendDelimitedList(["a", "b", "c"], delimiter: "`")
            .ToString().ShouldBe("`a`, `b`, `c`");
    }

    [Test]
    public void AppendDelimitedList_NullDelimiter()
    {
        new StringBuilder()
            .AppendDelimitedList(["a", "b", "c"], delimiter: null)
            .ToString().ShouldBe("a, b, c");
    }
}
