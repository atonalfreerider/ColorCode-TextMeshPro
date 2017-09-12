// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using ColorCode.Compilation;
using ColorCode.Lexing;

namespace ColorCode
{
    /// <summary>
    /// Lexes source code.
    /// </summary>
    public class CodeColorizer
    {
        private readonly LanguageLexer _languageLexer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeColorizer"/> class.
        /// </summary>
        public CodeColorizer()
        {
            _languageLexer = new LanguageLexer(new LanguageCompiler(Languages.CompiledLanguages),
                Languages.LanguageRepository);
        }

        /// <summary>
        /// Lex source code using the specified language.
        /// </summary>
        /// <param name="sourceCode">The source code to colorize.</param>
        /// <param name="language">The language to use to colorize the source code.</param>
        /// <returns>The lexed source code.</returns>
        public string[] Lex(string sourceCode, ILanguage language)
        {
            LanguageLexer.StringScope[] stringScopeArray = _languageLexer.Lex(sourceCode, language);
            string[] ret = new string[stringScopeArray.Length];
            int count = 0;
            foreach (LanguageLexer.StringScope stringScope in stringScopeArray)
            {
                ret[count] = stringScope.scope.Name + stringScope.sourceCode;
                Console.WriteLine(stringScope.scope.Name + stringScope.sourceCode);
                count++;
            }
            return ret;
        }

        public static void Main(string[] args)
        {
            new CodeColorizer().Lex(@"public void refreshColors(Iterable<String> list)
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
                Languages.Java);
        }
    }
}