// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Text;
using ColorCode.Common;
using ColorCode.Compilation;
using ColorCode.Formatting;
using ColorCode.Parsing;
using ColorCode.Styling.StyleSheets;

namespace ColorCode
{
    /// <summary>
    /// Colorizes source code.
    /// </summary>
    public class CodeColorizer : ICodeColorizer
    {
        private readonly LanguageParser languageParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeColorizer"/> class.
        /// </summary>
        public CodeColorizer()
        {
            languageParser = new LanguageParser(new LanguageCompiler(Languages.CompiledLanguages),
                Languages.LanguageRepository);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeColorizer"/> class.
        /// </summary>
        /// <param name="languageParser">The language parser that the <see cref="CodeColorizer"/> instance will use for its lifetime.</param>
        public CodeColorizer(LanguageParser languageParser)
        {
            Guard.ArgNotNull(languageParser, "languageParser");

            this.languageParser = languageParser;
        }

        /// <summary>
        /// Colorizes source code using the specified language, the default formatter, and the default style sheet.
        /// </summary>
        /// <param name="sourceCode">The source code to colorize.</param>
        /// <param name="language">The language to use to colorize the source code.</param>
        /// <param name="styleSheet">The style sheet to use to colorize the source code. Null loads default style sheet.</param>
        /// <returns>The colorized source code.</returns>
        public string Colorize(string sourceCode, ILanguage language, IStyleSheet styleSheet)
        {
            var buffer = new StringBuilder(sourceCode.Length * 2);

            if (styleSheet == null)
                styleSheet = StyleSheets.Default;

            using (TextWriter writer = new StringWriter(buffer))
            {
                Colorize(sourceCode, language, Formatters.Default, styleSheet, writer);

                writer.Flush();
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Colorizes source code using the specified language, the default formatter, and the default style sheet.
        /// </summary>
        /// <param name="sourceCode">The source code to colorize.</param>
        /// <param name="language">The language to use to colorize the source code.</param>
        /// <param name="textWriter">The text writer to which the colorized source code will be written.</param>
        public void Colorize(string sourceCode, ILanguage language, TextWriter textWriter)
        {
            Colorize(sourceCode, language, Formatters.Default, StyleSheets.Default, textWriter);
        }

        /// <summary>
        /// Colorizes source code using the specified language, formatter, and style sheet.
        /// </summary>
        /// <param name="sourceCode">The source code to colorize.</param>
        /// <param name="language">The language to use to colorize the source code.</param>
        /// <param name="formatter">The formatter to use to colorize the source code.</param>
        /// <param name="styleSheet">The style sheet to use to colorize the source code.</param>
        /// <param name="textWriter">The text writer to which the colorized source code will be written.</param>
        public void Colorize(string sourceCode,
            ILanguage language,
            IFormatter formatter,
            IStyleSheet styleSheet,
            TextWriter textWriter)
        {
            Guard.ArgNotNull(language, "language");
            Guard.ArgNotNull(formatter, "formatter");
            Guard.ArgNotNull(styleSheet, "styleSheet");
            Guard.ArgNotNull(textWriter, "textWriter");

            //formatter.WriteHeader(styleSheet, language, textWriter);

            LanguageParser.StringScope[] stringScopeArray = languageParser.Parse(sourceCode, language,
                (parsedSourceCode, captures) => formatter.Write(parsedSourceCode, captures, styleSheet, textWriter));

            foreach (LanguageParser.StringScope stringScope in stringScopeArray)
            {
                Console.WriteLine(stringScope.scope.Name + stringScope.sourceCode);
            }
            
            //formatter.WriteFooter(styleSheet, language, textWriter);
        }

        public static void Main(string[] args)
        {
            new CodeColorizer().Colorize(@"public void refreshColors(Iterable<String> list)
{
	topColors=new HashMap<String, Color>();
	double x=.1;
	int i=0;
	for (String s: list)
	{
		topColors.put(s, i<handPalette.length ? handPalette[i] : palette(x));
		i++;
		x+=PHI;
	}
}",
                Languages.Java, new DefaultStyleSheet());
        }
    }
}