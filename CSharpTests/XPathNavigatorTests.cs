using System.Xml;
using System.Xml.XPath;

namespace CSharpTests;

public class XPathNavigatorTests
{
    [Fact]
    public void DeepXPathWorks()
    {
        var xml = new XmlDocument().Do(xml =>
            xml.LoadXml("""
                        <root>
                            <child>
                                <id>1</id>
                            </child>
                            <child>
                                <id>2</id>
                                <attributes>
                                    <attribute>
                                        <barbaz>bar</barbaz>
                                    </attribute>
                                    <attribute>
                                        <origin>Foo</origin>
                                    </attribute>
                                    <attribute>
                                        <barbaz>adad</barbaz>
                                    </attribute>
                                    <attribute>
                                        <origin>Foo2</origin>
                                    </attribute>
                                </attributes>
                            </child>
                        </root>
                        """));
        var navigator = xml.CreateNavigator();
        var children = 
            navigator?.Select("/root/child").OfType<XPathNavigator>()
                .Select(child =>
                {
                    var id = child.SelectSingleNode("id")?.Value;
                    Assert.NotNull(id);
                    var origin =
                        child
                            .Select("attributes/attribute/origin").OfType<XPathNavigator>()
                            .Select(x => x.Value)
                            .FirstOrDefault();
                    return new {id, origin};
                })
                .ToList();
        Assert.NotNull(children);
        Assert.Collection(children,
            child =>
            {
                Assert.Equal("1", child.id);
                Assert.Null(child.origin);
            },
            child =>
            {
                Assert.Equal("2", child.id);
                Assert.Equal("Foo", child.origin);
            });
    }

    [Fact]
    public void NullsWorkButRequireTricks()
    {
        var xml = new XmlDocument().Do(xml =>
            xml.LoadXml("""
                        <attributes xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                            <attribute>
                                <barbaz>bar</barbaz>
                            </attribute>
                            <attribute>
                                <origin xsi:nil="true" />
                            </attribute>
                            <attribute>
                                <origin>Foo</origin>
                            </attribute>
                            <attribute>
                                <barbaz>adad</barbaz>
                            </attribute>
                            <attribute>
                                <origin>Foo2</origin>
                            </attribute>
                        </attributes>
                        """));
        var origin =
            xml.CreateNavigator()
                ?.Select("attributes/attribute/origin").OfType<XPathNavigator>()
                .Where(x => 
                    x.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance")
                    is not "true")
                .Select(x => x.Value)
                .FirstOrDefault();
        Assert.Equal("Foo", origin);
    }
    
    [Fact]
    public void SkippingEmptyStrings()
    {
        var xml = new XmlDocument().Do(xml =>
            xml.LoadXml("""
                        <attributes xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                            <attribute>
                                <barbaz>bar</barbaz>
                            </attribute>
                            <attribute>
                                <origin>   </origin>
                            </attribute>
                            <attribute>
                                <origin/>
                            </attribute>
                            <attribute>
                                <origin>Foo</origin>
                            </attribute>
                            <attribute>
                                <barbaz>adad</barbaz>
                            </attribute>
                            <attribute>
                                <origin>Foo2</origin>
                            </attribute>
                        </attributes>
                        """));
        var origin =
            xml.CreateNavigator()
                ?.Select("attributes/attribute/origin").OfType<XPathNavigator>()
                .Select(x => x.Value.Trim())
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
        Assert.Equal("Foo", origin);
    }
}