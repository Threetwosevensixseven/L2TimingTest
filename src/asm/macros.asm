; macros.asm

BORDER                  MACRO Colour?
                            IF Colour? == 0
                                xor a
                            ELSE
                                ld a, Colour?
                            ENDIF
                            out (ULA_PORT), a
                        ENDM

BORDER2                 MACRO Colour?
                          ld a, Colour?
                          out (ULA_PORT), a
                        ENDM                        

FREEZE                  MACRO Colour1?, Colour2?
.Loop                       BORDER Colour1?
                            BORDER Colour2?
                            jr .Loop
                        ENDM

CSBREAK                 MACRO                           ; Intended for CSpect debugging.
                            db $DD, $01                 ; Enabled when the -brk switch is supplied.
                        ENDM                            ; Remove before running on real hardware!

CSBREAKSAFE             MACRO                           ; Intended for CSpect debugging.
                            push bc                     ; Enabled when the -brk switch is supplied.
                            db $DD, $01                 ; Mitigates worst effects of running on real hardware.
                            nop                         ; On real Z80 or Z80N, DD 01 does NOP:LD BC, NNNN,
                            nop                         ; so we set safe values for NNNN,
                            pop bc                      ; then we restore the value of BC we saved earlier. 
                        ENDM

MFBREAK                 MACRO                           ; Intended for NextZXOS NMI debugging.
                            nextreg 2, 8                ; This only works in Next core 3.01.10 and above.
                        ENDM                            ; A different macro is needed for older cores.

NRREAD                  MACRO Register?                 ; Nextregs have to be read through the register I/O port pair,
                        ld bc, $243B                    ; as there is no dedicated ZX80N opcode like there is for
                        ld a, Register?                 ; writes.
                        out (c), a
                        inc b
                        in a, (c)
                        ENDM
