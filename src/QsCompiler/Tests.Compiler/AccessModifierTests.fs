﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Quantum.QsCompiler.Testing

open System.Collections.Generic
open Microsoft.Quantum.QsCompiler.DataTypes
open Microsoft.Quantum.QsCompiler.Diagnostics
open Microsoft.Quantum.QsCompiler.SyntaxExtensions
open Microsoft.Quantum.QsCompiler.SyntaxTree
open Xunit
open System.IO
open System.Linq


type AccessModifierTests (output) =
    inherit CompilerTests (CompilerTests.Compile "TestCases"
                                                 ["AccessModifiers.qs"]
                                                 [File.ReadAllLines("ReferenceTargets.txt").ElementAt(1)],
                           output)

    member private this.Expect name (diagnostics : IEnumerable<DiagnosticItem>) = 
        let ns = "Microsoft.Quantum.Testing.AccessModifiers" |> NonNullable<_>.New
        let name = name |> NonNullable<_>.New
        this.Verify (QsQualifiedName.New (ns, name), diagnostics)

    [<Fact>]
    member this.``Redefine inaccessible symbols in reference`` () =
        this.Expect "T1" []
        this.Expect "T2" []
        this.Expect "F1" []
        this.Expect "F2" []

    [<Fact>]
    member this.``Callables with access modifiers`` () =
        this.Expect "CallableUseOK" []
        this.Expect "CallableUnqualifiedUsePrivateInaccessible" [Error ErrorCode.InaccessibleCallable]
        this.Expect "CallableQualifiedUsePrivateInaccessible" [Error ErrorCode.InaccessibleCallableInNamespace]
        this.Expect "CallableReferencePrivateInaccessible" [Error ErrorCode.InaccessibleCallable]
        this.Expect "CallableReferenceInternalInaccessible" [Error ErrorCode.InaccessibleCallable]

    [<Fact>]
    member this.``Types with access modifiers`` () =
        this.Expect "TypeUseOK" []
        this.Expect "TypeUnqualifiedUsePrivateInaccessible" [Error ErrorCode.InaccessibleType]
        this.Expect "TypeQualifiedUsePrivateInaccessible" [Error ErrorCode.InaccessibleTypeInNamespace]
        this.Expect "TypeReferencePrivateInaccessible" [Error ErrorCode.InaccessibleType]
        this.Expect "TypeReferenceInternalInaccessible" [Error ErrorCode.InaccessibleType]

    [<Fact>]
    member this.``Callable signatures`` () =
        this.Expect "PublicCallableLeaksPrivateTypeIn1" [Error ErrorCode.TypeLessAccessibleThanParentCallable]
        this.Expect "PublicCallableLeaksPrivateTypeIn2" [Error ErrorCode.TypeLessAccessibleThanParentCallable]
        this.Expect "PublicCallableLeaksPrivateTypeOut1" [Error ErrorCode.TypeLessAccessibleThanParentCallable]
        this.Expect "PublicCallableLeaksPrivateTypeOut2" [Error ErrorCode.TypeLessAccessibleThanParentCallable]
        this.Expect "InternalCallableLeaksPrivateTypeIn" [Error ErrorCode.TypeLessAccessibleThanParentCallable]
        this.Expect "InternalCallableLeaksPrivateTypeOut" [Error ErrorCode.TypeLessAccessibleThanParentCallable]
        this.Expect "CallablePrivateTypeOK" []
        this.Expect "CallableLeaksInternalTypeIn" [Error ErrorCode.TypeLessAccessibleThanParentCallable]
        this.Expect "CallableLeaksInternalTypeOut" [Error ErrorCode.TypeLessAccessibleThanParentCallable]
        this.Expect "InternalCallableInternalTypeOK" []
        this.Expect "PrivateCallableInternalTypeOK" []

    [<Fact>]
    member this.``Underlying types`` () =
        this.Expect "PublicTypeLeaksPrivateType1" [Error ErrorCode.TypeLessAccessibleThanParentType]
        this.Expect "PublicTypeLeaksPrivateType2" [Error ErrorCode.TypeLessAccessibleThanParentType]
        this.Expect "PublicTypeLeaksPrivateType3" [Error ErrorCode.TypeLessAccessibleThanParentType]
        this.Expect "InternalTypeLeaksPrivateType" [Error ErrorCode.TypeLessAccessibleThanParentType]
        this.Expect "PrivateTypePrivateTypeOK" []
        this.Expect "PublicTypeLeaksInternalType" [Error ErrorCode.TypeLessAccessibleThanParentType]
        this.Expect "InternalTypeInternalTypeOK" []
        this.Expect "PrivateTypeInternalTypeOK" []
