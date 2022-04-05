﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Quantum.QsCompiler.SyntaxProcessing.TypeInference

open Microsoft.Quantum.QsCompiler.DataTypes
open Microsoft.Quantum.QsCompiler.SyntaxTokens
open Microsoft.Quantum.QsCompiler.SyntaxTree
open Microsoft.Quantum.QsCompiler.Transformations.QsCodeOutput

type ClassConstraint =
    | Adjointable of operation: ResolvedType
    | Callable of callable: ResolvedType * input: ResolvedType * output: ResolvedType
    | Controllable of operation: ResolvedType * controlled: ResolvedType
    | Eq of ResolvedType
    | GenerateFunctors of callable: ResolvedType * functors: QsFunctor Set
    | HasField of record: ResolvedType * field: Identifier * item: ResolvedType
    | HasIndex of container: ResolvedType * index: ResolvedType * item: ResolvedType
    | Integral of ResolvedType
    | Iterable of container: ResolvedType * item: ResolvedType
    | Num of ResolvedType
    | PartialAp of callable: ResolvedType * missing: ResolvedType * callable': ResolvedType
    | Semigroup of ResolvedType
    | Unwrap of container: ResolvedType * item: ResolvedType

module ClassConstraint =
    let types =
        function
        | Adjointable operation -> [ operation ]
        | Callable (callable, input, output) -> [ callable; input; output ]
        | Controllable (operation, controlled) -> [ operation; controlled ]
        | Eq ty -> [ ty ]
        | GenerateFunctors (callable, _) -> [ callable ]
        | HasField (record, _, item) -> [ record; item ]
        | HasIndex (container, index, item) -> [ container; index; item ]
        | Integral ty -> [ ty ]
        | Iterable (container, item) -> [ container; item ]
        | Num ty -> [ ty ]
        | PartialAp (callable, missing, callable') -> [ callable; missing; callable' ]
        | Semigroup ty -> [ ty ]
        | Unwrap (container, item) -> [ container; item ]

    let pretty cls =
        let p: ResolvedType -> _ = SyntaxTreeToQsharp.Default.ToCode

        match cls with
        | Adjointable operation -> sprintf "Adjointable<%s>" (p operation)
        | Callable (callable, input, output) -> sprintf "Callable<%s, %s, %s>" (p callable) (p input) (p output)
        | Controllable (operation, controlled) -> sprintf "Controllable<%s, %s>" (p operation) (p controlled)
        | Eq ty -> sprintf "Eq<%s>" (p ty)
        | GenerateFunctors (callable, functors) ->
            let functors = Seq.map string functors |> String.concat ", "
            sprintf "GenerateFunctors<%s, {%s}>" (p callable) functors
        | HasField (record, field, item) ->
            let field = Identifier(field, Null) |> SyntaxTreeToQsharp.Default.ToCode
            sprintf "HasField<%s, \"%s\", %s>" (p record) field (p item)
        | HasIndex (container, index, item) -> sprintf "HasIndex<%s, %s, %s>" (p container) (p index) (p item)
        | Integral ty -> sprintf "Integral<%s>" (p ty)
        | Iterable (container, item) -> sprintf "Iterable<%s, %s>" (p container) (p item)
        | Num ty -> sprintf "Num<%s>" (p ty)
        | PartialAp (callable, missing, callable') ->
            sprintf "PartialAp<%s, %s, %s>" (p callable) (p missing) (p callable')
        | Semigroup ty -> sprintf "Semigroup<%s>" (p ty)
        | Unwrap (container, item) -> sprintf "Unwrap<%s, %s>" (p container) (p item)

type Ordering =
    | Subtype
    | Equal
    | Supertype

module Ordering =
    let reverse =
        function
        | Subtype -> Supertype
        | Equal -> Equal
        | Supertype -> Subtype

type Constraint =
    | Class of ClassConstraint
    | Relation of ResolvedType * Ordering * ResolvedType

module Constraint =
    let types =
        function
        | Class cls -> ClassConstraint.types cls
        | Relation (lhs, _, rhs) -> [ lhs; rhs ]

module RelationOps =
    let (<.) lhs rhs = Relation(lhs, Subtype, rhs)

    let (.=) lhs rhs = Relation(lhs, Equal, rhs)

    let (.>) lhs rhs = Relation(lhs, Supertype, rhs)
