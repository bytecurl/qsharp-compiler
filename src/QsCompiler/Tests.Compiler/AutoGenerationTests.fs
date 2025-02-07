﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Quantum.QsCompiler.Testing

open System.Collections.Generic
open Microsoft.Quantum.QsCompiler.DataTypes
open Microsoft.Quantum.QsCompiler.Diagnostics
open Microsoft.Quantum.QsCompiler.SyntaxExtensions
open Microsoft.Quantum.QsCompiler.SyntaxTree
open Xunit


type FunctorAutoGenTests() =
    inherit CompilerTests(CompilerTests.Compile("TestCases", [ "General.qs"; "FunctorGeneration.qs" ]))

    member private this.Expect name (diag: IEnumerable<DiagnosticItem>) =
        let ns = "Microsoft.Quantum.Testing.FunctorGeneration"
        this.VerifyDiagnostics(QsQualifiedName.New(ns, name), diag)


    [<Fact>]
    member this.``Operation characteristics``() =

        this.Expect "AutoAdjSpec" [ Warning WarningCode.MissingBodyDeclaration ]
        this.Expect "InvertAdjSpec" [ Warning WarningCode.MissingBodyDeclaration ]
        this.Expect "SelfAdjSpec" [ Warning WarningCode.MissingBodyDeclaration ]
        this.Expect "AutoCtlSpec" [ Warning WarningCode.MissingBodyDeclaration ]
        this.Expect "DistrCtlSpec" [ Warning WarningCode.MissingBodyDeclaration ]
        this.Expect "CtlAffAutoAdjSpec" [ Warning WarningCode.MissingBodyDeclaration ]
        this.Expect "CtlAffInvertAdjSpec" [ Warning WarningCode.MissingBodyDeclaration ]
        this.Expect "CtlAffSelfAdjSpec" [ Warning WarningCode.MissingBodyDeclaration ]
        this.Expect "AdjAffAutoCtlSpec" [ Warning WarningCode.MissingBodyDeclaration ]
        this.Expect "AdjAffDistrCtlSpec" [ Warning WarningCode.MissingBodyDeclaration ]

        this.Expect "OperationCharacteristics1" []
        this.Expect "OperationCharacteristics2" [ Error ErrorCode.InvalidAdjointApplication ]

        this.Expect
            "OperationCharacteristics3"
            [
                Error ErrorCode.InvalidControlledApplication
                Error ErrorCode.TypeMismatch
                Error ErrorCode.TypeMismatch
            ]

        this.Expect
            "OperationCharacteristics4"
            [
                Error ErrorCode.InvalidControlledApplication
                Error ErrorCode.TypeMismatch
                Error ErrorCode.TypeMismatch
            ]

        this.Expect "OperationCharacteristics5" [ Error ErrorCode.InvalidAdjointApplication ]
        this.Expect "OperationCharacteristics6" [ Error ErrorCode.InvalidAdjointApplication ]
        this.Expect "OperationCharacteristics7" [ Error ErrorCode.InvalidAdjointApplication ]
        this.Expect "OperationCharacteristics8" [ Error ErrorCode.InvalidAdjointApplication ]
        this.Expect "OperationCharacteristics9" [ Error ErrorCode.InvalidControlledApplication ]
        this.Expect "OperationCharacteristics10" [ Error ErrorCode.InvalidControlledApplication ]
        this.Expect "OperationCharacteristics11" [ Error ErrorCode.InvalidControlledApplication ]
        this.Expect "OperationCharacteristics12" [ Error ErrorCode.InvalidAdjointApplication ]
        this.Expect "OperationCharacteristics13" [ Error ErrorCode.InvalidAdjointApplication ]
        this.Expect "OperationCharacteristics14" [ Error ErrorCode.InvalidControlledApplication ]
        this.Expect "OperationCharacteristics15" [ Error ErrorCode.InvalidControlledApplication ]
        this.Expect "OperationCharacteristics16" []
        this.Expect "OperationCharacteristics17" []
        this.Expect "OperationCharacteristics18" []
        this.Expect "OperationCharacteristics19" [ Error ErrorCode.InvalidAdjointApplication ]
        this.Expect "OperationCharacteristics20" [ Error ErrorCode.InvalidAdjointApplication ]
        this.Expect "OperationCharacteristics21" [ Error ErrorCode.InvalidControlledApplication ]
        this.Expect "OperationCharacteristics22" [ Error ErrorCode.InvalidControlledApplication ]
        this.Expect "OperationCharacteristics23" [ Error ErrorCode.InvalidControlledApplication ]


    [<Fact>]
    member this.``Generator directives``() =

        this.Expect "AdjointGenDirective1" []
        this.Expect "AdjointGenDirective2" []
        this.Expect "AdjointGenDirective3" [ Error ErrorCode.DistributedAdjointGenerator ]
        this.Expect "AdjointGenDirective4" []
        this.Expect "AdjointGenDirective5" []
        this.Expect "AdjointGenDirective6" [ Warning WarningCode.GeneratorDirectiveWillBeIgnored ]

        this.Expect
            "AdjointGenDirective7"
            [
                Error ErrorCode.DistributedAdjointGenerator
                Warning WarningCode.GeneratorDirectiveWillBeIgnored
            ]

        this.Expect "AdjointGenDirective8" []

        this.Expect "ControlledGenDirective1" [ Error ErrorCode.SelfControlledGenerator ]
        this.Expect "ControlledGenDirective2" [ Error ErrorCode.InvertControlledGenerator ]
        this.Expect "ControlledGenDirective3" []
        this.Expect "ControlledGenDirective4" []
        this.Expect "ControlledGenDirective5" [ Error ErrorCode.SelfControlledGenerator ]

        this.Expect
            "ControlledGenDirective6"
            [
                Error ErrorCode.InvertControlledGenerator
                Warning WarningCode.GeneratorDirectiveWillBeIgnored
            ]

        this.Expect "ControlledGenDirective7" [ Warning WarningCode.GeneratorDirectiveWillBeIgnored ]
        this.Expect "ControlledGenDirective8" []

        this.Expect "ControlledAdjointGenDirective1" []
        this.Expect "ControlledAdjointGenDirective2" []
        this.Expect "ControlledAdjointGenDirective3" []
        this.Expect "ControlledAdjointGenDirective4" []
        this.Expect "ControlledAdjointGenDirective5" []
        this.Expect "ControlledAdjointGenDirective6" [ Warning WarningCode.GeneratorDirectiveWillBeIgnored ]
        this.Expect "ControlledAdjointGenDirective7" [ Warning WarningCode.GeneratorDirectiveWillBeIgnored ]
        this.Expect "ControlledAdjointGenDirective8" []
        this.Expect "ControlledAdjointGenDirective9" []
        this.Expect "ControlledAdjointGenDirective10" [ Error ErrorCode.NonSelfGeneratorForSelfadjoint ]
        this.Expect "ControlledAdjointGenDirective11" [ Error ErrorCode.NonSelfGeneratorForSelfadjoint ]
        this.Expect "ControlledAdjointGenDirective12" [ Error ErrorCode.NonSelfGeneratorForSelfadjoint ]
        this.Expect "ControlledAdjointGenDirective13" [ Error ErrorCode.NonSelfGeneratorForSelfadjoint ]
        this.Expect "ControlledAdjointGenDirective14" [ Error ErrorCode.NonSelfGeneratorForSelfadjoint ]

        this.Expect "Intrinsic1" [ Error ErrorCode.UserDefinedImplementationForIntrinsic ]
        this.Expect "Intrinsic2" [ Error ErrorCode.UserDefinedImplementationForIntrinsic ]
        this.Expect "Intrinsic3" [ Error ErrorCode.UserDefinedImplementationForIntrinsic ]
        this.Expect "Intrinsic4" [ Error ErrorCode.UserDefinedImplementationForIntrinsic ]
        this.Expect "Intrinsic5" [ Error ErrorCode.UserDefinedImplementationForIntrinsic ]
        this.Expect "Intrinsic6" [ Error ErrorCode.UserDefinedImplementationForIntrinsic ]

    // TODO: If external specializations are supported we need to make sure that
    // "intrinsic" is not a valid generator for external specializations.
    // Similarly, we need to properly generate errors when the auto generation of external specializations fails.


    [<Fact>]
    member this.``Feasibility check``() =

        this.Expect "NeedsUnitReturn1" [ Error ErrorCode.RequiredUnitReturnForAdjoint ]
        this.Expect "NeedsUnitReturn2" [ Error ErrorCode.RequiredUnitReturnForControlled ]
        this.Expect "NeedsUnitReturn3" [ Error ErrorCode.RequiredUnitReturnForControlledAdjoint ]
        this.Expect "NeedsUnitReturn4" [ Error ErrorCode.RequiredUnitReturnForAdjoint ]
        this.Expect "NeedsUnitReturn5" [ Error ErrorCode.RequiredUnitReturnForAdjoint ]
        this.Expect "NeedsUnitReturn6" [ Error ErrorCode.RequiredUnitReturnForControlled ]
        this.Expect "NeedsUnitReturn7" [ Error ErrorCode.RequiredUnitReturnForAdjoint ]
        this.Expect "CallNeedsUnitReturn1" []
        this.Expect "CallNeedsUnitReturn2" [ Error ErrorCode.InvalidAdjointApplication ]
        this.Expect "CallNeedsUnitReturn3" []
        this.Expect "CallNeedsUnitReturn4" [ Error ErrorCode.InvalidAdjointApplication ]
        this.Expect "UnitReturn1" []
        this.Expect "UnitReturn2" []
        this.Expect "UnitReturn3" []
        this.Expect "UnitReturn4" []
        this.Expect "UnitReturn5" []
        this.Expect "UnitReturn6" []

        this.Expect "NeedsFunctorSupport1" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "NeedsFunctorSupport2" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "NeedsFunctorSupport3" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "NeedsFunctorSupport4" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "NeedsFunctorSupport5" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "NeedsFunctorSupport6" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "NeedsFunctorSupport7" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "NeedsFunctorSupport8" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "NeedsFunctorSupport9" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "NeedsFunctorSupport10" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "NeedsFunctorSupport11" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "NeedsFunctorSupport12" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "NeedsFunctorSupport13" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "NeedsFunctorSupport14" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "NeedsFunctorSupport15" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "NeedsFunctorSupport16" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "NeedsFunctorSupport17" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "FunctorSupport1" []
        this.Expect "FunctorSupport2" []
        this.Expect "FunctorSupport3" []
        this.Expect "FunctorSupport4" []
        this.Expect "FunctorSupport5" []
        this.Expect "FunctorSupport6" []
        this.Expect "FunctorSupport7" []
        this.Expect "FunctorSupport8" []
        this.Expect "FunctorSupport9" []
        this.Expect "FunctorSupport10" []
        this.Expect "FunctorSupport11" []
        this.Expect "FunctorSupport12" []
        this.Expect "FunctorSupport13" []
        this.Expect "FunctorSupport14" []
        this.Expect "FunctorSupport15" []
        this.Expect "FunctorSupport16" []
        this.Expect "FunctorSupport17" []
        this.Expect "FunctorSupport18" []
        this.Expect "FunctorSupport19" []
        this.Expect "FunctorSupport20" []
        this.Expect "FunctorSupport21" []
        this.Expect "FunctorSupport22" []
        this.Expect "FunctorSupport23" []

        this.Expect "VariableNeedsFunctorSupport1" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "VariableNeedsFunctorSupport2" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "VariableNeedsFunctorSupport3" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "VariableNeedsFunctorSupport4" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "VariableWithFunctorSupport1" []
        this.Expect "VariableWithFunctorSupport2" []
        this.Expect "VariableWithFunctorSupport3" []
        this.Expect "VariableWithFunctorSupport4" []

        this.Expect "InvalidAutoInversion1" [ Error ErrorCode.ReturnStatementWithinAutoInversion ]
        this.Expect "InvalidAutoInversion2" [ Error ErrorCode.RUSloopWithinAutoInversion ]
        this.Expect "InvalidAutoInversion3" [ Error ErrorCode.ValueUpdateWithinAutoInversion ]
        this.Expect "InvalidAutoInversion4" [ Error ErrorCode.ReturnStatementWithinAutoInversion ]
        this.Expect "InvalidAutoInversion5" [ Error ErrorCode.RUSloopWithinAutoInversion ]
        this.Expect "InvalidAutoInversion6" [ Error ErrorCode.ValueUpdateWithinAutoInversion ]
        this.Expect "InvalidAutoInversion7" [ Error ErrorCode.ReturnStatementWithinAutoInversion ]
        this.Expect "InvalidAutoInversion8" [ Error ErrorCode.RUSloopWithinAutoInversion ]
        this.Expect "InvalidAutoInversion9" [ Error ErrorCode.ValueUpdateWithinAutoInversion ]
        this.Expect "InvalidAutoInversion10" [ Error ErrorCode.ReturnStatementWithinAutoInversion ]
        this.Expect "InvalidAutoInversion11" [ Error ErrorCode.RUSloopWithinAutoInversion ]
        this.Expect "InvalidAutoInversion12" [ Error ErrorCode.ValueUpdateWithinAutoInversion ]
        this.Expect "InvalidAutoInversion13" [ Error ErrorCode.ReturnStatementWithinAutoInversion ]
        this.Expect "InvalidAutoInversion14" [ Error ErrorCode.RUSloopWithinAutoInversion ]
        this.Expect "InvalidAutoInversion15" [ Error ErrorCode.ValueUpdateWithinAutoInversion ]
        this.Expect "InvalidAutoInversion16" [ Error ErrorCode.ReturnStatementWithinAutoInversion ]
        this.Expect "InvalidAutoInversion17" [ Error ErrorCode.ValueUpdateWithinAutoInversion ]
        this.Expect "InvalidAutoInversion18" [ Error ErrorCode.RUSloopWithinAutoInversion ]
        this.Expect "ValidInversion1" []
        this.Expect "ValidInversion2" []
        this.Expect "ValidInversion3" []
        this.Expect "ValidInversion4" []
        this.Expect "ValidInversion5" [ Error ErrorCode.ReturnFromWithinApplyBlock ]
        this.Expect "ValidInversion6" []
        this.Expect "ValidInversion7" []
        this.Expect "ValidInversion8" []

        this.Expect "WithInvalidQuantumDependency1" [ Error ErrorCode.QuantumDependencyOutsideExprStatement ]
        this.Expect "WithInvalidQuantumDependency2" [ Error ErrorCode.QuantumDependencyOutsideExprStatement ]
        this.Expect "WithInvalidQuantumDependency3" [ Error ErrorCode.QuantumDependencyOutsideExprStatement ]

        this.Expect
            "WithInvalidQuantumDependency4"
            [
                Error ErrorCode.QuantumDependencyOutsideExprStatement
                Error ErrorCode.MissingFunctorForAutoGeneration
            ]

        this.Expect
            "WithInvalidQuantumDependency5"
            [
                Error ErrorCode.QuantumDependencyOutsideExprStatement
                Error ErrorCode.MissingFunctorForAutoGeneration
            ]

        this.Expect
            "WithInvalidQuantumDependency6"
            [
                Error ErrorCode.QuantumDependencyOutsideExprStatement
                Error ErrorCode.MissingFunctorForAutoGeneration
            ]

        this.Expect
            "WithInvalidQuantumDependency7"
            [
                Error ErrorCode.QuantumDependencyOutsideExprStatement
                Error ErrorCode.MissingFunctorForAutoGeneration
            ]

        this.Expect "WithoutInvalidQuantumDependency1" []
        this.Expect "WithoutInvalidQuantumDependency2" []
        this.Expect "WithoutInvalidQuantumDependency3" []
        this.Expect "WithoutInvalidQuantumDependency4" []
        this.Expect "WithoutInvalidQuantumDependency5" []
        this.Expect "WithoutInvalidQuantumDependency6" []
        this.Expect "WithoutInvalidQuantumDependency7" []

        this.Expect "InvalidControlled1" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "InvalidControlled2" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "InvalidControlled3" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "InvalidControlled4" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "ValidControlled1" []
        this.Expect "ValidControlled2" []
        this.Expect "ValidControlled3" []
        this.Expect "ValidControlled4" []
        this.Expect "ValidControlled5" []
        this.Expect "ValidControlled6" []
        this.Expect "ValidControlled7" []
        this.Expect "ValidControlled8" []
        this.Expect "ValidControlled9" []

        this.Expect "InvalidControlledAdjointGeneration1" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "InvalidControlledAdjointGeneration2" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "InvalidControlledAdjointGeneration3" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "InvalidControlledAdjointGeneration4" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "InvalidControlledAdjointGeneration5" [ Error ErrorCode.NonSelfGeneratorForSelfadjoint ]
        this.Expect "InvalidControlledAdjointGeneration6" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "InvalidControlledAdjointGeneration7" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "ValidControlledAdjointGeneration1" []
        this.Expect "ValidControlledAdjointGeneration2" []
        this.Expect "ValidControlledAdjointGeneration3" []
        this.Expect "ValidControlledAdjointGeneration4" []
        this.Expect "ValidControlledAdjointGeneration5" []

        this.Expect "InvalidMutalRecursion1a" []
        this.Expect "InvalidMutalRecursion1b" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "InvalidMutalRecursion2a" []
        this.Expect "InvalidMutalRecursion2b" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "InvalidMutalRecursion3a" []
        this.Expect "InvalidMutalRecursion3b" [ Error ErrorCode.MissingFunctorForAutoGeneration ]
        this.Expect "MutalRecursion1a" []
        this.Expect "MutalRecursion1b" []
        this.Expect "MutalRecursion2a" []
        this.Expect "MutalRecursion2b" []
        this.Expect "MutalRecursion3a" []
        this.Expect "MutalRecursion3b" []
        this.Expect "Recursion1" []
        this.Expect "Recursion2" []
        this.Expect "Recursion3" []
