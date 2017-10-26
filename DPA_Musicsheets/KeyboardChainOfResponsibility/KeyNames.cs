using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyboardChainOfResponsibility;

namespace KeyboardChainOfResponsibility.KeyNames
{
    // this class defines Modifiers  (CTRL and ALT)
    //                ... Characters (A, B, C ..., Z)
    //                ... Numbers    (0, 1, 2 ..., 9)
    // the Enums have been defined using binary
    // making it so that you can safely OR 
    // any Modifier with any single Character with any single number
    // Modifiers.CTRL | Characters.A | Numbers.ONE
    // You always use Three Enums, and Always Two logical or's, 
    // At least one Modifier (either CTRL or ALT)
    // At least one Character (A-Z)
    // and optionally a number, use Numbers.None if you don't have one.

    // NOTE: Numbers are defined in Enums using words, IE Numbers.ONE, Numbers.TWO ...
    // because Enums don't support left-hand-side literals, for hopefully obvious reasons.

    // Modifiers are defined in 0b0000_0000_0000_00xx
    //        A-Z is defined in 0bxxxx_xxxx_0000_0000
    //        0-9 is defined in 0b0000_0000_xxxx_xx00
    //

    // Modifiers are defined in 0b0000_0000_0000_00xx
    public enum Modifiers
    {
        CTRL = 0b0000_0000_0000_0001,
        ALT  = 0b0000_0000_0000_0010
    }

    // A-Z is defined in 0bxxxx_xxxx_0000_0000
    public enum Characters
    {
        A = 0b0100_0001_0000_0000,
        B = 0b0100_0010_0000_0000,
        C = 0b0100_0011_0000_0000,
        D = 0b0100_0100_0000_0000,
        E = 0b0100_0101_0000_0000,
        F = 0b0100_0110_0000_0000,
        G = 0b0100_0111_0000_0000,
        H = 0b0100_1000_0000_0000,
        I = 0b0100_1001_0000_0000,
        J = 0b0100_1010_0000_0000,
        K = 0b0100_1011_0000_0000,
        L = 0b0100_1100_0000_0000,
        M = 0b0100_1101_0000_0000,
        N = 0b0100_1110_0000_0000,
        O = 0b0100_1111_0000_0000,
        P = 0b0101_0000_0000_0000,
        Q = 0b0101_0001_0000_0000,
        R = 0b0101_0010_0000_0000,
        S = 0b0101_0011_0000_0000,
        T = 0b0101_0100_0000_0000,
        U = 0b0101_0101_0000_0000,
        V = 0b0101_0110_0000_0000,
        W = 0b0101_0111_0000_0000,
        X = 0b0101_1000_0000_0000,
        Y = 0b0101_1001_0000_0000,
        Z = 0b0101_1010_0000_0000
    }

    // 0-9 is defined in 0b0000_0000_xxxx_xx00
    public enum Numbers
    {
        NONE    = 0,
        ZERO    = 0b0000_0000_1100_0000,
        ONE     = 0b0000_0000_1100_0100,
        TWO     = 0b0000_0000_1100_1000,
        THREE   = 0b0000_0000_1100_1100,
        FOUR    = 0b0000_0000_1101_0000,
        FIVE    = 0b0000_0000_1101_0100,
        SIX     = 0b0000_0000_1101_1000,
        SEVEN   = 0b0000_0000_1101_1100,
        EIGHT   = 0b0000_0000_1110_0000,
        NINE    = 0b0000_0000_1110_0100
    }
}
