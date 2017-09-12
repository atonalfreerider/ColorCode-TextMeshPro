﻿// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using ColorCode.Common;
using ColorCode.Compilation;

namespace ColorCode.Parsing
{
    public class LanguageParser
    {
        private readonly ILanguageCompiler languageCompiler;
        private readonly ILanguageRepository languageRepository;

        public LanguageParser(ILanguageCompiler languageCompiler,
            ILanguageRepository languageRepository)
        {
            this.languageCompiler = languageCompiler;
            this.languageRepository = languageRepository;
        }

        public StringScope[] Parse(string sourceCode,
            ILanguage language,
            Action<string, IList<Scope>> parseHandler)
        {
            if (string.IsNullOrEmpty(sourceCode))
                return new StringScope[0];

            CompiledLanguage compiledLanguage = languageCompiler.Compile(language);

            //Parse(sourceCode, compiledLanguage, parseHandler);
            return Parse(sourceCode, compiledLanguage, parseHandler, 0);
        }

        private void Parse(string sourceCode,
            CompiledLanguage compiledLanguage,
            Action<string, IList<Scope>> parseHandler)
        {
            Match regexMatch = compiledLanguage.Regex.Match(sourceCode);

            if (!regexMatch.Success)
                parseHandler(sourceCode, new List<Scope>());
            else
            {
                int currentIndex = 0;

                while (regexMatch.Success)
                {
                    string sourceCodeBeforeMatch = sourceCode.Substring(currentIndex, regexMatch.Index - currentIndex);
                    if (!string.IsNullOrEmpty(sourceCodeBeforeMatch))
                        parseHandler(sourceCodeBeforeMatch, new List<Scope>());

                    string matchedSourceCode = sourceCode.Substring(regexMatch.Index, regexMatch.Length);
                    if (!string.IsNullOrEmpty(matchedSourceCode))
                    {
                        List<Scope> capturedStylesForMatchedFragment =
                            GetCapturedStyles(regexMatch, regexMatch.Index, compiledLanguage);
                        List<Scope> capturedStyleTree = CreateCapturedStyleTree(capturedStylesForMatchedFragment);
                        parseHandler(matchedSourceCode, capturedStyleTree);
                    }

                    currentIndex = regexMatch.Index + regexMatch.Length;
                    regexMatch = regexMatch.NextMatch();
                }

                string sourceCodeAfterAllMatches = sourceCode.Substring(currentIndex);
                if (!string.IsNullOrEmpty(sourceCodeAfterAllMatches))
                    parseHandler(sourceCodeAfterAllMatches, new List<Scope>());
            }
        }

        public struct StringScope
        {
            public string sourceCode;
            public Scope scope;

            public StringScope(string sourceCode, Scope scope)
            {
                this.sourceCode = sourceCode;
                this.scope = scope;
            }
        }

        private StringScope[] Parse(string sourceCode,
            CompiledLanguage compiledLanguage,
            Action<string, IList<Scope>> parseHandler,
            int meaningless
        )
        {
            Match regexMatch = compiledLanguage.Regex.Match(sourceCode);
            List<StringScope> list = new List<StringScope>();

            if (!regexMatch.Success)
                parseHandler(sourceCode, new List<Scope>());
            else
            {
                int currentIndex = 0;

                while (regexMatch.Success)
                {
                    string sourceCodeBeforeMatch = sourceCode.Substring(currentIndex, regexMatch.Index - currentIndex);
                    if (!string.IsNullOrEmpty(sourceCodeBeforeMatch))
                        parseHandler(sourceCodeBeforeMatch, new List<Scope>());
                    list.Add(new StringScope(sourceCodeBeforeMatch, new Scope("plainText", 0, 0)));

                    string matchedSourceCode = sourceCode.Substring(regexMatch.Index, regexMatch.Length);
                    if (!string.IsNullOrEmpty(matchedSourceCode))
                    {
                        List<Scope> capturedStylesForMatchedFragment =
                            GetCapturedStyles(regexMatch, regexMatch.Index, compiledLanguage);
                        List<Scope> capturedStyleTree = CreateCapturedStyleTree(capturedStylesForMatchedFragment);
                        list.Add(new StringScope(matchedSourceCode, new Scope(capturedStyleTree[0].Name, 0, 0)));

                        parseHandler(matchedSourceCode, capturedStyleTree);
                    }

                    currentIndex = regexMatch.Index + regexMatch.Length;
                    regexMatch = regexMatch.NextMatch();
                }

                string sourceCodeAfterAllMatches = sourceCode.Substring(currentIndex);
                list.Add(new StringScope(sourceCodeAfterAllMatches, new Scope("plainText", 0, 0)));
                if (!string.IsNullOrEmpty(sourceCodeAfterAllMatches))
                    parseHandler(sourceCodeAfterAllMatches, new List<Scope>());
            }
            return list.ToArray();
        }

        private static List<Scope> CreateCapturedStyleTree(IList<Scope> capturedStyles)
        {
            capturedStyles.SortStable((x, y) => x.Index.CompareTo(y.Index));

            var capturedStyleTree = new List<Scope>(capturedStyles.Count);
            Scope currentScope = null;

            foreach (Scope capturedStyle in capturedStyles)
            {
                if (currentScope == null)
                {
                    capturedStyleTree.Add(capturedStyle);
                    currentScope = capturedStyle;
                    continue;
                }

                AddScopeToNestedScopes(capturedStyle, ref currentScope, capturedStyleTree);
            }

            return capturedStyleTree;
        }

        private static void AddScopeToNestedScopes(Scope scope,
            ref Scope currentScope,
            ICollection<Scope> capturedStyleTree)
        {
            if (scope.Index >= currentScope.Index &&
                (scope.Index + scope.Length <= currentScope.Index + currentScope.Length))
            {
                currentScope.AddChild(scope);
                currentScope = scope;
            }
            else
            {
                currentScope = currentScope.Parent;

                if (currentScope != null)
                    AddScopeToNestedScopes(scope, ref currentScope, capturedStyleTree);
                else
                    capturedStyleTree.Add(scope);
            }
        }


        private List<Scope> GetCapturedStyles(Match regexMatch,
            int currentIndex,
            CompiledLanguage compiledLanguage)
        {
            var capturedStyles = new List<Scope>();

            for (int i = 0; i < regexMatch.Groups.Count; i++)
            {
                Group regexGroup = regexMatch.Groups[i];
                if (regexGroup.Length > 0 && i < compiledLanguage.Captures.Count)
                {
                    //note: i can be >= Captures.Count due to named groups; these do capture a group but always get added after all non-named groups (which is why we do not count them in numberOfCaptures)
                    string styleName = compiledLanguage.Captures[i];
                    if (!String.IsNullOrEmpty(styleName))
                    {
                        foreach (Capture regexCapture in regexGroup.Captures)
                            AppendCapturedStylesForRegexCapture(regexCapture, currentIndex, styleName, capturedStyles);
                    }
                }
            }

            return capturedStyles;
        }

        private void AppendCapturedStylesForRegexCapture(Capture regexCapture,
            int currentIndex,
            string styleName,
            ICollection<Scope> capturedStyles)
        {
            if (styleName.StartsWith(ScopeName.LanguagePrefix))
            {
                string nestedGrammarId = styleName.Substring(1);
                AppendCapturedStylesForNestedLanguage(regexCapture, regexCapture.Index - currentIndex, nestedGrammarId,
                    capturedStyles);
            }
            else
                capturedStyles.Add(new Scope(styleName, regexCapture.Index - currentIndex, regexCapture.Length));
        }

        private void AppendCapturedStylesForNestedLanguage(Capture regexCapture,
            int offset,
            string nestedLanguageId,
            ICollection<Scope> capturedStyles)
        {
            ILanguage nestedLanguage = languageRepository.FindById(nestedLanguageId);

            if (nestedLanguage == null)
                throw new InvalidOperationException("The nested language was not found in the language repository.");
            else
            {
                CompiledLanguage nestedCompiledLanguage = languageCompiler.Compile(nestedLanguage);

                Match regexMatch = nestedCompiledLanguage.Regex.Match(regexCapture.Value, 0, regexCapture.Value.Length);

                if (!regexMatch.Success)
                    return;
                else
                {
                    while (regexMatch.Success)
                    {
                        List<Scope> capturedStylesForMatchedFragment =
                            GetCapturedStyles(regexMatch, 0, nestedCompiledLanguage);
                        List<Scope> capturedStyleTree = CreateCapturedStyleTree(capturedStylesForMatchedFragment);

                        foreach (Scope nestedCapturedStyle in capturedStyleTree)
                        {
                            IncreaseCapturedStyleIndicies(capturedStyleTree, offset);
                            capturedStyles.Add(nestedCapturedStyle);
                        }

                        regexMatch = regexMatch.NextMatch();
                    }
                }
            }
        }

        private static void IncreaseCapturedStyleIndicies(IList<Scope> capturedStyles,
            int amountToIncrease)
        {
            for (int i = 0; i < capturedStyles.Count; i++)
            {
                Scope scope = capturedStyles[i];

                scope.Index += amountToIncrease;

                if (scope.Children.Count > 0)
                    IncreaseCapturedStyleIndicies(scope.Children, amountToIncrease);
            }
        }
    }
}