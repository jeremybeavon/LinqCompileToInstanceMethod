using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ClrTest.Reflection;

namespace LinqCompileToInstanceMethod
{
    internal sealed class ILTransform : ILInstructionVisitor
    {
        private readonly ILGenerator ilGenerator;

        public ILTransform(ILGenerator ilGenerator)
        {
            this.ilGenerator = ilGenerator;
        }

        public override void VisitInlineBrTargetInstruction(InlineBrTargetInstruction inlineBrTargetInstruction)
        {
            throw new NotSupportedException();
        }

        public override void VisitInlineFieldInstruction(InlineFieldInstruction inlineFieldInstruction)
        {
            ilGenerator.Emit(inlineFieldInstruction.OpCode, inlineFieldInstruction.Field);
        }

        public override void VisitInlineI8Instruction(InlineI8Instruction inlineI8Instruction)
        {
            ilGenerator.Emit(inlineI8Instruction.OpCode, inlineI8Instruction.Int64);
        }

        public override void VisitInlineIInstruction(InlineIInstruction inlineIInstruction)
        {
            ilGenerator.Emit(inlineIInstruction.OpCode, inlineIInstruction.Int32);
        }

        public override void VisitInlineMethodInstruction(InlineMethodInstruction inlineMethodInstruction)
        {
            MethodInfo method = inlineMethodInstruction.Method as MethodInfo;
            if (method != null)
            {
                ilGenerator.Emit(inlineMethodInstruction.OpCode, method);
                return;
            }

            ConstructorInfo constructor = inlineMethodInstruction.Method as ConstructorInfo;
            if (constructor == null)
            {
                throw new NotSupportedException();
            }

            ilGenerator.Emit(inlineMethodInstruction.OpCode, constructor);
        }

        public override void VisitInlineNoneInstruction(InlineNoneInstruction inlineNoneInstruction)
        {
            ilGenerator.Emit(inlineNoneInstruction.OpCode);
        }

        public override void VisitInlineRInstruction(InlineRInstruction inlineRInstruction)
        {
            ilGenerator.Emit(inlineRInstruction.OpCode, inlineRInstruction.Double);
        }

        public override void VisitInlineSigInstruction(InlineSigInstruction inlineSigInstruction)
        {
            throw new NotSupportedException();
        }

        public override void VisitInlineStringInstruction(InlineStringInstruction inlineStringInstruction)
        {
            ilGenerator.Emit(inlineStringInstruction.OpCode, inlineStringInstruction.String);
        }

        public override void VisitInlineSwitchInstruction(InlineSwitchInstruction inlineSwitchInstruction)
        {
            throw new NotSupportedException();
        }

        public override void VisitInlineTokInstruction(InlineTokInstruction inlineTokInstruction)
        {
            throw new NotSupportedException();
        }

        public override void VisitInlineTypeInstruction(InlineTypeInstruction inlineTypeInstruction)
        {
            ilGenerator.Emit(inlineTypeInstruction.OpCode, inlineTypeInstruction.Type);
        }

        public override void VisitInlineVarInstruction(InlineVarInstruction inlineVarInstruction)
        {
            ilGenerator.Emit(inlineVarInstruction.OpCode, inlineVarInstruction.Ordinal);
        }

        public override void VisitShortInlineBrTargetInstruction(ShortInlineBrTargetInstruction shortInlineBrTargetInstruction)
        {
            throw new NotSupportedException();
        }

        public override void VisitShortInlineIInstruction(ShortInlineIInstruction shortInlineIInstruction)
        {
            ilGenerator.Emit(shortInlineIInstruction.OpCode, shortInlineIInstruction.Byte);
        }

        public override void VisitShortInlineRInstruction(ShortInlineRInstruction shortInlineRInstruction)
        {
            ilGenerator.Emit(shortInlineRInstruction.OpCode, shortInlineRInstruction.Single);
        }

        public override void VisitShortInlineVarInstruction(ShortInlineVarInstruction shortInlineVarInstruction)
        {
            ilGenerator.Emit(shortInlineVarInstruction.OpCode, shortInlineVarInstruction.Ordinal);
        }
    }
}
