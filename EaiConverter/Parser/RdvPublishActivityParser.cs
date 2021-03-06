namespace EaiConverter.Parser
{
    using System.Xml.Linq;

    using EaiConverter.Model;
    using EaiConverter.Parser.Utils;

    public class RdvPublishActivityParser : IActivityParser
	{
        public Activity Parse(XElement inputElement)
        {
            var activity = new RdvPublishActivity();

            activity.Name = inputElement.Attribute("name").Value;
            activity.Type = (ActivityType) inputElement.Element (XmlnsConstant.tibcoProcessNameSpace + "type").Value;

            var configElement = inputElement.Element("config");

            activity.Subject = XElementParserUtils.GetStringValue(configElement.Element("subject"));
            activity.SharedChannel = XElementParserUtils.GetStringValue(configElement.Element("sharedChannel"));
            activity.isXmlEncode = XElementParserUtils.GetBoolValue(configElement.Element("xmlEncoding"));

            var xsdStringElement = configElement.Element("xsdString");
            if (xsdStringElement.Attribute("ref") != null)
            {
                activity.XsdString = xsdStringElement.Attribute("ref").ToString();
            }
            else
            {
                activity.ObjectXNodes = xsdStringElement.Nodes();
            }

            if (inputElement.Element(XmlnsConstant.tibcoProcessNameSpace + "inputBindings") != null)
            {
                activity.InputBindings = inputElement.Element(XmlnsConstant.tibcoProcessNameSpace + "inputBindings").Nodes();
                activity.Parameters = new XslParser().Parse(activity.InputBindings);
            }

            return activity;
        }
        
	}

}

