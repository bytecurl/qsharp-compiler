// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Quantum.QsCompiler.DataTypes;
using Microsoft.Quantum.QsCompiler.SyntaxTree;
using Microsoft.Quantum.QsCompiler.Transformations.Core;

namespace Microsoft.Quantum.QsCompiler.QIR
{
    internal class QirStatementTransformation : StatementTransformation<GenerationContext>
    {
        public QirStatementTransformation(SyntaxTreeTransformation<GenerationContext> parentTransformation)
            : base(parentTransformation)
        {
        }

        public QirStatementTransformation(GenerationContext sharedState)
            : base(sharedState)
        {
        }

        public QirStatementTransformation(SyntaxTreeTransformation<GenerationContext> parentTransformation, TransformationOptions options)
            : base(parentTransformation, options)
        {
        }

        public QirStatementTransformation(GenerationContext sharedState, TransformationOptions options)
            : base(sharedState, options)
        {
        }

        /* public overrides */

        public override QsStatement OnStatement(QsStatement stm)
        {
            QsNullable<QsLocation> loc = stm.Location;
            this.SharedState.DIManager.StatementLocationStack.Push(loc);
            this.SharedState.DIManager.EmitLocation(Position.Zero);
            QsStatement result = base.OnStatement(stm);
            this.SharedState.DIManager.StatementLocationStack.Pop();
            return result;
        }

        public override QsScope OnScope(QsScope scope)
        {
            return base.OnScope(scope); // TODO: keep track of location??
        }
    }
}