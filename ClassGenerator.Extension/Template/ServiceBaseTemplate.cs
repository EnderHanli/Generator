﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 16.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace ClassGenerator.Extension.Template
{
    using System.Linq;
    using System.Data;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class ServiceBaseTemplate : ServiceBaseTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("using System;\r\nusing System.Collections.Generic;\r\nusing Appendesk;\r\nusing ");
            
            #line 21 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoNamespace));
            
            #line default
            #line hidden
            this.Write(";\r\nusing ");
            
            #line 22 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalNamespace));
            
            #line default
            #line hidden
            this.Write(";\r\n\r\nnamespace ");
            
            #line 24 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\n    public partial class ");
            
            #line 26 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            this.Write(" : BusinessLayer\r\n    {\r\n        private readonly ");
            
            #line 28 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalClassName));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 28 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalParameterName));
            
            #line default
            #line hidden
            this.Write(";\r\n\r\n        public ");
            
            #line 30 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            this.Write("()\r\n        {\r\n            ");
            
            #line 32 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalParameterName));
            
            #line default
            #line hidden
            this.Write(" = new ");
            
            #line 32 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalClassName));
            
            #line default
            #line hidden
            this.Write("();\r\n        }\r\n\r\n        public ");
            
            #line 35 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            this.Write("(string connectionName)\r\n        {\r\n            ");
            
            #line 37 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalParameterName));
            
            #line default
            #line hidden
            this.Write(" = new ");
            
            #line 37 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalClassName));
            
            #line default
            #line hidden
            this.Write("(connectionName);\r\n        }\r\n\r\n        public ");
            
            #line 40 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            
            #line default
            #line hidden
            this.Write("(DataLayer dataLayer)\r\n        {\r\n            ");
            
            #line 42 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalParameterName));
            
            #line default
            #line hidden
            this.Write(" = new ");
            
            #line 42 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalClassName));
            
            #line default
            #line hidden
            this.Write("(dataLayer);\r\n        }\r\n\r\n        public List<");
            
            #line 45 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoClassName));
            
            #line default
            #line hidden
            this.Write("> GetByParameter(List<QueryParameter> queryParameters)\r\n        {\r\n            tr" +
                    "y\r\n            {\r\n                return ");
            
            #line 49 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalParameterName));
            
            #line default
            #line hidden
            this.Write(".GetByParameter(queryParameters);\r\n            }\r\n            catch (Exception)\r\n" +
                    "            {\r\n                throw;\r\n            }\r\n        }\r\n\r\n        publi" +
                    "c ");
            
            #line 57 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoClassName));
            
            #line default
            #line hidden
            this.Write(" Insert(");
            
            #line 57 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoClassName));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 57 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoParameterName));
            
            #line default
            #line hidden
            this.Write(")\r\n        {\r\n            try\r\n            {\r\n                ");
            
            #line 61 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalParameterName));
            
            #line default
            #line hidden
            this.Write(".TransactionBegin();\r\n                ");
            
            #line 62 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalParameterName));
            
            #line default
            #line hidden
            this.Write(".Insert(");
            
            #line 62 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoParameterName));
            
            #line default
            #line hidden
            this.Write(");\r\n\r\n                ");
            
            #line 64 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalParameterName));
            
            #line default
            #line hidden
            this.Write(".TransactionCommit();\r\n                ");
            
            #line 65 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoParameterName));
            
            #line default
            #line hidden
            this.Write(".Response = new Response(0, \"Kaydedildi\", ResponseLevel.Success);\r\n              " +
                    "  return ");
            
            #line 66 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoParameterName));
            
            #line default
            #line hidden
            this.Write(";\r\n            }\r\n            catch (Exception)\r\n            {\r\n                ");
            
            #line 70 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalParameterName));
            
            #line default
            #line hidden
            this.Write(".TransactionRollback();\r\n                throw;\r\n            }\r\n        }\r\n\r\n    " +
                    "    public ");
            
            #line 75 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoClassName));
            
            #line default
            #line hidden
            this.Write(" Update(");
            
            #line 75 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoClassName));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 75 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoParameterName));
            
            #line default
            #line hidden
            this.Write(")\r\n        {\r\n            try\r\n            {\r\n                ");
            
            #line 79 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalParameterName));
            
            #line default
            #line hidden
            this.Write(".TransactionBegin();\r\n                ");
            
            #line 80 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalParameterName));
            
            #line default
            #line hidden
            this.Write(".Update(");
            
            #line 80 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoParameterName));
            
            #line default
            #line hidden
            this.Write(");\r\n\r\n                ");
            
            #line 82 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalParameterName));
            
            #line default
            #line hidden
            this.Write(".TransactionCommit();\r\n                ");
            
            #line 83 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoParameterName));
            
            #line default
            #line hidden
            this.Write(".Response = new Response(0, \"Güncellendi\", ResponseLevel.Success);\r\n             " +
                    "   return ");
            
            #line 84 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoParameterName));
            
            #line default
            #line hidden
            this.Write(";\r\n            }\r\n            catch (Exception)\r\n            {\r\n                ");
            
            #line 88 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalParameterName));
            
            #line default
            #line hidden
            this.Write(".TransactionRollback();\r\n                throw;\r\n            }\r\n        }\r\n\r\n    " +
                    "    public ");
            
            #line 93 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoClassName));
            
            #line default
            #line hidden
            this.Write(" Delete(");
            
            #line 93 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoClassName));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 93 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoParameterName));
            
            #line default
            #line hidden
            this.Write(")\r\n        {\r\n            try\r\n            {\r\n                ");
            
            #line 97 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalParameterName));
            
            #line default
            #line hidden
            this.Write(".TransactionBegin();\r\n                ");
            
            #line 98 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalParameterName));
            
            #line default
            #line hidden
            this.Write(".Delete(");
            
            #line 98 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoParameterName));
            
            #line default
            #line hidden
            this.Write(");\r\n\r\n                ");
            
            #line 100 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalParameterName));
            
            #line default
            #line hidden
            this.Write(".TransactionCommit();\r\n                ");
            
            #line 101 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoParameterName));
            
            #line default
            #line hidden
            this.Write(".Response = new Response(0, \"Silindi\", ResponseLevel.Success);\r\n                r" +
                    "eturn ");
            
            #line 102 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DtoParameterName));
            
            #line default
            #line hidden
            this.Write(";\r\n            }\r\n            catch (Exception)\r\n            {\r\n                ");
            
            #line 106 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(DalParameterName));
            
            #line default
            #line hidden
            this.Write(".TransactionRollback();\r\n                throw;\r\n            }\r\n        }\r\n    }\r" +
                    "\n}");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "W:\Generator\ClassGenerator.Extension\Template\ServiceBaseTemplate.tt"

private string _NamespaceField;

/// <summary>
/// Access the Namespace parameter of the template.
/// </summary>
private string Namespace
{
    get
    {
        return this._NamespaceField;
    }
}

private string _DtoNamespaceField;

/// <summary>
/// Access the DtoNamespace parameter of the template.
/// </summary>
private string DtoNamespace
{
    get
    {
        return this._DtoNamespaceField;
    }
}

private string _DtoClassNameField;

/// <summary>
/// Access the DtoClassName parameter of the template.
/// </summary>
private string DtoClassName
{
    get
    {
        return this._DtoClassNameField;
    }
}

private string _DtoParameterNameField;

/// <summary>
/// Access the DtoParameterName parameter of the template.
/// </summary>
private string DtoParameterName
{
    get
    {
        return this._DtoParameterNameField;
    }
}

private string _DalNamespaceField;

/// <summary>
/// Access the DalNamespace parameter of the template.
/// </summary>
private string DalNamespace
{
    get
    {
        return this._DalNamespaceField;
    }
}

private string _DalClassNameField;

/// <summary>
/// Access the DalClassName parameter of the template.
/// </summary>
private string DalClassName
{
    get
    {
        return this._DalClassNameField;
    }
}

private string _DalParameterNameField;

/// <summary>
/// Access the DalParameterName parameter of the template.
/// </summary>
private string DalParameterName
{
    get
    {
        return this._DalParameterNameField;
    }
}

private string _SchemaNameField;

/// <summary>
/// Access the SchemaName parameter of the template.
/// </summary>
private string SchemaName
{
    get
    {
        return this._SchemaNameField;
    }
}

private string _ClassNameField;

/// <summary>
/// Access the ClassName parameter of the template.
/// </summary>
private string ClassName
{
    get
    {
        return this._ClassNameField;
    }
}

private global::System.Collections.Generic.List<Model.DbColumn> _DbColumnsField;

/// <summary>
/// Access the DbColumns parameter of the template.
/// </summary>
private global::System.Collections.Generic.List<Model.DbColumn> DbColumns
{
    get
    {
        return this._DbColumnsField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool NamespaceValueAcquired = false;
if (this.Session.ContainsKey("Namespace"))
{
    this._NamespaceField = ((string)(this.Session["Namespace"]));
    NamespaceValueAcquired = true;
}
if ((NamespaceValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("Namespace");
    if ((data != null))
    {
        this._NamespaceField = ((string)(data));
    }
}
bool DtoNamespaceValueAcquired = false;
if (this.Session.ContainsKey("DtoNamespace"))
{
    this._DtoNamespaceField = ((string)(this.Session["DtoNamespace"]));
    DtoNamespaceValueAcquired = true;
}
if ((DtoNamespaceValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("DtoNamespace");
    if ((data != null))
    {
        this._DtoNamespaceField = ((string)(data));
    }
}
bool DtoClassNameValueAcquired = false;
if (this.Session.ContainsKey("DtoClassName"))
{
    this._DtoClassNameField = ((string)(this.Session["DtoClassName"]));
    DtoClassNameValueAcquired = true;
}
if ((DtoClassNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("DtoClassName");
    if ((data != null))
    {
        this._DtoClassNameField = ((string)(data));
    }
}
bool DtoParameterNameValueAcquired = false;
if (this.Session.ContainsKey("DtoParameterName"))
{
    this._DtoParameterNameField = ((string)(this.Session["DtoParameterName"]));
    DtoParameterNameValueAcquired = true;
}
if ((DtoParameterNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("DtoParameterName");
    if ((data != null))
    {
        this._DtoParameterNameField = ((string)(data));
    }
}
bool DalNamespaceValueAcquired = false;
if (this.Session.ContainsKey("DalNamespace"))
{
    this._DalNamespaceField = ((string)(this.Session["DalNamespace"]));
    DalNamespaceValueAcquired = true;
}
if ((DalNamespaceValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("DalNamespace");
    if ((data != null))
    {
        this._DalNamespaceField = ((string)(data));
    }
}
bool DalClassNameValueAcquired = false;
if (this.Session.ContainsKey("DalClassName"))
{
    this._DalClassNameField = ((string)(this.Session["DalClassName"]));
    DalClassNameValueAcquired = true;
}
if ((DalClassNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("DalClassName");
    if ((data != null))
    {
        this._DalClassNameField = ((string)(data));
    }
}
bool DalParameterNameValueAcquired = false;
if (this.Session.ContainsKey("DalParameterName"))
{
    this._DalParameterNameField = ((string)(this.Session["DalParameterName"]));
    DalParameterNameValueAcquired = true;
}
if ((DalParameterNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("DalParameterName");
    if ((data != null))
    {
        this._DalParameterNameField = ((string)(data));
    }
}
bool SchemaNameValueAcquired = false;
if (this.Session.ContainsKey("SchemaName"))
{
    this._SchemaNameField = ((string)(this.Session["SchemaName"]));
    SchemaNameValueAcquired = true;
}
if ((SchemaNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("SchemaName");
    if ((data != null))
    {
        this._SchemaNameField = ((string)(data));
    }
}
bool ClassNameValueAcquired = false;
if (this.Session.ContainsKey("ClassName"))
{
    this._ClassNameField = ((string)(this.Session["ClassName"]));
    ClassNameValueAcquired = true;
}
if ((ClassNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("ClassName");
    if ((data != null))
    {
        this._ClassNameField = ((string)(data));
    }
}
bool DbColumnsValueAcquired = false;
if (this.Session.ContainsKey("DbColumns"))
{
    this._DbColumnsField = ((global::System.Collections.Generic.List<Model.DbColumn>)(this.Session["DbColumns"]));
    DbColumnsValueAcquired = true;
}
if ((DbColumnsValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("DbColumns");
    if ((data != null))
    {
        this._DbColumnsField = ((global::System.Collections.Generic.List<Model.DbColumn>)(data));
    }
}


    }
}


        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public class ServiceBaseTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
