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
                            db $FD, $00                 ; Enabled when the -brk switch is supplied.
                        ENDM                            ; Remove before running on real hardware!

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

FILLLDIR                MACRO Addr?, Size?, Value?
                            ld hl, Addr?
                            ld (hl), Value?
                            ld de, Addr?+1
                            ld bc, Size?-1
                            ldir
                        ENDM 

SETUPL2                 MACRO Bank?, Colour?
                            ld a, Bank?
                            ld b, Colour?
                            call FillBank
                        ENDM                 
