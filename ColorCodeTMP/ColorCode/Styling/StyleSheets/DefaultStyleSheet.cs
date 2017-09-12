// Copyright (c) Microsoft Corporation.  All rights reserved.

using ColorCode.Common;

namespace ColorCode.Styling.StyleSheets
{
    public class DefaultStyleSheet : IStyleSheet
    {
        private static readonly StyleDictionary styles;

        static DefaultStyleSheet()
        {
            styles = new StyleDictionary
            {
                new Style(ScopeName.PlainText)
                {
                    Foreground = "#000000",
                    CssClassName = "plainText"
                },
                new Style(ScopeName.Comment)
                {
                    Foreground = "#565656",
                    CssClassName = "comment"
                },
                new Style(ScopeName.String)
                {
                    Foreground = "#6a8759",
                    CssClassName = "string"
                },
                new Style(ScopeName.StringCSharpVerbatim)
                {
                    Foreground = "#6a8759",
                    CssClassName = "stringCSharpVerbatim"
                },
                new Style(ScopeName.Keyword)
                {
                    Foreground = "#f4d03f",
                    CssClassName = "keyword"
                },
                new Style(ScopeName.PreprocessorKeyword)
                {
                    Foreground = "#f4d03f",
                    CssClassName = "preprocessorKeyword"
                },
                new Style(ScopeName.ClassName)
                {
                    Foreground = "#f4d03f",
                    CssClassName = "className"
                },
                new Style(ScopeName.Type)
                {
                    Foreground = "#e0c46c",
                    CssClassName = "type"
                },
                new Style(ScopeName.TypeVariable)
                {
                    Foreground = "#e0c46c",
                    Italic = true,
                    CssClassName = "typeVariable"
                },
                new Style(ScopeName.NameSpace)
                {
                    Foreground = "#f4d03f",
                    CssClassName = "namespace"
                },
                new Style(ScopeName.Constructor)
                {
                    Foreground = "#f4d03f",
                    CssClassName = "constructor"
                },
                new Style(ScopeName.Predefined)
                {
                    Foreground = "#f4d03f",
                    CssClassName = "predefined"
                },
                new Style(ScopeName.PseudoKeyword)
                {
                    Foreground = "#f4d03f",
                    CssClassName = "pseudoKeyword"
                },
                new Style(ScopeName.StringEscape)
                {
                    Foreground = "gray",
                    CssClassName = "stringEscape"
                },
                new Style(ScopeName.ControlKeyword)
                {
                    Foreground = "blue",
                    CssClassName = "controlKeyword"
                },
                new Style(ScopeName.Number)
                {
                    CssClassName = "number"
                },
                new Style(ScopeName.Operator)
                {
                    CssClassName = "operator"
                },
                new Style(ScopeName.Delimiter)
                {
                    CssClassName = "delimiter"
                },

                #region HTML

                new Style(ScopeName.HtmlServerSideScript)
                {
                    CssClassName = "htmlServerSideScript"
                },
                new Style(ScopeName.HtmlComment)
                {
                    Foreground = "green",
                    CssClassName = "htmlComment"
                },
                new Style(ScopeName.HtmlTagDelimiter)
                {
                    Foreground = "blue",
                    CssClassName = "htmlTagDelimiter"
                },
                new Style(ScopeName.HtmlElementName)
                {
                    Foreground = "red",
                    CssClassName = "htmlElementName"
                },
                new Style(ScopeName.HtmlAttributeName)
                {
                    Foreground = "red",
                    CssClassName = "htmlAttributeName"
                },
                new Style(ScopeName.HtmlAttributeValue)
                {
                    Foreground = "blue",
                    CssClassName = "htmlAttributeValue"
                },
                new Style(ScopeName.HtmlOperator)
                {
                    Foreground = "blue",
                    CssClassName = "htmlOperator"
                },
                new Style(ScopeName.HtmlEntity)
                {
                    Foreground = "red",
                    CssClassName = "htmlEntity"
                },

                #endregion

                #region XML

                new Style(ScopeName.XmlDocTag)
                {
                    Foreground = "gray",
                    CssClassName = "xmlDocTag"
                },
                new Style(ScopeName.XmlDocComment)
                {
                    Foreground = "green",
                    CssClassName = "xmlDocComment"
                },
                new Style(ScopeName.XmlAttribute)
                {
                    Foreground = "red",
                    CssClassName = "xmlAttribute"
                },
                new Style(ScopeName.XmlAttributeQuotes)
                {
                    Foreground = "black",
                    CssClassName = "xmlAttributeQuotes"
                },
                new Style(ScopeName.XmlAttributeValue)
                {
                    Foreground = "blue",
                    CssClassName = "xmlAttributeValue"
                },
                new Style(ScopeName.XmlCDataSection)
                {
                    Foreground = "gray",
                    CssClassName = "xmlCDataSection"
                },
                new Style(ScopeName.XmlComment)
                {
                    Foreground = "green",
                    CssClassName = "xmlComment"
                },
                new Style(ScopeName.XmlDelimiter)
                {
                    Foreground = "blue",
                    CssClassName = "xmlDelimiter"
                },
                new Style(ScopeName.XmlName)
                {
                    Foreground = "red",
                    CssClassName = "xmlName"
                },

                #endregion

                #region CSS / PowerShell / SQL

                new Style(ScopeName.CssSelector)
                {
                    Foreground = "red",
                    CssClassName = "cssSelector"
                },
                new Style(ScopeName.CssPropertyName)
                {
                    Foreground = "red",
                    CssClassName = "cssPropertyName"
                },
                new Style(ScopeName.CssPropertyValue)
                {
                    Foreground = "blue",
                    CssClassName = "cssPropertyValue"
                },
                new Style(ScopeName.SqlSystemFunction)
                {
                    Foreground = "magenta",
                    CssClassName = "sqlSystemFunction"
                },
                new Style(ScopeName.PowerShellAttribute)
                {
                    Foreground = "blue",
                    CssClassName = "powershellAttribute"
                },
                new Style(ScopeName.PowerShellOperator)
                {
                    Foreground = "gray",
                    CssClassName = "powershellOperator"
                },
                new Style(ScopeName.PowerShellType)
                {
                    Foreground = "teal",
                    CssClassName = "powershellType"
                },
                new Style(ScopeName.PowerShellVariable)
                {
                    Foreground = "orange",
                    CssClassName = "powershellVariable"
                },

                #endregion

                #region MARKDWON

                new Style(ScopeName.MarkdownHeader)
                {
                    // Foreground = "blue",
                    Bold = true,
                    CssClassName = "markdownHeader"
                },
                new Style(ScopeName.MarkdownCode)
                {
                    Foreground = "teal",
                    CssClassName = "markdownCode"
                },
                new Style(ScopeName.MarkdownListItem)
                {
                    Bold = true,
                    CssClassName = "markdownListItem"
                },
                new Style(ScopeName.MarkdownEmph)
                {
                    Italic = true,
                    CssClassName = "italic"
                },
                new Style(ScopeName.MarkdownBold)
                {
                    Bold = true,
                    CssClassName = "bold"
                },

                #endregion
            };
        }

        public string Name
        {
            get { return "DefaultStyleSheet"; }
        }

        public StyleDictionary Styles
        {
            get { return styles; }
        }
    }
}