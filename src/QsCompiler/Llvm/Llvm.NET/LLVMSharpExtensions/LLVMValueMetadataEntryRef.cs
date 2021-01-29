// Copyright (c) Microsoft and Contributors. All rights reserved. Licensed under the University of Illinois/NCSA Open Source License. See LICENSE.txt in the project root for license information.

using System;

namespace LLVMSharp.Interop
{
    /// <summary>Support for refs to LLVMOpaqueValueMetadataEntry*</summary>
    public unsafe partial struct LLVMValueMetadataEntryRef : IEquatable<LLVMValueMetadataEntryRef>
    {
        /// <summary>Pointer to the underlying native type</summary>
        public IntPtr Handle;

        /// <summary>Contstructor</summary>
        public LLVMValueMetadataEntryRef(IntPtr handle)
        {
            Handle = handle;
        }

        /// <summary>Conversion support for LLVMOpaqueValueMetadataEntry*</summary>
        public static implicit operator LLVMValueMetadataEntryRef(LLVMOpaqueValueMetadataEntry* value) => new LLVMValueMetadataEntryRef((IntPtr)value);

        /// <summary>Conversion support for LLVMOpaqueValueMetadataEntry*</summary>
        public static implicit operator LLVMOpaqueValueMetadataEntry*(LLVMValueMetadataEntryRef value) => (LLVMOpaqueValueMetadataEntry*)value.Handle;

        /// <summary>Basic equality comparison support</summary>
        public static bool operator ==(LLVMValueMetadataEntryRef left, LLVMValueMetadataEntryRef right) => left.Handle == right.Handle;

        /// <summary>Basic inequality comparison support</summary>
        public static bool operator !=(LLVMValueMetadataEntryRef left, LLVMValueMetadataEntryRef right) => !(left == right);

        /// <summary>Basic equality comparison support</summary>
        public override bool Equals(object obj) => (obj is LLVMValueMetadataEntryRef other) && Equals(other);

        /// <summary>Basic equality comparison support</summary>
        public bool Equals(LLVMValueMetadataEntryRef other) => this == other;

        /// <summary>Basic hash code support</summary>
        public override int GetHashCode() => Handle.GetHashCode();

        /// <summary>Basic string representation support</summary>
        public override string ToString() => $"{nameof(LLVMValueMetadataEntryRef)}: {Handle:X}";

        /// <summary>Convenience wrapper for LLVm.ValueMetadataEntriesGetMetadata</summary>
        public LLVMMetadataRef ValueMetadataEntriesGetMetadata( uint i ) => ( this.Handle != default ) ? LLVM.ValueMetadataEntriesGetMetadata( this, i ) : default;
    }
}
