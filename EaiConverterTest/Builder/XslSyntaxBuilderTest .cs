﻿using NUnit.Framework;

using System.Xml.Linq;
using EaiConverter.Builder;
using EaiConverter.Test.Utils;

namespace EaiConverter.Test.Builder
{
	[TestFixture]
	public class XslSyntaxBuilderTest 
	{
        XslSyntaxBuilder xslBuilder;
   

        [SetUp]
        public void SetUp ()
        {
			xslBuilder = new XslSyntaxBuilder (new XpathBuilder());

        }

		[Ignore]
        [Test]
        public void Should_Return_1_Variable_assignement_with_String_Type (){
            var xml =
                @"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"">
        <FundName>
            <xsl:value-of select=""'testvalue'""/>
        </FundName>
</pd:inputBindings>
";
            XElement doc = XElement.Parse(xml);

            var codeStatement = xslBuilder.Build(doc.Nodes());

            string generateCode = TestCodeGeneratorUtils.GenerateCode(codeStatement);
			Assert.AreEqual (@"System.String FundName = ""testvalue"";

", generateCode);
        }

		[Ignore]
        [Test]
        public void Should_Return_1_Variable_assignement_with_double_Type (){
            var xml =
                @"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"">
        <FundName>
            <xsl:value-of select=""number('testvalue')""/>
        </FundName>
</pd:inputBindings>
";
            XElement doc = XElement.Parse(xml);

            var codeStatement = xslBuilder.Build (doc.Nodes());

            string generateCode = TestCodeGeneratorUtils.GenerateCode(codeStatement);
            Assert.AreEqual (@"double FundName = TibcoXslHelper.ParseNumber(""testvalue"");

", generateCode);
        }

		[Ignore]
        [Test]
        public void Should_Return_1_Variable_assignement_with_DateTime_Type (){
            var xml =
                @"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"">
        <FundName>
            <xsl:value-of select=""tib:parse-dateTime('MMM dd yyyy', $Mystuff)""/>
        </FundName>
</pd:inputBindings>
";
            XElement doc = XElement.Parse(xml);

            var codeStatement = xslBuilder.Build (doc.Nodes());

            string generateCode = TestCodeGeneratorUtils.GenerateCode(codeStatement);
            Assert.AreEqual ("DateTime FundName = TibcoXslHelper.ParseDateTime(\"MMM dd yyyy\", Mystuff);\n\n", generateCode);

        }

		[Ignore]
        [Test]
        public void Should_Return_2_Variable_assignement_with_string_Type (){
            var xml =
                @"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"">
        <FundName>
            <xsl:value-of select=""'testvalue'""/>
        </FundName>
        <AdminID>
            <xsl:value-of select=""'EVL'""/>
        </AdminID>
</pd:inputBindings>
";
            XElement doc = XElement.Parse(xml);

            var generateCode = TestCodeGeneratorUtils.GenerateCode(xslBuilder.Build (doc.Nodes()));

			Assert.AreEqual (@"System.String FundName = ""testvalue"";
System.String AdminID = ""EVL"";

", generateCode);
        }

		[Ignore]
        [Test]
        public void Should_Return_1_Variable_assignement_with_1_Level_Children (){
            var xml =
                @"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"">
<sqlParams>        
<FundName>
            <xsl:value-of select=""'testvalue'""/>
        </FundName>
        <AdminID>
            <xsl:value-of select=""'EVL'""/>
        </AdminID>
</sqlParams>  
</pd:inputBindings>
";
            XElement doc = XElement.Parse(xml);

            var generateCode = TestCodeGeneratorUtils.GenerateCode(xslBuilder.Build (doc.Nodes()));

            Assert.AreEqual (@"sqlParams sqlParams = new sqlParams();
sqlParams.FundName = ""testvalue"";
sqlParams.AdminID = ""EVL"";

", generateCode.ToString());
        }

		[Ignore]
        [Test]
        public void Should_Return_1_Variable_assignement_with_2_Levels_Of_Children (){
            var xml =
                @"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"">
<sqlParams>  
    <FundCompany>      
        <FundName>
            <xsl:value-of select=""'testvalue'""/>
        </FundName>
    </FundCompany> 
        <AdminID>
            <xsl:value-of select=""'EVL'""/>
        </AdminID>
</sqlParams>  
</pd:inputBindings>
";
            XElement doc = XElement.Parse(xml);

            var generateCode = TestCodeGeneratorUtils.GenerateCode(xslBuilder.Build (doc.Nodes()));

            Assert.AreEqual (@"sqlParams sqlParams = new sqlParams();
sqlParams.FundCompany = new FundCompany();
sqlParams.FundCompany.FundName = ""testvalue"";
sqlParams.AdminID = ""EVL"";

", generateCode.ToString());
        }

		[Ignore]
        [Test]
        public void Should_manage_if_condition(){
            var xml =
                @"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"">
<xsl:if test=""true"">        
    <FundName>
            <xsl:value-of select=""'testvalue'""/>
        </FundName>
</xsl:if>
</pd:inputBindings>
";
            XElement doc = XElement.Parse(xml);

            var codeStatement = xslBuilder.Build (doc.Nodes());

            string generateCode = TestCodeGeneratorUtils.GenerateCode(codeStatement);
			Assert.AreEqual ("if (true)\n{\n    System.String FundName = \"testvalue\";\n}\n\n", generateCode);
        }

	
		[Ignore]
		[Test]
		public void Should_manage_choose_condition_Outside(){
			var xml =
				@"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"">
<FundName>
<xsl:choose>
    <xsl:when test=""true"">        
        
                <xsl:value-of select=""'testvalue1'""/>
    
    </xsl:when>
    <xsl:otherwise>        
       
                <xsl:value-of select=""'testvalue2'""/>
        
    </xsl:otherwise>
</xsl:choose>
</FundName>
</pd:inputBindings>
";
			XElement doc = XElement.Parse(xml);

			var codeStatement = xslBuilder.Build (doc.Nodes());

			string generateCode = TestCodeGeneratorUtils.GenerateCode(codeStatement);
			Assert.AreEqual ("System.String FundName;\nif (true)\n{\n    FundName = \"testvalue1\";\n}\nelse\n{\n    FundName = \"testvalue2\";\n}\n\n", generateCode);
		}

		[Ignore]
        [Test]
        public void Should_manage_Choose_condition(){
            var xml =
                @"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"">
<xsl:choose>
    <xsl:when test=""true"">        
        <FundName>
                <xsl:value-of select=""'testvalue1'""/>
            </FundName>
    </xsl:when>
    <xsl:otherwise>        
        <FundName>
                <xsl:value-of select=""'testvalue2'""/>
            </FundName>
    </xsl:otherwise>
</xsl:choose>
</pd:inputBindings>
";
            XElement doc = XElement.Parse(xml);

            var codeStatement = xslBuilder.Build (doc.Nodes());

            string generateCode = TestCodeGeneratorUtils.GenerateCode(codeStatement);
			Assert.AreEqual ("if (true)\n{\n    System.String FundName = \"testvalue1\";\n}\nelse\n{\n    System.String FundName = \"testvalue2\";\n}\n\n", generateCode);
        }

		[Ignore]
        [Test]
        public void Should_manage_foreach(){
            var xml =
                @"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"">
<FundCompany>
    <xsl:for-each select=""items"">       
            <FundName>
                    <xsl:value-of select=""'testvalue1'""/>
            </FundName>
    </xsl:for-each>
</FundCompany>
</pd:inputBindings>
";
            XElement doc = XElement.Parse(xml);

            var codeStatement = xslBuilder.Build (doc.Nodes());

            string generateCode = TestCodeGeneratorUtils.GenerateCode(codeStatement);
            Assert.AreEqual (
				"FundCompany FundCompany = new FundCompany();\nFundCompany.FundNames = new List<System.String>();\nforeach (var item in items)\n{\n    System.String FundName = \"testvalue1\";\n    FundCompany.FundNames.Add(FundName);\n}\n\n", generateCode);
        }



		[Ignore]
        [Test]
        public void Should_manage_Null_value_when_no_Parent(){
            var xml =
                @"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
                <FundName  xsi:nil=""true""/>
</pd:inputBindings>
";
            XElement doc = XElement.Parse(xml);
            var codeStatement = xslBuilder.Build (doc.Nodes());
            string generateCode = TestCodeGeneratorUtils.GenerateCode(codeStatement);
            Assert.AreEqual (
                @"object FundName = null;
", generateCode);
        }

		[Ignore]
        [Test]
        public void Should_Return_manage_null_value_when_parent_are_present (){
            var xml =
                @"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
<sqlParams>        
<FundName>
            <xsl:value-of select=""'testvalue'""/>
        </FundName>
        <AdminID xsi:nil=""true"">
        </AdminID>
</sqlParams>  
</pd:inputBindings>
";
            XElement doc = XElement.Parse(xml);

            var generateCode = TestCodeGeneratorUtils.GenerateCode(xslBuilder.Build (doc.Nodes()));

            Assert.AreEqual (@"sqlParams sqlParams = new sqlParams();
sqlParams.FundName = ""testvalue"";
sqlParams.AdminID = null;
", generateCode.ToString());
        }

		[Ignore]
        [Test]
        public void Should_Return_manage_List_value (){
            var xml =
                @"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"" >
    <sqlParams>        
        <param>
            <xsl:value-of select=""'testvalue1'""/>
        </param>
        <param >
            <xsl:value-of select=""'testvalue2'""/>
        </param>
    </sqlParams>  
</pd:inputBindings>
";
            XElement doc = XElement.Parse(xml);

            var generateCode = TestCodeGeneratorUtils.GenerateCode(xslBuilder.Build (doc.Nodes()));

            Assert.AreEqual (@"sqlParams sqlParams = new sqlParams();
sqlParams.param = new List<System.String>();
sqlParams.param.Add(""testvalue1"");
sqlParams.param.Add(""testvalue2"");

", generateCode.ToString());
        }

		[Ignore]
		[Test]
		public void Should_manage_add_package_name_When_type_is_complex_and_when_its_inputed()
		{
			var packageName = "MyPackage.";
			var xml =
				@"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"" >
    <sqlParams>        
        <param>
            <xsl:value-of select=""'testvalue1'""/>
        </param>
    </sqlParams>  
</pd:inputBindings>
";
			XElement doc = XElement.Parse(xml);

			var generateCode = TestCodeGeneratorUtils.GenerateCode(xslBuilder.Build(packageName, doc.Nodes()));

			Assert.AreEqual ("MyPackage.sqlParams sqlParams = new MyPackage.sqlParams();\nsqlParams.param = \"testvalue1\";\n\n", generateCode.ToString());
		}

		[Ignore]
		[Test]
		public void Should_manage_add_the_dot_At_the_end_Of_package_name_When_its_missing()
		{
			var packageName = "MyPackage";
			var xml =
				@"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"" >
    <sqlParams>        
        <param>
            <xsl:value-of select=""'testvalue1'""/>
        </param>
    </sqlParams>  
</pd:inputBindings>
";
			XElement doc = XElement.Parse(xml);

			var generateCode = TestCodeGeneratorUtils.GenerateCode(xslBuilder.Build(packageName, doc.Nodes()));

			Assert.AreEqual ("MyPackage.sqlParams sqlParams = new MyPackage.sqlParams();\nsqlParams.param = \"testvalue1\";\n\n", generateCode.ToString());
		}

		[Ignore]
		[Test]
		public void Should_manage_NOT_add_package_name_When_type_is_basic_and_when_its_inputed()
		{
			var packageName = "string.";
			var xml =
				@"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"" >
        
        <param>
            <xsl:value-of select=""'testvalue1'""/>
        </param>

</pd:inputBindings>
";
			XElement doc = XElement.Parse(xml);

			var generateCode = TestCodeGeneratorUtils.GenerateCode(xslBuilder.Build(packageName, doc.Nodes()));

			Assert.AreEqual ("System.String param = \"testvalue1\";\n\n", generateCode.ToString());
		}

		[Ignore]
		[Test]
		public void Should_Return_remove_prefix (){
			var xml =
				@"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"" xmlns:pfx1=""http://www.SomeWhere.com"">
 <pfx1:logInfo>       
<FundName>
            <xsl:value-of select=""'testvalue'""/>
        </FundName>
</pfx1:logInfo>
</pd:inputBindings>
";
			XElement doc = XElement.Parse(xml);

			var codeStatement = xslBuilder.Build (doc.Nodes());

			string generateCode = TestCodeGeneratorUtils.GenerateCode(codeStatement);
			Assert.AreEqual ("logInfo logInfo = new logInfo();\nlogInfo.FundName = \"testvalue\";\n\n", generateCode);
		}

		[Ignore]
        [Test]
		public void Should_Manage_xsl_attribute()
        {
			var xml =
 @"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"" xmlns:pfx1=""http://www.SomeWhere.com"">
<NTMMessage>
    <xsl:attribute name=""version"">
        <xsl:value-of select=""'1.03'""/>
    </xsl:attribute>
</NTMMessage>
</pd:inputBindings>
";
			XElement doc = XElement.Parse(xml);

			var codeStatement = this.xslBuilder.Build(doc.Nodes());

			string generateCode = TestCodeGeneratorUtils.GenerateCode(codeStatement);
            Assert.AreEqual("NTMMessage NTMMessage = new NTMMessage();\nNTMMessage.NTMMessageVersion = \"1.03\";\n\n", generateCode);
		}

		[Ignore]
        [Test]
        public void Should_Manage_xsl_attribute_null_value()
        {
            var xml =
 @"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"" xmlns:pfx1=""http://www.SomeWhere.com"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
<NTMMessage>
    <xsl:attribute name=""xsi:nil"">
        true
    </xsl:attribute>
</NTMMessage>
</pd:inputBindings>
";
            XElement doc = XElement.Parse(xml);

            var codeStatement = this.xslBuilder.Build(doc.Nodes());

            string generateCode = TestCodeGeneratorUtils.GenerateCode(codeStatement);
            Assert.AreEqual("NTMMessage NTMMessage = new NTMMessage();\nNTMMessage = null;\n\n", generateCode);
        }

		[Ignore]
        [Test]
        public void Should_manage_complex_type_embedded()
        {
            var xml =
                @"<pd:inputBindings xmlns:pd=""http://xmlns.tibco.com/bw/process/2003"" xmlns:xsl=""http://w3.org/1999/XSL/Transform"" xmlns:pfx1=""http://www.SomeWhere.com"">
 <logInfo>       
<FundStruct>
<FundName>
            <xsl:value-of select=""'testvalue'""/>
        </FundName>
        </FundStruct>
</logInfo>
</pd:inputBindings>
";
            XElement doc = XElement.Parse(xml);

            var codeStatement = xslBuilder.Build(doc.Nodes());

            string generateCode = TestCodeGeneratorUtils.GenerateCode(codeStatement);
            Assert.AreEqual("logInfo logInfo = new logInfo();\nlogInfo.FundStruct = new FundStruct();\nlogInfo.FundStruct.FundName = \"testvalue\";\n\n", generateCode);
        }
	}
}

