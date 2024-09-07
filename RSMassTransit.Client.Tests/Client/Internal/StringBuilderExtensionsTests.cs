// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

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
            .ToString().Should().BeEmpty();
    }

    [Test]
    public void AppendDelimitedList_One()
    {
        new StringBuilder()
            .AppendDelimitedList(["a"])
            .ToString().Should().Be("'a'");
    }

    [Test]
    public void AppendDelimitedList_Two()
    {
        new StringBuilder()
            .AppendDelimitedList(["a", "b"])
            .ToString().Should().Be("'a', 'b'");
    }

    [Test]
    public void AppendDelimitedList_WithSeparator()
    {
        new StringBuilder()
            .AppendDelimitedList(["a", "b", "c"], separator: "|")
            .ToString().Should().Be("'a'|'b'|'c'");
    }

    [Test]
    public void AppendDelimitedList_NullSeparator()
    {
        new StringBuilder()
            .AppendDelimitedList(["a", "b", "c"], separator: null)
            .ToString().Should().Be("'a''b''c'");
    }

    [Test]
    public void AppendDelimitedList_WithDelimiter()
    {
        new StringBuilder()
            .AppendDelimitedList(["a", "b", "c"], delimiter: "`")
            .ToString().Should().Be("`a`, `b`, `c`");
    }

    [Test]
    public void AppendDelimitedList_NullDelimiter()
    {
        new StringBuilder()
            .AppendDelimitedList(["a", "b", "c"], delimiter: null)
            .ToString().Should().Be("a, b, c");
    }
}
