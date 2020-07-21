﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Quantum.QsCompiler.DataTypes;
using Microsoft.Quantum.QsCompiler.Diagnostics;
using Microsoft.Quantum.QsCompiler.SyntaxTree;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Lsp = Microsoft.VisualStudio.LanguageServer.Protocol;
using Position = Microsoft.Quantum.QsCompiler.DataTypes.Position;
using Range = Microsoft.Quantum.QsCompiler.DataTypes.Range;

namespace Microsoft.Quantum.QsCompiler.CompilationBuilder
{
    public static class DiagnosticTools
    {
        /// <summary>
        /// Converts the language server protocol position into a Q# compiler position.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="position"/> is null.</exception>
        public static Position ToQSharp(this Lsp.Position position) =>
            position is null
                ? throw new ArgumentNullException(nameof(position))
                : Position.Create(position.Line, position.Character);

        /// <summary>
        /// Converts the Q# compiler position into a language server protocol position.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="position"/> is null.</exception>
        public static Lsp.Position ToLsp(this Position position) =>
            position is null
                ? throw new ArgumentNullException(nameof(position))
                : new Lsp.Position(position.Line, position.Column);

        /// <summary>
        /// Adds the Q# position to the language server protocol position, returning a new language server protocol
        /// position.
        /// </summary>
        /// <param name="position1">
        /// The language server protocol position. Null is equivalent to the zero position.
        /// </param>
        /// <param name="position2">The Q# position.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="position1"/> is invalid.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="position2"/> is null.</exception>
        internal static Lsp.Position GetAbsolutePosition(Lsp.Position position1, Position position2)
        {
            // TODO: This should be replaced with `position1 + position2` once both are Q# positions.
            if (!Utils.IsValidPosition(position1))
            {
                throw new ArgumentException(nameof(position1));
            }
            if (position2 is null)
            {
                throw new ArgumentNullException(nameof(position2));
            }
            var absPos = position1?.Copy() ?? new Lsp.Position();
            absPos.Line += position2.Line;
            absPos.Character =
                position2.Line > 0
                    ? position2.Column
                    : absPos.Character + position2.Column;
            return absPos;
        }

        /// <summary>
        /// Adds the language server protocol position to the Q# compiler range.
        /// </summary>
        /// <param name="position">The language server protocol position.</param>
        /// <param name="range">The Q# compiler range.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="range"/> is null.</exception>
        internal static Lsp.Range GetAbsoluteRange(Lsp.Position position, Range range)
        {
            // TODO: This should be replaced with `position + range` once the LSP position and returned range are Q#
            // compiler types.
            if (range == null)
            {
                throw new ArgumentNullException(nameof(range));
            }
            return new Lsp.Range
            {
                Start = GetAbsolutePosition(position, range.Start),
                End = GetAbsolutePosition(position, range.End)
            };
        }

        /// <summary>
        /// Given the location information for a declared symbol,
        /// as well as the position of the declaration within which the symbol is declared,
        /// returns the zero-based line and character index indicating the position of the symbol in the file.
        /// Returns null if the given object is not compatible with the position information generated by this CompilationBuilder.
        /// </summary>
        public static Position SymbolPosition(QsLocation rootLocation, QsNullable<Position> symbolPosition, Range symbolRange)
        {
            // the position offset is set to null (only) for variables defined in the declaration
            var offset = symbolPosition.IsNull ? rootLocation.Offset : rootLocation.Offset + symbolPosition.Item;
            return offset + symbolRange.Start;
        }

        /// <summary>
        /// Returns a new Position with the line number and character of the given Position
        /// or null in case the given Position is null.
        /// </summary>
        public static Lsp.Position Copy(this Lsp.Position pos)
        {
            return pos == null
                ? null
                : new Lsp.Position(pos.Line, pos.Character);
        }

        /// <summary>
        /// Verifies the given Position, and returns a *new* Position with updated line number.
        /// Throws an ArgumentNullException if the given Position is null.
        /// Throws an ArgumentException if the given Position is invalid.
        /// Throws and ArgumentOutOfRangeException if the updated line number is negative.
        /// </summary>
        public static Lsp.Position WithUpdatedLineNumber(this Lsp.Position pos, int lineNrChange)
        {
            if (!Utils.IsValidPosition(pos))
            {
                throw new ArgumentException($"invalid Position in {nameof(WithUpdatedLineNumber)}");
            }
            if (pos.Line + lineNrChange < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lineNrChange));
            }
            var updated = pos.Copy();
            updated.Line += lineNrChange;
            return updated;
        }

        /// <summary>
        /// For a given Range, returns a new Range with its starting and ending position a copy of the start and end of the given Range
        /// (i.e. does a deep copy) or null in case the given Range is null.
        /// </summary>
        public static Lsp.Range Copy(this Lsp.Range r)
        {
            return r == null
                ? null
                : new Lsp.Range { Start = r.Start.Copy(), End = r.End.Copy() };
        }

        /// <summary>
        /// Verifies the given Range, and returns a *new* Range with updated line numbers.
        /// Throws an ArgumentNullException if the given Range is null.
        /// Throws an ArgumentException if the given Range is invalid.
        /// Throws and ArgumentOutOfRangeException if the updated line number is negative.
        /// </summary>
        public static Lsp.Range WithUpdatedLineNumber(this Lsp.Range range, int lineNrChange)
        {
            if (lineNrChange == 0)
            {
                return range ?? throw new ArgumentNullException(nameof(range));
            }
            if (!Utils.IsValidRange(range))
            {
                throw new ArgumentException($"invalid Range in {nameof(WithUpdatedLineNumber)}"); // range can be empty
            }
            if (range.Start.Line + lineNrChange < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lineNrChange));
            }
            var updated = range.Copy();
            updated.Start.Line += lineNrChange;
            updated.End.Line += lineNrChange;
            return updated;
        }

        /// <summary>
        /// Returns a new Diagnostic, making a deep copy of the given one (in particular a deep copy of it's Range)
        /// or null if the given Diagnostic is null.
        /// </summary>
        public static Diagnostic Copy(this Diagnostic message)
        {
            if (message == null)
            {
                return null;
            }
            return new Diagnostic()
            {
                Range = message.Range.Copy(),
                Severity = message.Severity,
                Code = message.Code,
                Source = message.Source,
                Message = message.Message
            };
        }

        /// <summary>
        /// For a given Diagnostic, verifies its range and returns a *new* Diagnostic with updated line numbers.
        /// Throws an ArgumentNullException if the given Diagnostic is null.
        /// Throws an ArgumentException if the Range of the given Diagnostic is invalid.
        /// Throws and ArgumentOutOfRangeException if the updated line number is negative.
        /// </summary>
        public static Diagnostic WithUpdatedLineNumber(this Diagnostic diagnostic, int lineNrChange)
        {
            if (lineNrChange == 0)
            {
                return diagnostic ?? throw new ArgumentNullException(nameof(diagnostic));
            }
            var updatedRange = diagnostic.Range.WithUpdatedLineNumber(lineNrChange); // throws if the given diagnostic is null
            var updated = diagnostic.Copy();
            updated.Range = updatedRange;
            return updated;
        }

        /// <summary>
        /// Returns a function that returns true if the ErrorType of the given Diagnostic is one of the given types.
        /// </summary>
        public static Func<Diagnostic, bool> ErrorType(params ErrorCode[] types)
        {
            var codes = types.Select(err => err.Code());
            return m => m.IsError() && codes.Contains(m.Code);
        }

        /// <summary>
        /// Returns a function that returns true if the WarningType of the given Diagnostic is one of the given types.
        /// </summary>
        public static Func<Diagnostic, bool> WarningType(params WarningCode[] types)
        {
            var codes = types.Select(warn => warn.Code());
            return m => m.IsWarning() && codes.Contains(m.Code);
        }

        /// <summary>
        /// Returns true if the given diagnostics is an error.
        /// </summary>
        public static bool IsError(this Diagnostic m) =>
            m.Severity == DiagnosticSeverity.Error;

        /// <summary>
        /// Returns true if the given diagnostics is a warning.
        /// </summary>
        public static bool IsWarning(this Diagnostic m) =>
            m.Severity == DiagnosticSeverity.Warning;

        /// <summary>
        /// Returns true if the given diagnostics is an information.
        /// </summary>
        public static bool IsInformation(this Diagnostic m) =>
            m.Severity == DiagnosticSeverity.Information;

        /// <summary>
        /// Extracts all elements satisfying the given condition and which start at a line that is larger or equal to lowerBound.
        /// Diagnostics without any range information are only extracted if no lower bound is specified or the specified lower bound is smaller than zero.
        /// Throws an ArgumentNullException if the given condition is null.
        /// </summary>
        public static IEnumerable<Diagnostic> Filter(this IEnumerable<Diagnostic> orig, Func<Diagnostic, bool> condition, int lowerBound = -1)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }
            return orig?.Where(m => condition(m) && lowerBound <= (m.Range?.Start?.Line ?? -1));
        }

        /// <summary>
        /// Extracts all elements satisfying the given condition and which start at a line that is larger or equal to lowerBound and smaller than upperBound.
        /// Throws an ArgumentNullException if the given condition is null.
        /// </summary>
        public static IEnumerable<Diagnostic> Filter(this IEnumerable<Diagnostic> orig, Func<Diagnostic, bool> condition, int lowerBound, int upperBound)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }
            return orig?.Where(m => condition(m) && lowerBound <= m.Range.Start.Line && m.Range.End.Line < upperBound);
        }

        /// <summary>
        /// Extracts all elements which start at a line that is larger or equal to lowerBound.
        /// </summary>
        public static IEnumerable<Diagnostic> Filter(this IEnumerable<Diagnostic> orig, int lowerBound)
        {
            return orig?.Filter(m => true, lowerBound);
        }

        /// <summary>
        /// Extracts all elements which start at a line that is larger or equal to lowerBound and smaller than upperBound.
        /// </summary>
        public static IEnumerable<Diagnostic> Filter(this IEnumerable<Diagnostic> orig, int lowerBound, int upperBound)
        {
            return orig?.Filter(m => true, lowerBound, upperBound);
        }

        /// <summary>
        /// Returns true if the start line of the given diagnostic is larger or equal to lowerBound.
        /// </summary>
        internal static bool SelectByStartLine(this Diagnostic m, int lowerBound)
        {
            return m?.Range?.Start?.Line == null ? false : lowerBound <= m.Range.Start.Line;
        }

        /// <summary>
        /// Returns true if the start line of the given diagnostic is larger or equal to lowerBound, and smaller than upperBound.
        /// </summary>
        internal static bool SelectByStartLine(this Diagnostic m, int lowerBound, int upperBound)
        {
            return m?.Range?.Start?.Line == null ? false : lowerBound <= m.Range.Start.Line && m.Range.Start.Line < upperBound;
        }

        /// <summary>
        /// Returns true if the end line of the given diagnostic is larger or equal to lowerBound, and smaller than upperBound.
        /// </summary>
        internal static bool SelectByEndLine(this Diagnostic m, int lowerBound, int upperBound)
        {
            return m?.Range?.End?.Line == null ? false : lowerBound <= m.Range.End.Line && m.Range.End.Line < upperBound;
        }

        /// <summary>
        /// Returns true if the start position of the given diagnostic is larger or equal to lowerBound.
        /// </summary>
        internal static bool SelectByStart(this Diagnostic m, Lsp.Position lowerBound)
        {
            return m?.Range?.Start?.Line == null ? false : lowerBound.IsSmallerThanOrEqualTo(m.Range.Start);
        }

        /// <summary>
        /// Returns true if the start position of the given diagnostic is larger or equal to lowerBound, and smaller than upperBound.
        /// </summary>
        internal static bool SelectByStart(this Diagnostic m, Lsp.Position lowerBound, Lsp.Position upperBound)
        {
            return m?.Range?.Start?.Line == null ? false : lowerBound.IsSmallerThanOrEqualTo(m.Range.Start) && m.Range.Start.IsSmallerThan(upperBound);
        }

        /// <summary>
        /// Returns true if the end position of the given diagnostic is larger or equal to lowerBound.
        /// </summary>
        internal static bool SelectByEnd(this Diagnostic m, Lsp.Position lowerBound)
        {
            return m?.Range?.End?.Line == null ? false : lowerBound.IsSmallerThanOrEqualTo(m.Range.End);
        }

        /// <summary>
        /// Returns true if the end position of the given diagnostic is larger or equal to lowerBound, and smaller than upperBound.
        /// </summary>
        internal static bool SelectByEnd(this Diagnostic m, Lsp.Position lowerBound, Lsp.Position upperBound)
        {
            return m?.Range?.End?.Line == null ? false : lowerBound.IsSmallerThanOrEqualTo(m.Range.End) && m.Range.End.IsSmallerThan(upperBound);
        }
    }
}
