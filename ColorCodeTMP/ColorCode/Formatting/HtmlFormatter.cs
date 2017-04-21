// Copyright (c) Microsoft Corporation.  All rights reserved.

using System.Collections.Generic;
using System.IO;
using ColorCode.Common;
using ColorCode.Parsing;

namespace ColorCode.Formatting
{
    public class HtmlFormatter : IFormatter
    {
        public void Write(string parsedSourceCode,
                          IList<Scope> scopes,
                          IStyleSheet styleSheet,
                          TextWriter textWriter)
        {
            var styleInsertions = new List<TextInsertion>();

            foreach (Scope scope in scopes)
                GetStyleInsertionsForCapturedStyle(scope, styleInsertions);

            styleInsertions.SortStable((x, y) => x.Index.CompareTo(y.Index));

            int offset = 0;


            foreach (TextInsertion styleInsertion in styleInsertions)
            {
                textWriter.Write(parsedSourceCode.Substring(offset, styleInsertion.Index - offset));
                if (string.IsNullOrEmpty(styleInsertion.Text))
                    BuildSpanForCapturedStyle(styleInsertion.Scope, styleSheet, textWriter);
                else
                    textWriter.Write(styleInsertion.Text);
                offset = styleInsertion.Index;
            }

            textWriter.Write(parsedSourceCode.Substring(offset));

        }

        public void WriteFooter(IStyleSheet styleSheet,
                                ILanguage language,
                                TextWriter textWriter)
        {
            Guard.ArgNotNull(styleSheet, "styleSheet");
            Guard.ArgNotNull(textWriter, "textWriter");

            textWriter.WriteLine();
            WriteHeaderPreEnd(textWriter);
            WriteHeaderDivEnd(textWriter);
        }

        public void WriteHeader(IStyleSheet styleSheet,
                                ILanguage language,
                                TextWriter textWriter)
        {
            Guard.ArgNotNull(styleSheet, "styleSheet");
            Guard.ArgNotNull(textWriter, "textWriter");

            WriteHeaderDivStart(styleSheet, textWriter);
            WriteHeaderPreStart(textWriter);
            textWriter.WriteLine();
        }

        private static void GetStyleInsertionsForCapturedStyle(Scope scope, ICollection<TextInsertion> styleInsertions)
        {
            styleInsertions.Add(new TextInsertion
            {
                Index = scope.Index,
                Scope = scope
            });


            foreach (Scope childScope in scope.Children)
                GetStyleInsertionsForCapturedStyle(childScope, styleInsertions);

            styleInsertions.Add(new TextInsertion
            {
                Index = scope.Index + scope.Length,
                Text = "</color>"
            });
        }

        private static void BuildSpanForCapturedStyle(Scope scope,
                                                        IStyleSheet styleSheet,
                                                        TextWriter writer)
        {
            string foreground = "";
            string background = "";
            bool italic = false;
            bool bold = false;

            if (styleSheet.Styles.Contains(scope.Name))
            {
                Style style = styleSheet.Styles[scope.Name];

                foreground = style.Foreground;
                background = style.Background;
                italic = style.Italic;
                bold = style.Bold;
            }

            WriteElementStart(writer, "", foreground, background, italic, bold);
        }

        private static void WriteHeaderDivEnd(TextWriter writer)
        {
            WriteElementEnd("", writer);
        }

        private static void WriteElementEnd(string elementName,
                                            TextWriter writer)
        {
            writer.Write("{0}", elementName);
        }

        private static void WriteHeaderPreEnd(TextWriter writer)
        {
            WriteElementEnd("", writer);
        }

        private static void WriteHeaderPreStart(TextWriter writer)
        {
            WriteElementStart(writer, "");
        }

        private static void WriteHeaderDivStart(IStyleSheet styleSheet,
                                                TextWriter writer)
        {
            string foreground = "";
            string background = "";

            if (styleSheet.Styles.Contains(ScopeName.PlainText))
            {
                Style plainTextStyle = styleSheet.Styles[ScopeName.PlainText];

                foreground = plainTextStyle.Foreground;
                background = plainTextStyle.Background;
            }

            WriteElementStart(writer, "", foreground, background);
        }

        private static void WriteElementStart(TextWriter writer,
                                              string elementName)
        {
            WriteElementStart(writer, elementName, "", "");
        }

        private static void WriteElementStart(TextWriter writer,
                                              string elementName,
                                              string foreground,
                                              string background,
                                              bool italic = false,
                                              bool bold = false
                                              )
        {
            writer.Write("{0}", elementName);

            if (foreground != "")
                writer.Write("<color={0}>", foreground);

        }
    }
}